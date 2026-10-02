namespace ResumeTailor.Application.Resumes.Common.Models;

public sealed record ResumeBulletResult(int? Id, int? SourceBulletId, string Value, string? AlternativeValue, int? SortOrder, bool IsSourceDeleted);

