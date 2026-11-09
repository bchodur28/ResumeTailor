using ResumeTailor.Application.Contracts.Accounts;
using ResumeTailor.Domain.Common;

namespace ResumeTailor.Application.Profile.Accounts.Models;

public sealed record AccountResponse(
    int Id,
    string Auth0UserId,
    string Email,
    string DisplayName,
    string Location,
    ResumeDateFormat DateFormat,
    IReadOnlyCollection<PersonalLinkResponse> PersonalLinks,
    IReadOnlyCollection<TitleResponse> Titles,
    IReadOnlyCollection<SkillResponse> Skills,
    
    string? Summary
    );

