namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibraryBulletRequest(int? Id, int ExperienceId, string Value, bool IsDeleting);
