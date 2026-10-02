using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record UpdateResumeRequest(
    string Name,
    IReadOnlyCollection<ResumeCompanySelectionRequest> Companies,
    IReadOnlyCollection<ResumeEducationSelectionRequest> Education,
    IReadOnlyCollection<ResumeProjectSelectionRequest> Projects,
    ResumeAppearanceRequest Appearance);
