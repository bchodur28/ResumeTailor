namespace ResumeTailor.Application.Extraction.Models;

public sealed record FieldPatternRequest(int FieldExtractionDefinitionId, string? ScropePattern, string MatchPattern, int Priority);
