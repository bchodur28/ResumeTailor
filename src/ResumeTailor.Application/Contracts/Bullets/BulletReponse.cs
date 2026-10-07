namespace ResumeTailor.Application.Contracts.Bullets;

public sealed record BulletReponse(int Id, string Value, int? AiScore, int ResumeCount);
