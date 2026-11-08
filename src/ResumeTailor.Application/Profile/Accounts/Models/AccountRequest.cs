namespace ResumeTailor.Application.Profile.Accounts.Models;

public sealed record AccountRequest(
    string Email,
    string DisplayName,
    string Location,
    IReadOnlyCollection<PersonalLinkRequest> PersonalLinks,
    IReadOnlyCollection<TitleRequest> Titles
    );
