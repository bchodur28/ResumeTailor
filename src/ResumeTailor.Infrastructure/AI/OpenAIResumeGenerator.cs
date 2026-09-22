#pragma warning disable OPENAI001

using OpenAI.Responses;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.ClientModel;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Domain.GeneratedResumes.AI;

namespace ResumeTailor.Infrastructure.AI;

public sealed class OpenAIResumeGenerator(ResponsesClient client, IOptions<OpenAIOptions> options) : IResumeAiGenerator
{
    private const decimal InputCostPerMillionTokens = 2.00m;
    private const decimal OutputCostPerMillionTokens = 12.00m;
    public async Task<ResumeAiGenerationResult> GenerateAsync(ResumeAiGenerationContext context, CancellationToken cancellationToken = default)
    {
        var prompt = BuildPrompt(context);

        var responseOptions = new CreateResponseOptions
        {
            Model = options.Value.Model,
            TextOptions = new ResponseTextOptions
            {
                TextFormat = ResponseTextFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "resume_analysis",
                    jsonSchema: BinaryData.FromString("""
                    {
                      "type": "object",
                      "properties": {
                        "score": {
                          "type": "integer",
                          "minimum": 0,
                          "maximum": 100
                        },
                        "summary": {
                          "type": "string"
                        },
                        "companies": {
                          "type": "array",
                          "items": {
                            "type": "object",
                            "properties": {
                              "company": {
                                "type": "string"
                              },
                              "bullets": {
                                "type": "array",
                                "items": {
                                  "type": "object",
                                  "properties": {
                                    "value": {
                                      "type": "string"
                                    },
                                    "alternative": {
                                      "type": ["string", "null"]
                                    }
                                  },
                                  "required": [
                                    "value",
                                    "alternative"
                                  ],
                                  "additionalProperties": false
                                }
                              }
                            },
                            "required": [
                              "company",
                              "bullets"
                            ],
                            "additionalProperties": false
                          }
                        },
                        "strengths": {
                          "type": "array",
                          "items": {
                            "type": "string"
                          },
                          "maxItems": 5
                        },
                        "weaknesses": {
                          "type": "array",
                          "items": {
                            "type": "string"
                          },
                          "maxItems": 5
                        }
                      },
                      "required": [
                        "score",
                        "summary",
                        "companies",
                        "strengths",
                        "weaknesses"
                      ],
                      "additionalProperties": false
                    }
                    """))
            }
        };

        responseOptions.InputItems.Add(ResponseItem.CreateUserMessageItem(prompt));

        var response = await client.CreateResponseAsync(responseOptions, cancellationToken);

        var json = response.Value.GetOutputText();

        var aiResponse = JsonSerializer.Deserialize<AiResumeResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("OpenAI returned an empty resume analysis response.");

        var companies = ValidateAndMapCompanies(aiResponse.Companies, context.CompanyBulletContexts);

        var strengths = aiResponse.Strengths
            .Take(5)
            .Select(strength => new ResumeAiInsightResult(ResumeAiInsightType.Strength, strength))
            .ToList();

        var weaknesses = aiResponse.Weaknesses
            .Take(5)
            .Select(weakness => new ResumeAiInsightResult(ResumeAiInsightType.Weakness, weakness))
            .ToList();

        var usage = CreateAiUsage(response);

