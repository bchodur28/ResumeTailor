

namespace ResumeTailor.Application.GeneratedResumes.Generation.Models;

public sealed record CompanyBulletContext(
    int CompanyId,
    string Name,
    string Title,
    string? Location,
    DateOnly Started,
    DateOnly? Ended,
    IReadOnlyList<string> Bullets,
    int MaxBullets);
