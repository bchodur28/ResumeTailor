using ResumeTailor.Domain.Common;

namespace ResumeTailor.Application.Profile.Accounts.Models;

public sealed record AccountRequest(
    string Email,
    string DisplayName,
    string Location,
    ResumeDateFormat DateFormat,
    IReadOnlyCollection<PersonalLinkRequest> PersonalLinks,
    IReadOnlyCollection<TitleRequest> Titles,
    string? Summary
    );
