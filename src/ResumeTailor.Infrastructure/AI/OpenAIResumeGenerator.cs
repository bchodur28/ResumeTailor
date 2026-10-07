#pragma warning disable OPENAI001

using Microsoft.Extensions.Options;
using OpenAI.Responses;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
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
    private const string AiMappingContext = """
    {
        "type": "object",
        "properties": {
            "AiScore": {
                "type": "integer",
                "minimum": 0,
                "maximum": 100
            },
            "CompanyBullets": {
                "type": "array",
                "items": {
                    "type": "object",
                    "properties": {
                        "CompanyId": { "type": "integer" },
                        "BulletId": { "type": "integer" },
                        "AlternativeValue": { "type": ["string", "null"] }
                    },
                    "required": [
                        "CompanyId",
                        "BulletId",
                        "AlternativeValue"
                    ],
                    "additionalProperties": false
                }
            },
            "CompanyAnalysis": {
                "type": "object",
                "properties": {
                    "summary": { "type": "string" },
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
                    "strengths",
                    "weaknesses"
                ],
                "additionalProperties": false
            },
            "jobPosting": {
                "type": "object",
                "properties": {
                    "companyName": { "type": ["string", "null"] },
                    "jobTitle": { "type": ["string", "null"] },
                    "location": { "type": ["string", "null"] },
                    "workStyle": {
                        "type": ["string", "null"],
                        "enum": ["OnSite", "Hybrid", "Remote", null]
                    },
                    "salaryMin": { "type": ["number", "null"] },
                    "salaryMax": { "type": ["number", "null"] },
                    "salary": { "type": ["number", "null"] },
                    "salaryPeriod": {
                        "type": ["string", "null"],
                        "enum": [
                            "Hourly",
                            "Daily",
                            "Weekly",
                            "Monthly",
                            "Yearly",
                            null
                        ]
                    },
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
            "AiScore",
            "CompanyBullets",
            "CompanyAnalysis",
            "jobPosting"
        ],
        "additionalProperties": false
    }
    """;

    public async Task<ResumeAiGenerationResult> GenerateAsync(ResumeAiContext context, CancellationToken cancellationToken = default)
    {
        var prompt = BuildPrompt(context);

        var responseOptions = new CreateResponseOptions
        {
            Model = options.Value.Model,
            TextOptions = new ResponseTextOptions
            {
                TextFormat = ResponseTextFormat.CreateJsonSchemaFormat(
                    jsonSchemaFormatName: "resume_analysis",
                    jsonSchema: BinaryData.FromString(AiMappingContext))
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

        var companyBulletResults = ValidateAndMapCompanyBulletResults(aiResponse.CompanyBullets, context.Companies);

        var metaData = CreateAiMetaData(response);
        var jobPosting = CreateJobPosting(aiResponse.JobPosting);
        var aiAnalysis = CreateCompanyAiAnalysis(aiResponse.CompanyAnalysis);

        return new ResumeAiGenerationResult(
            AiScore: aiResponse.AiScore,
            CompanyBullets: companyBulletResults,
            AiAnalysis: aiAnalysis,
            JobPosting: jobPosting,
            MetaData: metaData
            );
    }

    private static CompanyAiAnalysisResult CreateCompanyAiAnalysis(AiCompanyAnalysisResponse companyAnalysisResponse)
    {
        return new CompanyAiAnalysisResult(
            Summary: companyAnalysisResponse.Summary,
            Strengths: companyAnalysisResponse.Strengths,
            Weaknesses: companyAnalysisResponse.Weaknesses
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

    private static IReadOnlyList<CompanyBulletAiResult> ValidateAndMapCompanyBulletResults(
    IReadOnlyCollection<AiCompanyBulletResponse> responses,
    IReadOnlyCollection<CompanyAiContext> companies)
    {
        var companiesById = companies.ToDictionary(c => c.CompanyId);
        var validResults = new List<CompanyBulletAiResult>();

        foreach (var response in responses)
        {
            if(!companiesById.TryGetValue(response.CompanyId, out var companyContext))
            {
                continue;
            }

            var bulletExists = companyContext.Bullets.Any(b => b.BulletId == response.BulletId);

            if(!bulletExists)
            {
                continue;
            }

            validResults.Add(new CompanyBulletAiResult(
                CompanyId: response.CompanyId,
                BulletId: response.BulletId,
                AlternativeValue: response.AlternativeValue
            ));
        }

        return validResults
            .DistinctBy(x => (x.CompanyId, x.BulletId))
            .GroupBy(x => x.CompanyId)
            .OrderByDescending(group => companiesById[group.Key].Ended is null)
            .ThenByDescending(group => companiesById[group.Key].Ended)
            .ThenByDescending(group => companiesById[group.Key].Started)
            .SelectMany(group =>
            {
                var maxBullets = companiesById[group.Key].MaxBullets;
                return group.Take(maxBullets);
            })
            .ToList();
    }

    private static string BuildPrompt(ResumeAiContext context)
    {
        var companiesJson = JsonSerializer.Serialize(context.Companies, new JsonSerializerOptions { WriteIndented = true });

        return $$"""
            Analyze the candidate's experience against the provided job description
            and generate resume recommendations.

            JOB DESCRIPTION:
            {{context.JobDescription}}

            RESUME EXPERIENCE:
            {{companiesJson}}

            BULLET SELECTION:

            For each company:
            - Select the bullets that best match the job description.
            - Select no more than that company's MaxBullets.
            - A selected bullet must come from that company's supplied bullets.
            - Return the supplied CompanyId and BulletId exactly as provided.
            - Never invent, modify, or substitute a CompanyId or BulletId.
            - Never associate a BulletId with a different CompanyId.
            - Do not return bullet text in the selection.
            - Return selected bullets in strongest-to-weakest order for each company.
            - Do not select the same BulletId more than once.

            BULLET ALTERNATIVES:

            - AlternativeValue represents an optional rewritten version of the selected bullet.
            - Most selected bullets should have AlternativeValue set to null.
            - Provide an AlternativeValue for at most 3 selected bullets total.
            - Only provide an AlternativeValue when rewriting the original bullet would
              materially improve its relevance, clarity, or impact for this job.
            - The AlternativeValue must remain factually equivalent to the original bullet.
            - Do not invent technologies, responsibilities, metrics, accomplishments,
              scope, or experience.
            - Preserve all important factual claims from the original bullet.

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
        int AiScore,
        IReadOnlyCollection<AiCompanyBulletResponse> CompanyBullets,
        AiCompanyAnalysisResponse CompanyAnalysis,
        AiJobPostingResponse JobPosting
    );

    private sealed record AiCompanyBulletResponse(int CompanyId, int BulletId, string? AlternativeValue);

    private sealed record AiCompanyAnalysisResponse(string Summary, IReadOnlyCollection<string> Strengths, IReadOnlyCollection<string> Weaknesses);

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

