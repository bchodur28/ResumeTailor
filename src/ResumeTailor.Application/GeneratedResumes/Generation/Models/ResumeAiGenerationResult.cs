using ResumeTailor.Application.GeneratedResumes.Common.Models;

namespace ResumeTailor.Application.GeneratedResumes.Generation.Models;

public sealed record ResumeAiGenerationResult(
    string Summary,
    IReadOnlyList<ResumeCompanyResult> Companies,
    IReadOnlyList<ResumeAiInsightResult> Strengths,
    IReadOnlyList<ResumeAiInsightResult> Weaknesses,
    AiUsage AiUsage);
