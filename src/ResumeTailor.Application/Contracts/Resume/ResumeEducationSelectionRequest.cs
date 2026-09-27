namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeEducationSelectionRequest(int? Id, int EducationId, int SortOrder);
