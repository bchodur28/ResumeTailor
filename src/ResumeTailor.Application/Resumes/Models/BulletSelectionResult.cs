namespace ResumeTailor.Application.Resumes.Models;

public sealed record BulletSelectionResult(Dictionary<string, IReadOnlyList<string>> CompanyBullets, AiUsage Usage);
