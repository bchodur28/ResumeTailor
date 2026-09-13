using ResumeTailor.Application.Accounts.Models;
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
    IReadOnlyList<ResumeEducationResponse> Education,
    IReadOnlyCollection<ResumeProjectResponse> Projects
    );
