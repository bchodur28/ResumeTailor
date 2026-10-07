namespace ResumeTailor.Application.Contracts.Resume.Selection;

public sealed record EducationSelectionRequest(int? Id, int EducationId, int SortOrder);
