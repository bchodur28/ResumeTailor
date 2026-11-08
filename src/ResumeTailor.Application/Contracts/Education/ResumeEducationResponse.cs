using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Contracts.Education;

public sealed record ResumeEducationResponse(
    int Id,
    int LibraryEducationId,
    EducationType Type,
    string InstitutionName,
    string Qualification,
    string? FieldOfStudy,
    decimal? Gpa,
    string? Location,
    DateOnly Started,
    DateOnly? Ended,
    int SortOrder
    );
