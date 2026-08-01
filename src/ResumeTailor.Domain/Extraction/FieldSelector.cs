using ResumeTailor.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Domain.Extraction;

public class FieldSelector : Entity
{
    public int FieldExtractionDefinitionId { get; private set; }
    public string Selector { get; private set; } = string.Empty;
    public int Priority { get; private set; }
    public FieldExtractionDefinition FieldExtractionDefinition { get; private set; } = null!;

    public FieldSelector(int fieldExtractionDefinitionId, string selector, int priority)
    {
        FieldExtractionDefinitionId = fieldExtractionDefinitionId;
        Selector = selector;
        Priority = priority;
    }

    public void Update(int fieldExtractionDefinitionId, string selector, int priority)
    {
        FieldExtractionDefinitionId = fieldExtractionDefinitionId;
        Selector = selector;
        Priority = priority;
        MarkUpdated();
    }
}
