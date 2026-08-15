using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Resumes;

public class Bullet : Entity
{
    public int CompanyId { get; private set; }
    public Company Company { get; private set; } = null!;

    public string Value { get; private set; } = string.Empty;

    public Bullet(int companyId, string value)
    {
        CompanyId = companyId;
        Value = value;
    }
}
