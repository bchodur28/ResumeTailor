using ResumeTailor.Application.Contracts.Accounts;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeResponse(
    int? Id,
    int AccountId,
    string ResumeName,
    int? AiScore,
    AiScoreStaleness AiScoreStaleness,
    string PersonName,
    string Profession,
    string Email,
    string PhoneNumber,
    string Location,
    string? Summary,
    IReadOnlyCollection<PersonalLinkResponse> PersonalLinks,
    IReadOnlyCollection<SkillResponse> Skills,
    IReadOnlyCollection<ResumeEducationResponse> Education,
    IReadOnlyCollection<ResumeExperienceResponse> Experiences,
    ResumeAppearanceResponse Appearance
    );
