namespace ResumeTailor.Application.Profile.Experience.Models;

public sealed record ProjectRequest(
    int? Id,
    int AccountId,
    string Name,
    string Description,
    DateOnly Started,
    DateOnly? Ended,
    string? TechStack,
    string? Link,
    bool UseForResume);
