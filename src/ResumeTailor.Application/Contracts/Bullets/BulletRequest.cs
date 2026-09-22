namespace ResumeTailor.Application.Contracts.Bullets;

public sealed record BulletRequest(int? Id, int CompanyId, string Value, int? AiScore);

