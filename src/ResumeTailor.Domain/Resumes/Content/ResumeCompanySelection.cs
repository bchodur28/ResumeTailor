using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.Content;

public sealed class ResumeCompanySelection : Entity
{
    public int ResumeId { get; private set; }
    public int CompanyId { get; private set; }
    public int SortOrder { get; private set; }

    private readonly List<ResumeCompanyBullet> _bullets = new();
    public IReadOnlyCollection<ResumeCompanyBullet> Bullets => _bullets.AsReadOnly();

    public ResumeCompanySelection(int companyId, int sortOrder)
    {
        CompanyId = companyId;
        SortOrder = sortOrder;
    }

    public void Update(int companyId, int sortOrder)
    {
        CompanyId = companyId;
        SortOrder = sortOrder;
        MarkUpdated();
    }

    public void AddResumeBullet(int? sourceBulletId, string value, string? alternativeValue, int sortOrder)
    {
        var bullet = new ResumeCompanyBullet(sourceBulletId, value, alternativeValue, sortOrder);
        _bullets.Add(bullet);
    }

    public void RemoveResumeBullet(ResumeCompanyBullet bullet)
    {
        _bullets.Remove(bullet);
    }
}
