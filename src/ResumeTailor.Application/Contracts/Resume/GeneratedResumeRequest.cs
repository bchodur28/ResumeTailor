namespace ResumeTailor.Application.Contracts.Resume;

public sealed record GeneratedResumeRequest(int AccountId, int? JobApplicatonId, string Name);
