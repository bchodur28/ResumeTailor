namespace ResumeTailor.Application.Extraction.Models;

public sealed record FieldSelectorRequest(int FieldExtractionDefinitionId, string Selector, int Priority);
