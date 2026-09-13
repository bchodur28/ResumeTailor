using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Accounts;

public sealed class PersonalLink : Entity
{
    public int AccountId { get; private set; }

    public string DisplayName { get; private set; } = string.Empty;
    public string Url { get; private set; } = string.Empty;

    public PersonalLink(int accountId, string displayName, string url)
    {
        AccountId = accountId;
        DisplayName = displayName;
        Url = url;
    }

    public void Update(string displayName, string url)
    {
        DisplayName = displayName;
        Url = url;
        MarkUpdated();
    }
}
