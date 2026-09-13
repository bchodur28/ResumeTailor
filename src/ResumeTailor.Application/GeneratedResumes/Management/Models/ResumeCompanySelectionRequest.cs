namespace ResumeTailor.Application.GeneratedResumes.Management.Models;

public sealed record ResumeCompanySelectionRequest(int CompanyId, int SortOrder, IReadOnlyCollection<ResumeBulletRequest> Bullets);
