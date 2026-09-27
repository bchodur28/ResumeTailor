namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeAiMetaDataRequest(int ResumeId, string Model, int InputTokens, int OutputTokens, int TotalTokens, decimal Cost);
