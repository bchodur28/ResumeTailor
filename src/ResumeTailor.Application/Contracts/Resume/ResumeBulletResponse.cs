namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeBulletResponse(int Id, int SourceBulletId, string Value, string? AlternativeValue, int? SortOrder, bool IsSourceDeleted);

