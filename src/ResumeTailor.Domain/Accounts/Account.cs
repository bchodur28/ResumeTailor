using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Accounts;

public sealed class Account : Entity
{
    public string Auth0UserId { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    private readonly List<PersonalLink> _personalLinks = [];
    public IReadOnlyCollection<PersonalLink> PersonalLinks => _personalLinks.AsReadOnly();

    private readonly List<Title> _titles = [];
    public IReadOnlyCollection<Title> Titles => _titles.AsReadOnly();

    public Account(string auth0UserId, string email, string displayName, string city, string state, string country)
    {
        Auth0UserId = auth0UserId;
        Email = email;
        DisplayName = displayName;
        City = city;
        State = state;
        Country = country;
    }

    public void Update(string email, string displayName, string city, string state, string country)
    {
        Email = email;
        DisplayName = displayName;
        City = city;
        State = state;
        Country = country;
        MarkUpdated();
    }

    public void AddPersonalLink(string value, string url)
    {
        var personalLink = new PersonalLink(value, url);
        _personalLinks.Add(personalLink);
    }

    public void AddTitle(string value, bool isPrimary)
    {
        var title = new Title(value, isPrimary);
        _titles.Add(title);
    }

    public void RemovePersonalLink(PersonalLink personalLink)
    {
        _personalLinks.Remove(personalLink);
    }

    public void RemoveTitle(Title title)
    {
        _titles.Remove(title);
    }
}
