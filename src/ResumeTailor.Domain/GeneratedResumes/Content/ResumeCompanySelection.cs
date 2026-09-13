using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.Content;

public sealed class ResumeCompanySelection : Entity
{
    public int GeneratedResumeId { get; private set; }
    public int CompanyId { get; private set; }
    public int SortOrder { get; private set; }

    private readonly List<ResumeBullet> _bullets = new();
    public IReadOnlyCollection<ResumeBullet> Bullets => _bullets.AsReadOnly();

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
        var bullet = new ResumeBullet(sourceBulletId, value, alternativeValue, sortOrder);
        _bullets.Add(bullet);
        MarkUpdated();
    }
}
