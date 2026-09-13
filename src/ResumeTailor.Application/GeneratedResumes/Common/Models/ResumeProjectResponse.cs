namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record ResumeProjectResponse(
    int Id,
    string Name,
    string Description,
    DateOnly Started,
    DateOnly? Ended,
    string? TechStack,
    string? Link);
