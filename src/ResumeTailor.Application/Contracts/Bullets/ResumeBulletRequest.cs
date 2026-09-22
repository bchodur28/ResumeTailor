namespace ResumeTailor.Application.Contracts.Bullets;

public sealed record ResumeBulletRequest(int? SourceBulletId, string Value, string? AlternativeValue, int SortOrder);
