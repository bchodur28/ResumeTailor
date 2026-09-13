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
            aiResponse.Summary,
            companies,
            strengths,
            weaknesses,
            usage);
    }
    

    private static AiUsage CreateAiUsage(ClientResult<ResponseResult> response)
    {
        return new AiUsage(
            InputTokens: response.Value.Usage.InputTokenCount,
            OutputTokens: response.Value.Usage.OutputTokenCount,
            TotalTokens: response.Value.Usage.TotalTokenCount
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
                result => string.Equals(
                    result.Company,
                    context.Name,
                    StringComparison.OrdinalIgnoreCase));

            var bullets = new List<ResumeBulletResult>();

            if (companyResult is not null)
            {
                var validBullets = companyResult.Bullets
                    .Where(result =>
                        context.Bullets.Contains(result.Value))
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

            - Write a professional resume summary tailored to the job description.
            - Keep the summary between 150 and 200 words.
            - Base it entirely on the supplied experience.
            - Do not invent skills or experience.
            - Emphasize the candidate's strongest qualifications for this role.

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
            """;
    }

    private sealed record AiResumeResponse(
        string Summary,
        IReadOnlyList<AiCompanyResult> Companies,
        IReadOnlyList<string> Strengths,
        IReadOnlyList<string> Weaknesses);

    private sealed record AiCompanyResult(string Company, IReadOnlyList<AiBulletResult> Bullets);

    private sealed record AiBulletResult(string Value, string? Alternative);
}

#pragma warning restore OPENAI001

