namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeCompanyResponse(
    int CompanyId,
    int SelectionId,
    string Name,
    string Title,
    string Location,
    DateOnly Started,
    DateOnly? Ended,
    IReadOnlyList<ResumeBulletResponse> Bullets
    );
