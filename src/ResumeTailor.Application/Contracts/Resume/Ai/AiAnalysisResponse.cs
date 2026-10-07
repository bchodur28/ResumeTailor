namespace ResumeTailor.Application.Contracts.Resume;

public sealed record AiAnalysisResult(
    string Summary,
    IReadOnlyList<string> Strengths,
    IReadOnlyList<string> Weaknesses
    );
