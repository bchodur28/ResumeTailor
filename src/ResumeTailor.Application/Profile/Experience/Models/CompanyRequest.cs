namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record CompanyRequest(
    int? Id,
    int AccountId,
    string Name,
    string Title,
    string Location,
    DateOnly Started,
    DateOnly? Ended,
    bool GenerateBullets,
    int MaxGeneratedBulletCount);
