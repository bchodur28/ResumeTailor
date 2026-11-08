

using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibraryExperienceListItemResponse(
    int Id,
    ExperienceType Type,
    string Title,
    string? Organization,
    int BulletCount,
    int SummaryCount);
