using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Accounts;

public class Bullet : Entity
{
    public int CompanyId { get; private set; }

    public string Value { get; private set; } = string.Empty;
    public int? AiScore { get; private set; }

    public Bullet(int companyId, string value, int? aiScore = null)
    {
        CompanyId = companyId;
        Value = value;
        AiScore = aiScore;
    }

    public void Update(string value, int? aiScore = null)
    {
        Value = value;
        AiScore = aiScore;
        MarkUpdated();
    }
}
