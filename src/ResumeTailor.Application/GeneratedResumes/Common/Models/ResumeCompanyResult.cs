using ResumeTailor.Application.GeneratedResumes.Generation.Models;

namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record ResumeCompanyResult(
    int CompanyId,
    string Name,
    string Title,
    string? Location,
    DateOnly Started,
    DateOnly? Ended,
    IReadOnlyList<ResumeBulletResult> Bullets
    );
