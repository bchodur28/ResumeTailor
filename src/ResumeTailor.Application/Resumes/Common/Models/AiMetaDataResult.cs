namespace ResumeTailor.Application.Resumes.Common.Models;

public sealed record AiMetaDataResult(string Model, int InputTokens, int OutputTokens, int TotalTokens, decimal Cost);
