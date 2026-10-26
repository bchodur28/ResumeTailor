namespace ResumeTailor.Application.Contracts.Education;

public sealed record EducationRequest(
    int? Id,
    string InstitutionName,
    string Qualification,
    string? FieldOfStudy,
    decimal? Gpa,
    string? Location,
    DateOnly Started,
    DateOnly? Ended,
    bool UseForResume);
