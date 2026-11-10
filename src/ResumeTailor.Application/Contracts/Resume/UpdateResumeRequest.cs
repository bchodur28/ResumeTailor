using ResumeTailor.Application.Contracts.Resume.Selection;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record UpdateResumeRequest(
    string Name,
    IReadOnlyCollection<ResumeEducationRequest> Education,
    IReadOnlyCollection<ResumeExperienceRequest> Experiences,
    AppearanceRequest Appearance);
