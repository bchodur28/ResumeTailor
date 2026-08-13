namespace ResumeTailor.Application.Extraction.Models;

public sealed record FieldPatternResponse(int Id, string? ScopePattern, string MatchPattern, int Priority);
