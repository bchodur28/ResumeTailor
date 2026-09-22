namespace ResumeTailor.Application.Contracts.Education;

public sealed record EducationResponse(
    int Id,
    string SchoolName,
    string Degree,
    string Major,
    DateOnly Started,
    DateOnly? Ended,
    bool UseForResume
    );
