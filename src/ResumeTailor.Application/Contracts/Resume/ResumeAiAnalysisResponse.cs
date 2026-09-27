namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeAiAnalysisResponse(
    int? Score,
    string Summary,
    IReadOnlyList<string> Strengths,
    IReadOnlyList<string> Weaknesses
    );
