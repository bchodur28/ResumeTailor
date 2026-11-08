namespace ResumeTailor.Application.Resumes.Management.Models;

public sealed record ExperienceSelectionValidation(int LibraryExperienceId, int? LibrarySummaryId, IReadOnlyCollection<int> LibraryBulletIds);
