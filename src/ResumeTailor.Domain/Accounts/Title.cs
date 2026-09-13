

using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Accounts;

public sealed class Title : Entity
{
    public int AccountId { get; private set; }
    public string Value { get; private set; } = string.Empty;
    public bool IsPrimary { get; private set; }

}
