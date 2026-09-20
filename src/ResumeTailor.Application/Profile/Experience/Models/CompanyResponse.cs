

namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record CompanyResponse(
    int Id,
    string Name,
    string Title,
    string Location,
    DateOnly Started,
    DateOnly? Ended,
    bool GenerateBullets,
    int MaxGeneratedBulletCount,
    int BulletCount);
