using ResumeTailor.Application.Resumes.Common.Models;

namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record ResumeCompanyResult(
    int CompanyId,
    int? SelectionId,
    string Name,
    string Title,
    string? Location,
    DateOnly Started,
    DateOnly? Ended,
    IReadOnlyList<ResumeBulletResult> Bullets
    );
