using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Resume;

public class Bullet : Entity
{
    public int CompanyId { get; private set; }
    public string Value { get; private set; } = string.Empty;

    public Bullet(int companyId, string value, bool useInAI)
    {
        CompanyId = companyId;
        Value = value;
    }
}
