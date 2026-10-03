namespace ResumeTailor.Application.Contracts.Bullets;

public sealed record BulletReponse(int Id, int CompanyId, string Value, int? AiScore, int ResumeCount);
