using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Common.Exceptions;

namespace ResumeTailor.Domain.Profile;

public sealed class LibrarySummary : Entity
{
    public int ExperienceId { get; private set; }
    public bool IsPrimary { get; private set; }

    public string Value { get; private set; } = string.Empty;

    public string? AiRewrite { get; private set; }
    public string? AiOpinion { get; private set; }
    public int? AiScore { get; private set; }

    public DateTime? DeletedDate { get; private set; }
    public bool IsDeleted => DeletedDate.HasValue;

    public LibrarySummary(int experienceId, string value)
    {
        ExperienceId = experienceId;
        SetValue(value);
    }

    public void Update(string value)
    {
        SetValue(value);
        MarkUpdated();
    }

    public void SetIsPrimary(bool isPrimary)
    {
        IsPrimary = isPrimary;
    }

    public void SetValue(string value)
    {
        if (value.Trim().Length == 0 || value.Length > 500)
        {
            throw new DomainValidationException("The value for bullets must be between 1 and 500 characters.");
        }

        Value = value;
    }

    public void SetAiRewrite(string? aiRewrite)
    {
        AiRewrite = aiRewrite;
        MarkUpdated();
    }

    public void SetAiOpinion(string? aiOpinion)
    {
        AiOpinion = aiOpinion;
        MarkUpdated();
    }

    public void SetAiScore(int? aiScore)
    {
        AiScore = aiScore;
        MarkUpdated();
    }

    public void Delete()
    {
        DeletedDate = DateTime.UtcNow;
        IsPrimary = false;
        MarkUpdated();
    }
}
