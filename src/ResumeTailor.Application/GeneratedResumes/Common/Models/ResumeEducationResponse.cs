namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record ResumeEducationResponse(
    int Id,
    string SchoolName,
    string Degree,
    string Major,
    DateOnly Started,
    DateOnly? Ended
    );
