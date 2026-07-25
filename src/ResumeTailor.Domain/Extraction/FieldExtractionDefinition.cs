using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Extraction;

public sealed class FieldExtractionDefinition : Entity
{
    public int SiteExtractionDefinitionId { get; private set; }
    public SiteExtractionDefinition SiteExtractionDefinition { get; private set; } = null!;
    public JobFieldName FieldName { get; private set; }
    public string DisplayLabel { get; private set; } = string.Empty;
    public ExtractionValueType ExtractionType { get; private set;  }
    public string? AttributeName { get; private set; }
    public bool IsRequired { get; private set; }
    public int SortOrder { get; private set; }

    private readonly List<FieldSelector> _selectors = [];
    public IReadOnlyCollection<FieldSelector> Selectors => _selectors.AsReadOnly();

    private FieldExtractionDefinition() { }

    public FieldExtractionDefinition(int siteExtractionDefinitionId, JobFieldName fieldName, string displayLabel, ExtractionValueType extractionType, bool isRequired, string? attributeName = null)
    {
        SiteExtractionDefinitionId = siteExtractionDefinitionId;
        FieldName = fieldName;
        DisplayLabel = displayLabel;
        ExtractionType = extractionType;
        IsRequired = isRequired;
        AttributeName = attributeName;
    }

    public void Update(int siteExtractionDefinitionId, JobFieldName fieldName, string displayLabel, ExtractionValueType extractionType, bool isRequired, string? attributeName = null)
    {
        SiteExtractionDefinitionId = siteExtractionDefinitionId;
        FieldName = fieldName;
        DisplayLabel = displayLabel;
        ExtractionType = extractionType;
        IsRequired = isRequired;
        AttributeName = attributeName;
        MarkUpdated();
    }
}
