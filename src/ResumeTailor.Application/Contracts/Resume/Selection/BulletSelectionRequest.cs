namespace ResumeTailor.Application.Contracts.Resume.Selection;

public sealed record BulletSelectionRequest(int? Id, int BulletId, string Value, string? AlternativeValue, int SortOrder);
