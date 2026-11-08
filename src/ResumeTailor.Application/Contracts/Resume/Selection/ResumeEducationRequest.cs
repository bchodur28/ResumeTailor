namespace ResumeTailor.Application.Contracts.Resume.Selection;

public sealed record ResumeEducationRequest(int? Id, int LibraryEducationId, int SortOrder);
