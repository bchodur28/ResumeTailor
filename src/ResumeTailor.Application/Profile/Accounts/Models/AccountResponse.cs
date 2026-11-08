using ResumeTailor.Application.Contracts.Accounts;

namespace ResumeTailor.Application.Profile.Accounts.Models;

public sealed record AccountResponse(
    int Id,
    string Auth0UserId,
    string Email,
    string DisplayName,
    string Location,
    IReadOnlyCollection<PersonalLinkResponse> PersonalLinks,
    IReadOnlyCollection<TitleResponse> Titles,
    IReadOnlyCollection<SkillResponse> Skills
    );

