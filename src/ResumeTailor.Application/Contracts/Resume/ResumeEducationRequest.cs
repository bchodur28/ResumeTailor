namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeEducationRequest(int? Id, int LibraryEducationId, int SortOrder);
