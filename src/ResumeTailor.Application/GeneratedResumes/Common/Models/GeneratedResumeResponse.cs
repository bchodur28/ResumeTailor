using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Profile.Education.Models;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record GeneratedResumeResponse(
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
    IReadOnlyCollection<ResumeProjectResponse> Projects
    );
