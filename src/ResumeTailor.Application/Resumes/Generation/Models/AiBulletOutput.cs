namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record AiBulletOutput(int BulletId, string? Alternative, string Reasoning);
