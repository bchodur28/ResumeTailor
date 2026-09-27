using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.AI;

public sealed class ResumeAiMetaData : Entity
{
    public int ResumeId { get; private set; }

    public string Model { get; private set; } = string.Empty;
    public int InputTokens { get; private set; }
    public int OutputTokens { get; private set; }
    public int TotalTokens { get; private set; }
    public decimal Cost { get; private set; }


    public ResumeAiMetaData(string model, int inputTokens, int outputTokens, int totalTokens, decimal cost)
    {
        Model = model;
        InputTokens = inputTokens;
        OutputTokens = outputTokens;
        TotalTokens = totalTokens;
        Cost = cost;
    }

    public void Update(string model, int inputTokens, int outputTokens, int totalTokens, decimal cost)
    {
        Model = model;
        InputTokens = inputTokens;
        OutputTokens = outputTokens;
        TotalTokens = totalTokens;
        Cost = cost;
        MarkUpdated();
    }
}
