using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibraryExperienceRequest(
    ExperienceType Type,
    string Title,
    string? Organization,
    string? Location,
    DateOnly? Started,
    DateOnly? Ended,
    bool LetAiChooseSummary,
    string? Link,
    string? TechStack,
    bool LetAiChooseBullets,
    int? MaxBulletsToChoose,
    IReadOnlyCollection<LibrarySummaryRequest> Summeries,
    IReadOnlyCollection<LibraryBulletRequest> Bullets
    );
