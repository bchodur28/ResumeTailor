namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record BulletRequest(int? Id, int CompanyId, string Value, int? AiScore);

