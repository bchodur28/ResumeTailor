namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record AiJobMatchAnalysisOutput(string Summary, int AiScore, IReadOnlyCollection<string> Strengths, IReadOnlyCollection<string> Weaknesses);
