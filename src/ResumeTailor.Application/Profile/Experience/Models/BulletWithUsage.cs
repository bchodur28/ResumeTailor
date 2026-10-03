namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record BulletWithUsage(int Id, int CompanyId, string Value, int? AiScore, bool IsDeleted, int ResumeCount);
