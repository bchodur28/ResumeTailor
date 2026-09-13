using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.AI;

public sealed class ResumeAiInsight : Entity
{
    public int ResumeAiAnalysisId { get; private set; }
    public ResumeAiInsightType Type { get; private set; }
    public string Value { get; private set; } = string.Empty;

    public ResumeAiInsight(ResumeAiInsightType type, string value)
    {
        Type = type;
        Value = value;
    }

    public void Update(ResumeAiInsightType type, string value)
    {
        Type = type;
        Value = value;
        MarkUpdated();
    }
}
