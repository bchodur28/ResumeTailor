using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.Profile.Accounts.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeResponse(
    int? Id,
    int AccountId,
    string PersonName,
    string Profession,
    string Email,
    string PhoneNumber,
    string Location,
    IReadOnlyList<PersonalLinkResponse> PersonalLinks,
    IReadOnlyList<ResumeCompanyResult> Companies,
    IReadOnlyList<EducationResponse> Education,
    IReadOnlyCollection<ProjectResponse> Projects
    );
