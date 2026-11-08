using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Contracts.Experiences
{
    public sealed record ResumeExperienceResponse(
        int Id,
        int LibraryExperienceId,
        ExperienceType Type,
        string Title,
        string? Organization,
        string? Location,
        DateOnly? Started,
        DateOnly? Ended,
        ResumeExperienceSummaryResponse? PrimarySummary,
        int SortOrder,
        IReadOnlyCollection<ResumeBulletResponse> Bullets
        );
}
