

namespace ResumeTailor.Application.Resumes.Models;

public sealed record CompanyRequest(
    int ResumeId,
    string Name,
    string Position,
    string WorkingStatus,
    string Location,
    bool GenerateBullets,
    int MaxGeneratedBulletCount);

