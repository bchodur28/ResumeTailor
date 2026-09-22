namespace ResumeTailor.Application.Contracts.Projects;

public sealed record ProjectResponse(
    int Id,
    string Name,
    string Description,
    DateOnly Started,
    DateOnly? Ended,
    string? TechStack,
    string? Link,
    bool UseForResume);
