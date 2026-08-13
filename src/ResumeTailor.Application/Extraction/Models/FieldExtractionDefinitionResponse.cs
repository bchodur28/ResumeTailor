using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction.Models;

public sealed record FieldExtractionDefinitionResponse(
    int Id,
    JobFieldName FieldName,
    string DisplayLabel,
    ExtractionValueType ExtractionType,
    string? AttributeName,
    bool IsRequired,
    int SortOrder,
    IReadOnlyCollection<FieldPatternResponse> Patterns);
