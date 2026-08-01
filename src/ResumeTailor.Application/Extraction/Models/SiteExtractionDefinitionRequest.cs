namespace ResumeTailor.Application.Extraction.Models;

public sealed record SiteExtractionDefinitionRequest(
    string SiteName,
    string HostName,
    string PathPattern,
    bool IsEnabled);
