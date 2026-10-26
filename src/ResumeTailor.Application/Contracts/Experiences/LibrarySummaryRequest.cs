namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibrarySummaryRequest(int? Id, int ExperienceId, string Value, bool IsPrimary, bool IsDeleting);
