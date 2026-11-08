using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Profile;

public sealed class Account : Entity
{
    public string Auth0UserId { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;

    private readonly List<PersonalLink> _personalLinks = [];
    public IReadOnlyCollection<PersonalLink> PersonalLinks => _personalLinks.AsReadOnly();

    private readonly List<Title> _titles = [];
    public IReadOnlyCollection<Title> Titles => _titles.AsReadOnly();

    private readonly List<Skill> _skills = [];
    public IReadOnlyCollection<Skill> Skills => _skills.AsReadOnly();

    public Account(string auth0UserId, string email, string displayName, string location)
    {
        Auth0UserId = auth0UserId;
        Email = email;
        DisplayName = displayName;
        Location = location;
    }

    public void Update(string email, string displayName, string location)
    {
        Email = email;
        DisplayName = displayName;
        Location = location;
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

    public void AddSkill(string value)
    {
        var skill = new Skill(value);
        _skills.Add(skill);
    }

    public void RemovePersonalLink(PersonalLink personalLink)
    {
        _personalLinks.Remove(personalLink);
    }

    public void RemoveTitle(Title title)
    {
        _titles.Remove(title);
    }

    public void RemoveSkill(Skill skill)
    {
        _skills.Remove(skill);
    }
}
