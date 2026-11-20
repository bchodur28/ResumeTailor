using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Domain.Common;

namespace ResumeTailor.Application.Contracts.Accounts;

public sealed record AccountRequest(
    string Email,
    string DisplayName,
    string Location,
    ResumeDateFormat DateFormat,
    IReadOnlyCollection<PersonalLinkRequest> PersonalLinks,
    IReadOnlyCollection<TitleRequest> Titles,
    string? Summary
    );
