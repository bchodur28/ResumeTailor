using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.AI;

public class ResumeAiAnalysis : Entity
{
    public int GeneratedResumeId { get; private set; }

    public string Summary { get; private set; } = string.Empty;
    public int Score { get; private set; }

    private readonly List<ResumeAiInsight> _insights = [];
    public IReadOnlyCollection<ResumeAiInsight> Insights => _insights.AsReadOnly();

    public IEnumerable<ResumeAiInsight> Strengths => _insights.Where(i => i.Type == ResumeAiInsightType.Strength);
    public IEnumerable<ResumeAiInsight> Weaknesses => _insights.Where(i => i.Type == ResumeAiInsightType.Weakness);

    public ResumeAiAnalysis(string summary, int score)
    {
        Summary = summary;
        Score = score;
    }

    public void Update(string summary, int score)
    {
        Summary = summary;
        Score = score;
        MarkUpdated();
    }

    public void AddInsight(
    ResumeAiInsightType type,
    string value)
    {
        _insights.Add(new ResumeAiInsight(type, value));
    }
}
