using ResumeTailor.Application.GeneratedResumes.Common.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeSummaryResponse(
    int AiScore,
    string AiSummary,
    IReadOnlyList<ResumeAiInsightResult> Strengths,
    IReadOnlyList<ResumeAiInsightResult> Weaknesses
    );
