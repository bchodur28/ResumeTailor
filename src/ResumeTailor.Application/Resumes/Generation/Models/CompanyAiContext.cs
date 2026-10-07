namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record CompanyAiContext(int CompanyId, int MaxBullets, DateOnly Started, DateOnly? Ended, IReadOnlyCollection<BulletAiContext> Bullets);
