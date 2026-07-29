using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Extraction;

public sealed class SiteExtractionDefinition : Entity
{

    public string SiteName { get; private set; } = string.Empty;
    public string Hostname { get; private set; } = string.Empty;
    public string PathPattern { get; private set;  } = string.Empty;
    public int Version { get; private set;  }
    public bool IsEnabled { get; private set; }

    private readonly List<FieldExtractionDefinition> _fields = [];
    public IReadOnlyCollection<FieldExtractionDefinition> Fields => _fields.AsReadOnly();

    private SiteExtractionDefinition() { }

    public SiteExtractionDefinition(string siteName, string hostname, string pathPattern, int version)
    {
        SiteName = siteName;
        Hostname = hostname;
        PathPattern = pathPattern;
        Version = version;
        IsEnabled = true;
    }

    public void Update(string siteName, string hostName, string pathPattern, int version, bool isEnabled)
    {
        SiteName = siteName;
        Hostname = hostName;
        PathPattern = pathPattern;
        Version = version;
        IsEnabled = isEnabled;
        MarkUpdated();
    }
}
