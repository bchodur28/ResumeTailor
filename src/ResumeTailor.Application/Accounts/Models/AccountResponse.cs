namespace ResumeTailor.Application.Accounts.Models;

public sealed record AccountResponse(
    int Id,
    string Auth0UserId,
    string Email,
    string DisplayName,
    string City,
    string State,
    string Country,
    IReadOnlyCollection<PersonalLinkResponse> PersonalLinks,
    IReadOnlyCollection<TitleResponse> Titles
    );

