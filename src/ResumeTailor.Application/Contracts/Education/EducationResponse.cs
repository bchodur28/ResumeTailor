namespace ResumeTailor.Application.Contracts.Education;

public sealed record EducationResponse(
    int Id,
    int? SelectionId,
    string InstitutionName,
    string Qualification,
    string? FieldOfStudy,
    decimal? Gpa,
    string? Location,
    DateOnly Started,
    DateOnly? Ended,
    bool UseForResume
    );
