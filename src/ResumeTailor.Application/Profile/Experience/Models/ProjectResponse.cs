namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record ProjectResponse(
    int Id,
    string Name,
    string Description,
    DateOnly Started,
    DateOnly? Ended,
    string? TechStack,
    string? Link,
    bool UseForResume);
