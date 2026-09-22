using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.Content;

public class ResumeBullet : Entity
{
    public int ResumeCompanyId { get; private set; }
    public int? SourceBulletId { get; private set; }

    public string Value { get; private set; } = string.Empty;
    public string? AlternativeValue { get; private set; }

    public int SortOrder { get; private set; }

    public ResumeBullet(int? sourceBulletId, string value, string? alternativeValue, int sortOrder)
    {
        SourceBulletId = sourceBulletId;
        Value = value;
        AlternativeValue = alternativeValue;
        SortOrder = sortOrder;
    }

    public void Update(string value, string? alternativeValue, int sortOrder)
    {
        Value = value;
        AlternativeValue = alternativeValue;
        SortOrder = sortOrder;
        MarkUpdated();
    }
}
