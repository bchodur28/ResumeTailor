using ResumeTailor.Application.Contracts.Resume.Selection;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record UpdateResumeRequest(
    string Name,
    IReadOnlyCollection<CompanySelectionRequest> Companies,
    IReadOnlyCollection<EducationSelectionRequest> Education,
    IReadOnlyCollection<ProjectSelectionRequest> Projects,
    AppearanceRequest Appearance);
