using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.Resumes.Common.Models;

namespace ResumeTailor.Application.GeneratedResumes.Generation.Models;

public sealed record ResumeAiGenerationResult(
    int Score,
    string Summary,
    IReadOnlyList<ResumeCompanyResult> Companies,
    IReadOnlyList<string> Strengths,
    IReadOnlyList<string> Weaknesses,
    AiMetaDataResult AiMetaData,
    JobPostingResult JobPosting
    );
