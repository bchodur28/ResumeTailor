using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Profile;

public sealed class LibraryBullet : Entity
{
    public int ExperienceId { get; private set; }

    public string Value { get; private set; } = string.Empty;

    public string? AiRewrite { get; private set; }
    public string? AiOpinion { get; private set; }
    public int? AiScore { get; private set; }

    public DateTime? DeletedDate { get; private set; }
    public bool IsDeleted => DeletedDate.HasValue;

    public LibraryBullet(int experienceId, string value)
    {
        ExperienceId = experienceId;
        Value = value;
    }

    public void Update(string value)
    {
        Value = value;
        MarkUpdated();
    }

    public void SetAiRewrite(string? aiRewrite)
    {
        AiRewrite = aiRewrite;
    }

    public void SetAiOpinion(string? aiOpinion)
    {
        AiOpinion = aiOpinion;
    }

    public void SetAiScore(int? aiScore)
    {
        AiScore = aiScore;
    }

    public void Delete()
    {
        DeletedDate = DateTime.UtcNow;
        MarkUpdated();
    }
}
