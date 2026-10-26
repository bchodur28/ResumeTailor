using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibraryExperienceResponse(
    int Id,
    ExperienceType Type,
    string Title,
    string? Organization,
    string? Location,
    DateOnly? Started,
    DateOnly? Ended,
    LibrarySummaryResponse? PrimarySummary,
    bool LetAiChooseSummary,
    IReadOnlyCollection<LibrarySummaryResponse> Summeries,
    string? Link,
    string? TechStack,
    bool LetAiChooseBullets,
    int? MaxBulletsToChoose,
    IReadOnlyCollection<LibraryBulletResponse> Bullets
    );
