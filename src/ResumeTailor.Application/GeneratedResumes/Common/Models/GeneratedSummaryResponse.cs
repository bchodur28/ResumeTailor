namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record GeneratedSummaryResponse(
    string AiSummary,
    IReadOnlyList<ResumeAiInsightResult> Strengths,
    IReadOnlyList<ResumeAiInsightResult> Weaknesses
    );
