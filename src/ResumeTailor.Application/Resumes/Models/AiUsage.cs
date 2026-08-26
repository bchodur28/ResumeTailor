namespace ResumeTailor.Application.Resumes.Models;

public sealed record AiUsage(int InputTokens, int OutputTokens, int TotalTokens, decimal? EstimatedCost);
