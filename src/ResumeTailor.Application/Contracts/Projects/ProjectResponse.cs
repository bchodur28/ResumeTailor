namespace ResumeTailor.Application.Contracts.Projects;

public sealed record ProjectResponse(
    int Id,
    int? SelectionId,
    string Name,
    string Description,
    DateOnly Started,
    DateOnly? Ended,
    string? TechStack,
    string? Link,
    bool UseForResume);
