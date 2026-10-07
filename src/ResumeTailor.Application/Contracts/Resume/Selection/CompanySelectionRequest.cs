namespace ResumeTailor.Application.Contracts.Resume.Selection;

public sealed record CompanySelectionRequest(int? Id, int CompanyId, int SortOrder, IReadOnlyCollection<BulletSelectionRequest> Bullets);
