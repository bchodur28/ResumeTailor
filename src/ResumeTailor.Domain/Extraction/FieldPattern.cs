using ResumeTailor.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Domain.Extraction;

public class FieldPattern : Entity
{
    public int FieldExtractionDefinitionId { get; private set; }
    public string? ScopePattern { get; private set; }
    public string MatchPattern { get; private set; } = string.Empty;
    public int Priority { get; private set; }
    public FieldExtractionDefinition FieldExtractionDefinition { get; private set; } = null!;

    public FieldPattern(int fieldExtractionDefinitionId, string matchPattern, int priority, string? scopePattern = null)
    {
        FieldExtractionDefinitionId = fieldExtractionDefinitionId;
        ScopePattern = scopePattern;
        MatchPattern = matchPattern;
        Priority = priority;
    }

    public void Update(int fieldExtractionDefinitionId, string matchPattern, int priority, string? scopePattern = null)
    {
        FieldExtractionDefinitionId = fieldExtractionDefinitionId;
        MatchPattern = matchPattern;
        Priority = priority;
        MarkUpdated();
    }
}
