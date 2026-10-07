using ResumeTailor.Application.Resumes.Common.Models;

namespace ResumeTailor.Application.Resumes.Generation.Models;

public record ResumeAiGenerationResult(
    int AiScore,
    IReadOnlyList<CompanyBulletAiResult> CompanyBullets,
    CompanyAiAnalysisResult AiAnalysis,
    JobPostingResult JobPosting,
    AiMetaDataResult MetaData);
