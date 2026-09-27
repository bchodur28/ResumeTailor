using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeResponse(
    int? Id,
    int AccountId,
    string ResumeName,
    AiScoreStaleness AiScoreStaleness,
    string PersonName,
    string Profession,
    string Email,
    string PhoneNumber,
    string Location,
    IReadOnlyCollection<PersonalLinkResponse> PersonalLinks,
    IReadOnlyCollection<ResumeCompanyResult> Companies,
    IReadOnlyCollection<EducationResponse> Education,
    IReadOnlyCollection<ProjectResponse> Projects
    );
