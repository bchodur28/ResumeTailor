

using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction.Models;

public sealed record FieldExtractionDefinitionRequest(
    int SiteExtractionDefinitionId,
    JobFieldName FieldName,
    string DisplayLabel,
    ExtractionValueType ExtractionType,
    string? AttributeName,
    bool IsRequired,
    int SortOrder
    );
