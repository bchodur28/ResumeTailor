namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeBulletRequest(int? Id, int LibraryBulletId, int SortOrder);
