namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeAiMetaDataRequest(int GeneratedResumeId, int InputTokens, int OutputTokens, int TotalTokens);
