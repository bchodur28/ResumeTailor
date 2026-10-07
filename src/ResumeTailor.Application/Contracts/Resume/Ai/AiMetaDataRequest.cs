namespace ResumeTailor.Application.Contracts.Resume.Ai;

public sealed record AiMetaDataRequest(int ResumeId, string Model, int InputTokens, int OutputTokens, int TotalTokens, decimal Cost);
