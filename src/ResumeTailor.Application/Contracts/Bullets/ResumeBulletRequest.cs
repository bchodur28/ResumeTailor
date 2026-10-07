namespace ResumeTailor.Application.Contracts.Bullets;

public sealed record ResumeBulletRequest(int? Id, int SourceBulletId, string Value, string? AlternativeValue, int SortOrder);
