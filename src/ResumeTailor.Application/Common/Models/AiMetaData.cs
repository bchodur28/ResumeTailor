namespace ResumeTailor.Application.Common.Models;

public record AiMetaData(string Model, int InputTokens, int OutputTokens, int TotalTokens, decimal Cost);
