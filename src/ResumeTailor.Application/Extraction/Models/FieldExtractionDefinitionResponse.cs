namespace ResumeTailor.Application.Extraction.Models;

public sealed record FieldExtractionDefinitionResponse(
    int Id,
    string FieldName,
    string DisplayLabel,
    string ExtractionType,
    string? AttributeName,
    bool IsRequired,
    int SortOrder,
    IReadOnlyCollection<FieldSelectorResponse> Selectors);
