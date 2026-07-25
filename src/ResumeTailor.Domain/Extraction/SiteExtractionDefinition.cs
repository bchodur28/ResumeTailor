using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Extraction;

public sealed class SiteExtractionDefinition : Entity
{

    public string SiteName { get; private set; } = string.Empty;
    public int Version { get; private set;  }
    public bool IsEnabled { get; private set; }

    private readonly List<FieldExtractionDefinition> _fields;
    public IReadOnlyCollection<FieldExtractionDefinition> Fields => _fields.AsReadOnly();

    private SiteExtractionDefinition() { }

    public SiteExtractionDefinition(string siteName, int version, bool isEnabled)
    {
        SiteName = siteName;
        Version = version;
        IsEnabled = isEnabled;
    }

    public void Update(string siteName, int version, bool isEnabled)
    {
        SiteName = siteName;
        Version = version;
        IsEnabled = isEnabled;
        MarkUpdated();
    }
}
