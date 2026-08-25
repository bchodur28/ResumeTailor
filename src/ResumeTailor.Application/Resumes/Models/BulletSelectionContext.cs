namespace ResumeTailor.Application.Resumes.Models;

public sealed record BulletSelectionContext(IReadOnlyList<string> Bullets, string Company, int MaxBullets, string? AdditionalInstruction);
