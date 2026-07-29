namespace ResumeTailor.Application.Extraction.Models;

public sealed record SiteExtractionDefinitionResponse(
    int Id,
    string SiteName,
    string Hostname,
    string PathPattern,
    int Version,
    bool IsEnabled,
    IReadOnlyCollection<FieldExtractionDefinitionResponse> Fields);
