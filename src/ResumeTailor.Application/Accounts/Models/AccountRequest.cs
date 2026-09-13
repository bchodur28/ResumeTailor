

namespace ResumeTailor.Application.Accounts.Models;

public sealed record AccountRequest(
    string Auth0UserId,
    string Email,
    string DisplayName,
    string City,
    string State,
    string Country,
    IReadOnlyCollection<PersonalLinkRequest> PersonalLinks,
    IReadOnlyCollection<TitleRequest> Titles
    );
