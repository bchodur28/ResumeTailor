using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record UpdateResumeRequest(
    string Name,
    int? JobApplicationId,
    IReadOnlyCollection<ResumeCompanySelectionRequest> Companies,
    IReadOnlyCollection<ResumeEducationSelectionRequest> Education,
    IReadOnlyCollection<ResumeProjectSelectionRequest> Projects);
