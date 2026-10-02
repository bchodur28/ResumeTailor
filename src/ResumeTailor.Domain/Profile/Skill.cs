using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Profile;

public sealed class Skill : Entity
{
    public int AccountId { get; private set; }
    public string Value { get; private set; } = string.Empty;

    public Skill(string value)
    {
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
        MarkUpdated();
    }
}
