namespace ResumeTailor.Application.Contracts.Education;

public sealed record EducationRequest(
    int? Id,
    string SchoolName,
    string Degree,
    string Major,
    DateOnly Started,
    DateOnly? Ended,
    bool UseForResume);
