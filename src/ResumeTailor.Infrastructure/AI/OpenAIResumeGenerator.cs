#pragma warning disable OPENAI001

using Microsoft.Extensions.Options;
using OpenAI.Responses;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.Resumes.JobApplications;
using ResumeTailor.Domain.Resumes.JobPositing;
using System.ClientModel;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                        },
                        "jobPosting": {
                          "type": "object",
                          "properties": {
                            "companyName": { "type": ["string", "null"] },
                            "jobTitle": { "type": ["string", "null"] },
                            "location": { "type": ["string", "null"] },
                            "workStyle": {"type": ["string", "null"],"enum": ["OnSite", "Hybrid", "Remote", null]},
                            "salaryMin": { "type": ["number", "null"] },
                            "salaryMax": { "type": ["number", "null"] },
                            "salary": { "type": ["number", "null"] },
                            "salaryPeriod": {"type": ["string", "null"],"enum": ["Hourly", "Weekly", "Monthly", "Yearly", null]},
                            "salaryCurrency": { "type": ["string", "null"] }
                          },
                          "required": [
                            "companyName",
                            "jobTitle",
                            "location",
                            "workStyle",
                            "salaryMin",
                            "salaryMax",
                            "salary",
                            "salaryPeriod",
                            "salaryCurrency"
                          ],
                          "additionalProperties": false
                        }
                      },
                      "required": [
                        "score",
                        "summary",
                        "companies",
                        "strengths",
                        "weaknesses",
                        "jobPosting"
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
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            }) ?? throw new InvalidOperationException("OpenAI returned an empty resume analysis response.");

        var companies = ValidateAndMapCompanies(aiResponse.Companies, context.CompanyBulletContexts);

        var strengths = aiResponse.Strengths
            .Take(5)
            .ToList();

        var weaknesses = aiResponse.Weaknesses
            .Take(5)
            .ToList();

        var metaData = CreateAiMetaData(response);

        var jobPosting = CreateJobPosting(aiResponse.JobPosting);

        return new ResumeAiGenerationResult(
            aiResponse.Score,
            aiResponse.Summary,
            companies,
            strengths,
            weaknesses,
            metaData,
            jobPosting
            );
    }

    private static JobPostingResult CreateJobPosting(AiJobPostingResponse jobPostingResponse)
    {
        return new JobPostingResult(
            CompanyName: jobPostingResponse.CompanyName,
            JobTitle: jobPostingResponse.JobTitle,
            Location: jobPostingResponse.Location,
            WorkStyle: jobPostingResponse.WorkStyle,
            SalaryMin: jobPostingResponse.SalaryMin,
            SalaryMax: jobPostingResponse.SalaryMax,
            Salary: jobPostingResponse.Salary,
            SalaryPeriod: jobPostingResponse.SalaryPeriod,
            SalaryCurrency: jobPostingResponse.SalaryCurrency
        );
    }


    private static AiMetaDataResult CreateAiMetaData(ClientResult<ResponseResult> response)
    {
        var inputTokens = response.Value.Usage.InputTokenCount;
        var outputTokens = response.Value.Usage.OutputTokenCount;
        var totalTokens = response.Value.Usage.TotalTokenCount;

        var estimatedCost = CalculateEstimatedCost(inputTokens, outputTokens);

        return new AiMetaDataResult(
            Model: response.Value.Model,
            InputTokens: inputTokens,
            OutputTokens: outputTokens,
            TotalTokens: totalTokens,
            Cost: estimatedCost
        );
    }

    private static IReadOnlyList<ResumeCompanyResult> ValidateAndMapCompanies(
    IReadOnlyList<AiCompanyResponse> companyResponse,
    IReadOnlyList<CompanyBulletContext> contexts)
    {
        var results = new List<ResumeCompanyResult>();

        var alternativeCount = 0;

        foreach (var context in contexts)
        {
            var companyResult = companyResponse.FirstOrDefault(
                result => string.Equals(result.Company, context.Name, StringComparison.OrdinalIgnoreCase));

            var bullets = new List<ResumeBulletResult>();

            if (companyResult is not null)
            {
                foreach (var bullet in companyResult.Bullets)
                {
                    var sourceBullet = context.Bullets.FirstOrDefault(b => b.Value == bullet.Value);

                    if(sourceBullet is null)
                    {
                        continue;
                    }

                    string? alternative = null;

                    if (alternativeCount < 3 && !string.IsNullOrWhiteSpace(bullet.Alternative))
                    {
                        alternative = bullet.Alternative;
                        alternativeCount++;
                    }

                    bullets.Add(new ResumeBulletResult(
                        SourceBulletId: sourceBullet.SourceBulletId,
                        Value: bullet.Value,
                        AlternativeValue: alternative));

                    if(bullets.Count >= context.MaxBullets)
                    {
                        break;
                    }
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
            - The score should be consistent with the identified strengths and weaknesses.

            JOB POSTING EXTRACTION:

            Extract structured information about the job from the JOB DESCRIPTION.

            - CompanyName:
                - Extract the employer/company name when explicitly stated.
                - Return null if the company cannot be reliably determined.
                - Do not infer the company from the candidate's resume experience.

            - JobTitle:
                - Extract the title of the position being advertised.
                - Return null if no job title can be reliably determined.

            - Location:
                - Extract the office or job location when explicitly stated.
                - Prefer a concise location such as "Minneapolis, MN".
                - For remote positions, still return the associated geographic or office
                location if one is provided.
                - Return null if no location is provided.

            - WorkStyle:
                - Return "Remote" when the position is explicitly remote.
                - Return "Hybrid" when the position requires a combination of remote and in-office work.
                - Return "OnSite" when the position is expected to be performed in-office.
                - Return null when the work arrangement cannot be reliably determined.

            - SalaryMin and SalaryMax:
                - For a salary range, return the lower value as SalaryMin and the upper value as SalaryMax.
                - Return null for both when no salary range is provided.
                - Preserve the compensation period from the posting; do not convert hourly compensation to annual compensation or vice versa.

            - Salary:
                - Use Salary only when the posting provides a single compensation amount rather than a range.
                - When a range is provided, return null for Salary.
                - Do not estimate or convert compensation.

            - SalaryCurrency:
                - Return the currency when it can be determined from the posting.
                - Use standard currency codes such as "USD".
                - Return null if the currency cannot be reliably determined.

            - SalaryPeriod:
                - Return "Hourly" for compensation stated per hour.
                - Return "Daily" for compensation stated per day.
                - Return "Weekly" for compensation stated per week.
                - Return "Monthly" for compensation stated per month.
                - Return "Yearly" for annual or yearly compensation.
                - Return null if the compensation period cannot be reliably determined.
                - Do not infer or convert the compensation period.

            GENERAL EXTRACTION RULES:

            - Extract information only from the JOB DESCRIPTION.
            - Do not use RESUME EXPERIENCE to populate job posting information.
            - Do not guess or fabricate missing values.
            - When information cannot be reliably determined, return null.
            """;
    }

    private sealed record AiResumeResponse(
        int Score,
        string Summary,
        IReadOnlyList<AiCompanyResponse> Companies,
        IReadOnlyList<string> Strengths,
        IReadOnlyList<string> Weaknesses,
        AiJobPostingResponse JobPosting
);

    private sealed record AiCompanyResponse(int companyId, string Company, IReadOnlyList<AiBulletResponse> Bullets);

    private sealed record AiBulletResponse(string Value, string? Alternative);

    private sealed record AiJobPostingResponse(
        string? CompanyName,
        string? JobTitle,
        string? Location,
        WorkStyle? WorkStyle,
        decimal? SalaryMin,
        decimal? SalaryMax,
        decimal? Salary,
        SalaryPeriod? SalaryPeriod,
        string? SalaryCurrency);

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

