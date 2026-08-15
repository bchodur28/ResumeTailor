namespace ResumeTailor.Application.Resumes.Models;

public sealed record CompanyResponse(
    int Id,
    string Name,
    string Position,
    string WorkingStatus,
    string Location,
    bool GenerateBullets,
    int MaxGeneratedBulletCount,
    IReadOnlyCollection<BulletResponse> Bullets);