        return new ResumeAiGenerationResult(
            aiResponse.Score,
            aiResponse.Summary,
            companies,
            strengths,
            weaknesses,
            usage);
    }


    private static AiUsage CreateAiUsage(ClientResult<ResponseResult> response)
    {
        var inputTokens = response.Value.Usage.InputTokenCount;
        var outputTokens = response.Value.Usage.OutputTokenCount;
        var totalTokens = response.Value.Usage.TotalTokenCount;

        var estimatedCost = CalculateEstimatedCost(
            inputTokens,
            outputTokens);

        return new AiUsage(
            InputTokens: inputTokens,
            OutputTokens: outputTokens,
            TotalTokens: totalTokens,
            EstimatedCost: estimatedCost
        );
    }

    private static IReadOnlyList<ResumeCompanyResult> ValidateAndMapCompanies(
    IReadOnlyList<AiCompanyResult> companyResults,
    IReadOnlyList<CompanyBulletContext> contexts)
    {
        var results = new List<ResumeCompanyResult>();

        var alternativeCount = 0;

        foreach (var context in contexts)
        {
            var companyResult = companyResults.FirstOrDefault(
                result => string.Equals(result.Company, context.Name, StringComparison.OrdinalIgnoreCase));

            var bullets = new List<ResumeBulletResult>();

            if (companyResult is not null)
            {
                var validBullets = companyResult.Bullets
                    .Where(result => context.Bullets.Contains(result.Value))
                    .Take(context.MaxBullets);

                foreach (var bullet in validBullets)
                {
                    string? alternative = null;

                    if (alternativeCount < 3 &&
                        !string.IsNullOrWhiteSpace(bullet.Alternative))
                    {
                        alternative = bullet.Alternative;
                        alternativeCount++;
                    }

                    bullets.Add(
                        new ResumeBulletResult(
                            bullet.Value,
                            alternative));
                }
            }

            results.Add(
                new ResumeCompanyResult(
                    context.CompanyId,
                    context.Name,
                    context.Title,
                    context.Location,
                    context.Started,
                    context.Ended,
                    bullets));
        }

        return results;
    }

    private static string BuildPrompt(ResumeAiGenerationContext context)
    {
        var contextJson = JsonSerializer.Serialize(context, new JsonSerializerOptions { WriteIndented = true });

        return $$"""
            Analyze the candidate's experience against the provided job description
            and generate resume recommendations.

            JOB DESCRIPTION:
            {{context.JobDescription}}

            RESUME EXPERIENCE:
            {{contextJson}}

            BULLET SELECTION:

            For each company:
            - Select no more than MaxBullets.
            - Only select bullets from that company's Bullets collection.
            - Do not modify the selected bullet's Value.
            - Rank selected bullets from strongest to weakest match.
            - Never move bullets between companies.

            BULLET ALTERNATIVES:

            - Most selected bullets should have Alternative set to null.
            - Provide alternatives for at most 3 selected bullets total.
            - Only provide an alternative when rewriting the bullet would
              materially improve its relevance, clarity, or impact.
            - An alternative may improve wording but must not invent experience,
              technologies, responsibilities, metrics, or accomplishments.
            - Preserve the factual meaning of the original bullet.

            SUMMARY:

            - Summarize the job description itself, not the candidate.
            - Identify the role's primary responsibilities, technical focus, and recurring themes.
            - Give extra weight to requirements or responsibilities that are mentioned repeatedly or emphasized in multiple parts of the job description.
            - Highlight the technologies, engineering practices, and soft skills that appear most important to the employer.
            - If a concept appears multiple times, such as testing, scalability, collaboration, ownership, security, or cloud development, reflect that importance in the summary.
            - Do not describe the candidate's background, accomplishments, years of experience, or qualifications.
            - Do not compare the candidate to the job.
            - Do not include resume metrics or examples from the candidate's experience.
            - Base the summary entirely on the job description.
            - Keep the summary between 100 and 150 words.

            STRENGTHS:

            - Return 3 to 5 of the candidate's strongest matches for the role.
            - Keep each strength concise.
            - Base every strength on evidence in the supplied experience.
            - Rank strengths from strongest to weakest.

            WEAKNESSES:

            - Return 3 to 5 meaningful gaps or weaker areas relative to the job.
            - Keep each weakness concise.
            - Do not assume missing experience exists.
            - Do not exaggerate gaps when closely related experience is present.
            - Rank weaknesses from most significant to least significant.

            RESUME MATCH SCORE:

            - Assign a score from 0 to 100 representing how strongly the supplied resume experience matches the job description.
            - Score only demonstrated experience. Do not assume skills or experience that are not supplied.
            - Give the most weight to requirements, responsibilities, technologies, and engineering practices that are emphasized or repeated in the job description.
            - Consider both the importance of a requirement and the strength of the candidate's evidence for it.
            - Closely related experience may receive partial credit when it demonstrates transferable knowledge.
            - Do not heavily penalize minor or optional requirements.
            - - The score should be consistent with the identified strengths and weaknesses.
            """;
    }

    private sealed record AiResumeResponse(
        int Score,
        string Summary,
        IReadOnlyList<AiCompanyResult> Companies,
        IReadOnlyList<string> Strengths,
        IReadOnlyList<string> Weaknesses);

    private sealed record AiCompanyResult(int companyId, string Company, IReadOnlyList<AiBulletResult> Bullets);

    private sealed record AiBulletResult(string Value, string? Alternative);

    private static decimal CalculateEstimatedCost(
        int inputTokens,
        int outputTokens)
    {
        var inputCost =
            inputTokens / 1_000_000m * InputCostPerMillionTokens;

        var outputCost =
            outputTokens / 1_000_000m * OutputCostPerMillionTokens;

        return inputCost + outputCost;
    }
}



#pragma warning restore OPENAI001

