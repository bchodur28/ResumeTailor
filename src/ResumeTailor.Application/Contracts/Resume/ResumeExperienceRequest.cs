namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeExperienceRequest(
    int? Id,
    int LibraryExperienceId,
    int? LibrarySummaryId,
    int SortOrder,
    IReadOnlyCollection<ResumeBulletRequest> Bullets);
