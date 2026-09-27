

using ResumeTailor.Application.Resumes.Generation.Models;

namespace ResumeTailor.Application.GeneratedResumes.Generation.Models;

public sealed record CompanyBulletContext(
    int CompanyId,
    string Name,
    string Title,
    string? Location,
    DateOnly Started,
    DateOnly? Ended,
    IReadOnlyCollection<BulletContext> Bullets,
    int MaxBullets);
