using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.AI;

public sealed class ResumeAiMetaData : Entity
{
    public int GeneratedResumeId { get; private set; }

    public int InputTokens { get; private set; }
    public int OutputTokens { get; private set; }
    public int TotalTokens { get; private set; }


    public ResumeAiMetaData(int inputTokens, int outputTokens, int totalTokens)
    {
        InputTokens = inputTokens;
        OutputTokens = outputTokens;
        TotalTokens = totalTokens;
    }

    public void Update(int inputTokens, int outputTokens, int totalTokens)
    {
        InputTokens = inputTokens;
        OutputTokens = outputTokens;
        TotalTokens = totalTokens;
        MarkUpdated();
    }
}
