using ResumeTailor.Application.Contracts.Bullets;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeCompanySelectionRequest(int ResumeId, int CompanyId, int SortOrder, IReadOnlyCollection<ResumeBulletRequest> Bullets);
