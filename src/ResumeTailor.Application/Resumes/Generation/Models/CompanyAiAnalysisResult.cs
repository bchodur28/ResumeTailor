namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record CompanyAiAnalysisResult(
    string Summary,
    IReadOnlyCollection<string> Strengths,
    IReadOnlyCollection<string> Weaknesses);
