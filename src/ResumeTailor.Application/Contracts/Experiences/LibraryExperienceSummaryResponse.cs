using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibraryExperienceSummaryResponse(
    int Id,
    ExperienceType Type,
    string Title,
    string? Organaization,
    int SummaryCount,
    bool LetAiChooseSummary,
    int BulletCount,
    bool LetAiChooseBullets,
    int? MaxBulletsToChoose
    );
