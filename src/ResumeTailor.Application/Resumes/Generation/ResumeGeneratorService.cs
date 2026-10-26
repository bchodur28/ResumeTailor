using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Library.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Resumes.Content;
using System.Text.Json;


namespace ResumeTailor.Application.Resumes.Generation;

public class ResumeGeneratorService(
    IAiGenerator<AiResumeDataOutput> aiGenerator,
    IResumeCreator resumeCreator,
    ILibraryExperienceRepository libraryExperienceRepository) : IResumeGenerator
{


    private const string AiResumeDataOutputSchema = """
    {
        "type": "object",
        "properties": {
            "Experiences": {
                "type": "array",
                "items": {
                    "type": "object",
                    "properties": {
                        "ExperienceId": {
                            "type": "integer"
                        },
                        "ChosenSummary": {
                            "anyOf": [
                                {
                                    "type": "object",
                                    "properties": {
                                        "SummaryId": { "type": "integer" },
                                        "Reasoning": { "type": "string" }
                                    },
                                    "required": ["SummaryId", "Reasoning"],
                                    "additionalProperties": false
                                },
                                {
                                    "type": "null"
                                }
                            ]
                        },
                        "ChosenBullets": {
                            "type": "array",
                            "items": {
                                "type": "object",
                                "properties": {
                                    "BulletId": {
                                        "type": "integer"
                                    },
                                    "Alternative": {
                                        "type": ["string", "null"]
                                    },
                                    "Reasoning": {
                                        "type": "string"
                                    }
                                },
                                "required": [
                                    "BulletId",
                                    "Alternative",
                                    "Reasoning"
                                ],
                                "additionalProperties": false
                            }
                        }
                    },
                    "required": [
                        "ExperienceId",
                        "ChosenSummary",
                        "ChosenBullets"
                    ],
                    "additionalProperties": false
                }
            },
            "JobMatchAnalysis": {
                "type": "object",
                "properties": {
                    "Summary": {
                        "type": "string"
                    },
                    "AiScore": {
                        "type": "integer",
                        "minimum": 0,
                        "maximum": 100
                    },
                    "Strengths": {
                        "type": "array",
                        "items": {
                            "type": "string"
                        },
                        "maxItems": 5
                    },
                    "Weaknesses": {
                        "type": "array",
                        "items": {
                            "type": "string"
                        },
                        "maxItems": 5
                    }
                },
                "required": [
                    "Summary",
                    "AiScore",
                    "Strengths",
                    "Weaknesses"
                ],
                "additionalProperties": false
            },
            "JobPostingData": {
                "type": "object",
                "properties": {
                    "CompanyName": {
                        "type": ["string", "null"]
                    },
                    "JobTitle": {
                        "type": ["string", "null"]
                    },
                    "Location": {
                        "type": ["string", "null"]
                    },
                    "WorkStyle": {
                        "type": ["string", "null"],
                        "enum": ["OnSite", "Hybrid", "Remote", null]
                    },
                    "SalaryMin": {
                        "type": ["number", "null"]
                    },
                    "SalaryMax": {
                        "type": ["number", "null"]
                    },
                    "Salary": {
                        "type": ["number", "null"]
                    },
                    "SalaryPeriod": {
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
                    "SalaryCurrency": {
                        "type": ["string", "null"]
                    }
                },
                "required": [
                    "CompanyName",
                    "JobTitle",
                    "Location",
                    "WorkStyle",
                    "SalaryMin",
                    "SalaryMax",
                    "Salary",
                    "SalaryPeriod",
                    "SalaryCurrency"
                ],
                "additionalProperties": false
            }
        },
        "required": [
            "Experiences",
            "JobMatchAnalysis",
            "JobPostingData"
        ],
        "additionalProperties": false
    }
    """;
    private const string AiResumeSchemaFormatName = "resume_analysis";


    public async Task<int> GenerateResumeAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default)
    {
        var aiExperienceInput = await libraryExperienceRepository.GetAiInputDataForGeneration(accountId, cancellationToken);
        var aiResumeInput = new ResumeAiInput(jobDescription, aiExperienceInput);

        var aiContext = new AiContext(BuildPrompt(aiResumeInput), AiResumeSchemaFormatName, AiResumeDataOutputSchema);
        var aiResult = await aiGenerator.GenerateAsync(aiContext, cancellationToken);

        // Combine AI selections with Library selection
        var resolvedExperience = ResolveExperiences(aiExperienceInput, aiResult.Value.Experiences);


        var resumeId = await resumeCreator.CreateResumeAsync(accountId, aiResult, resolvedExperience,  cancellationToken);

        return resumeId;
    }

    private static IReadOnlyCollection<ResolvedResumeExperience> ResolveExperiences(
        IReadOnlyCollection<LibraryExperienceAiInput> originalExperiences,
        IReadOnlyCollection<AiExperienceOutput> aiChosenExperiences)
    {
        var aiLookup = aiChosenExperiences.ToDictionary(ex => ex.ExperienceId);

        return originalExperiences.Select(original => {
            aiLookup.TryGetValue(original.ExperienceId, out var ai);

            int? summaryId;
            string? summaryReasoning;

            if (original.LetAiChooseSummary)
            {
                summaryId = ai?.ChosenSummary?.SummaryId;
                summaryReasoning = ai?.ChosenSummary?.Reasoning;
            }
            else
            {
                summaryId = original.Summaries.Keys.FirstOrDefault();
                if (summaryId == 0)
                {
                    summaryId = null;
                }

                summaryReasoning = null;
            }

            IReadOnlyCollection<ResolvedResumeBullet> bullets;

            if (original.LetAiChooseBullets)
            {
                bullets = ai?.ChosenBullets
                    .Select(b => new ResolvedResumeBullet(b.BulletId, b.Alternative, b.Reasoning))
                    .ToList() ?? [];
            }
            else
            {
                bullets = original.Bullets.Keys
                    .Select(id => new ResolvedResumeBullet(id, null, null))
                    .ToList();
            }

            return new ResolvedResumeExperience(original.ExperienceId, summaryId, summaryReasoning, bullets);
        }).ToList();
    }

    private static string BuildPrompt(ResumeAiInput context)
    {
        var experienceJson = JsonSerializer.Serialize(
            context.Experiences,
            new JsonSerializerOptions
            {
                WriteIndented = true
            });

        return $$"""
        Analyze the candidate's experience against the provided job
        description and generate tailored resume recommendations,
        an organization analysis, and structured job posting data.

        JOB DESCRIPTION:
        {{context.JobDescription}}

        CANDIDATE EXPERIENCES:
        {{experienceJson}}
        
        EXPERIENCE SELECTION:

        - Evaluate every provided experience against the job description.
        - Use all supplied summaries and bullets as supporting evidence
          for JobMatchAnalysis, regardless of selection permissions.
        - Return exactly one Experiences entry per provided experience.
        - Preserve the supplied ExperienceId exactly.
        - When selection is disabled for a particular component,
          return null for ChosenSummary or an empty ChosenBullets array
          as appropriate.
        - Never invent, modify, or substitute experience,
          summary, or bullet identifiers.
        - Never associate a summary or bullet with an experience
          it does not belong to.
        - Preserve the order of the provided experiences.
        

        
        
        SUMMARY SELECTION:

        For each experience:

        - If LetAiChooseSummary is false:
          - Do not select a summary.
          - Return null for ChosenSummary.
          - Use the supplied primary summary only as context
            for evaluating the candidate's qualifications.
          - Do not generate selection reasoning.

        - If LetAiChooseSummary is true:
          - If Summaries is empty, return null for ChosenSummary.
          - Otherwise, select exactly one summary from that
            experience's available summaries.
          - Select the summary that best communicates the
            experience's relevance to the job description.
          - Return the supplied SummaryId exactly as provided.
          - Never invent, modify, or substitute a SummaryId.
          - Never select a summary belonging to another experience.
          - Do not return the original summary text.
          - When multiple summaries are available, prioritize
            relevance to the job's responsibilities and requirements.
          - Provide concise reasoning explaining why the summary
            was selected.
          - Base the reasoning on actual information from the
            selected summary and the job description.
          - Do not invent or exaggerate candidate qualifications.
        
        

        
        BULLET SELECTION:

        For each experience:

        - If LetAiChooseBullets is false:
          - Do not select any bullets.
          - Return an empty ChosenBullets array.
          - Use the supplied bullets only as evidence when
            evaluating the candidate's qualifications.
          - Do not generate bullet selection reasoning
            or alternative bullet rewrites.

        - If LetAiChooseBullets is true:
          - If Bullets is empty, return an empty ChosenBullets array.
          - Otherwise, select the bullets that best match the
            job description.
          - When MaxBullets is not null, select no more than
            the specified number of bullets.
          - When MaxBullets is null, there is no explicit
            selection limit. Select only relevant bullets.
          - A selected bullet must come from that experience's
            supplied bullets.
          - Return the supplied BulletId exactly as provided.
          - Never invent, modify, or substitute a BulletId.
          - Never associate a BulletId with a different ExperienceId.
          - Do not return the original bullet text in the selection.
          - Return selected bullets in strongest-to-weakest order
            for each experience.
          - Do not select the same BulletId more than once.
          - Prefer distinct evidence of relevant skills,
            technologies, achievements, and responsibilities.
          - Avoid selecting redundant bullets when more relevant
            and distinct bullets are available.
          - Give greater weight to requirements and responsibilities
            that are emphasized or repeated in the job description.
          - Consider transferable experience when it demonstrates
            closely related technical knowledge or skills.
          - Alternative represents an optional rewritten version
            of a selected bullet.
          - Most selected bullets should have Alternative set to null.
          - Provide an Alternative for at most 3 selected bullets
            total across all experiences.
          - Only provide an Alternative when rewriting the original
            bullet would materially improve its relevance,
            clarity, or impact for this job.
          - The Alternative must remain factually equivalent
            to the original bullet.
          - Do not invent technologies, responsibilities, metrics,
            accomplishments, scope, or experience.
          - Preserve all important factual claims from the
            original bullet.
          - Prefer improvements in wording, clarity, emphasis,
            or organization rather than introducing new claims.
          - Do not exaggerate the candidate's contributions.
          - Do not rewrite a bullet solely to insert keywords
            from the job description.
          - Return null when a rewrite is not warranted.
        
        

        BULLET REASONING:

        For each selected bullet:

        - Explain why the bullet was selected.
        - Identify the specific connection between the bullet
          and the job description.
        - Highlight relevant technologies, technical practices,
          responsibilities, skills, or accomplishments.
        - Keep the reasoning concise and specific.
        - Do not exaggerate the candidate's experience.
        - Do not claim the candidate has skills or experience
          that are not supported by the original bullet.
        - Avoid generic reasoning such as
          "This bullet is relevant to the position."

        BULLET ALTERNATIVES:

        - Alternative represents an optional rewritten version
          of the selected bullet.
        - Most selected bullets should have Alternative set to null.
        - Provide an Alternative for at most 3 selected bullets
          across all experiences.
        - Only provide an Alternative when rewriting the original
          bullet would materially improve its relevance,
          clarity, or impact for this job.
        - The Alternative must remain factually equivalent
          to the original bullet.
        - Do not invent technologies, responsibilities, metrics,
          accomplishments, scope, or experience.
        - Preserve all important factual claims from the
          original bullet.
        - Return null when a rewrite is not warranted.

        JOB MATCH ANALYSIS:

        Generate a JobMatchAnalysis containing:
        - AiScore
        - Summary
        - Strengths
        - Weaknesses

        Evaluate the candidate's complete supplied experience
        against the job description.

        SUMMARY:

        - Summarize the job description itself, not the candidate.
        - Identify the role's primary responsibilities,
          technical focus, and recurring themes.
        - Give extra weight to requirements or responsibilities
          mentioned repeatedly or emphasized in multiple
          parts of the job description.
        - Highlight the technologies, engineering practices,
          and soft skills that appear most important
          to the employer.
        - If a concept appears multiple times, such as testing,
          scalability, collaboration, ownership, security,
          or cloud development, reflect that importance
          in the summary.
        - Do not describe the candidate's background,
          accomplishments, years of experience,
          or qualifications.
        - Do not compare the candidate to the job.
        - Do not include resume metrics or examples
          from the candidate's experience.
        - Base the summary entirely on the job description.
        - Keep the summary between 100 and 150 words.

                
        RESUME MATCH SCORE:

        - Assign an AiScore from 0 to 100 representing how strongly
          the candidate's supplied experience matches the job description.
        - Evaluate all provided experiences, summaries, and bullets,
          regardless of LetAiChooseSummary or LetAiChooseBullets.
        - Score only demonstrated experience. Do not assume skills
          or experience that are not supplied.
        - Give the most weight to requirements, responsibilities,
          technologies, and engineering practices that are emphasized
          or repeated in the job description.
        - Consider both the importance of a requirement and the
          strength of the candidate's evidence for it.
        - Closely related experience may receive partial credit
          when it demonstrates transferable knowledge.
        - Do not heavily penalize minor or optional requirements.
        - Consider the depth and relevance of demonstrated experience,
          not just the presence of matching keywords.
        - Do not inflate the score simply because the candidate has
          many experiences or bullets.
        - The score must be consistent with the identified
          strengths and weaknesses.
        - Use the following general scoring guidelines:
          - 90-100: Exceptional match with nearly all major requirements
            strongly demonstrated.
          - 75-89: Strong match with most important requirements met.
          - 60-74: Moderate match with several relevant qualifications
            and some meaningful gaps.
          - 40-59: Partial match with significant missing requirements.
          - 0-39: Weak match with limited demonstrated relevance.
        - Return AiScore as an integer.
        

        STRENGTHS:

        - Return 3 to 5 of the candidate's strongest matches
          for the role.
        - Keep each strength concise.
        - Base every strength on evidence in the supplied
          candidate experiences.
        - Compare demonstrated experience with the
          job's actual requirements.
        - Rank strengths from strongest to weakest.
        - Do not invent or exaggerate qualifications.

        WEAKNESSES:

        - Return 3 to 5 meaningful gaps or weaker areas
          relative to the job.
        - Keep each weakness concise.
        - Do not assume missing experience exists.
        - Do not exaggerate gaps when closely related
          experience is present.
        - Distinguish between missing evidence and a
          demonstrated lack of experience.
        - Rank weaknesses from most significant
          to least significant.
        - Do not invent weaknesses merely to fill the list.

        JOB POSTING EXTRACTION:

        Extract structured information about the job
        from the JOB DESCRIPTION only.

        COMPANY NAME:

        - Extract the employer or company name when
          explicitly stated.
        - Return null if it cannot be reliably determined.
        - Do not infer the company from candidate experiences.

        JOB TITLE:

        - Extract the title of the advertised position.
        - Return null if the title cannot be reliably determined.

        LOCATION:

        - Extract the office or job location when stated.
        - Prefer concise locations such as "Minneapolis, MN".
        - For remote positions, still return the associated
          geographic or office location when provided.
        - Return null when no location is available.

        WORK STYLE:

        - Return "Remote" when explicitly remote.
        - Return "Hybrid" when both remote and in-office
          work are required.
        - Return "OnSite" when work is performed on-site.
        - Return null when the arrangement cannot
          be reliably determined.

        SALARY MIN AND SALARY MAX:

        - When a salary range is provided, return the lower
          amount as SalaryMin and the upper as SalaryMax.
        - Return null for both when no range is provided.
        - Preserve the compensation period.
        - Do not convert hourly compensation to yearly
          compensation or vice versa.

        SALARY:

        - Use Salary only when a single compensation
          amount is provided rather than a range.
        - When a range is provided, return null for Salary.
        - Do not estimate or convert compensation.

        SALARY CURRENCY:

        - Return the currency when it can be determined.
        - Use standard currency codes such as "USD".
        - Return null when currency cannot be determined.

        SALARY PERIOD:

        - Return "Hourly" for hourly compensation.
        - Return "Daily" for daily compensation.
        - Return "Weekly" for weekly compensation.
        - Return "Monthly" for monthly compensation.
        - Return "Yearly" for annual compensation.
        - Return null when the period cannot
          be reliably determined.
        - Do not infer or convert the compensation period.

        GENERAL EXTRACTION RULES:

        - Extract JobPostingData only from the JOB DESCRIPTION.
        - Do not use candidate experiences to populate
          job posting information.
        - Do not guess or fabricate missing values.
        - Return null when information cannot
          be reliably determined.

        OUTPUT REQUIREMENTS:

        - Follow the provided JSON schema exactly.
        - Experiences must contain the selected summary
          and bullets for each experience.
        - OrganizationAnalysis must contain Summary,
          Strengths, and Weaknesses.
        - JobPostingData must contain the extracted job fields.
        - Return no additional fields.
        """;
    }

}
