using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Common.Exceptions;

namespace ResumeTailor.Domain.Profile;

public sealed class LibraryExperience : Entity
{
    public int AccountId { get; private set; }

    public ExperienceType Type { get; private set; }

    public string Title { get; private set; } = string.Empty;
    public string? Organization { get; private set; }
    public string? Location { get; private set; }

    public DateOnly? Started { get; private set; }
    public DateOnly? Ended { get; private set; }

    public bool LetAiChooseSummary { get; private set; }
    private readonly List<LibrarySummary> _summaries = [];
    public IReadOnlyCollection<LibrarySummary> Summaries => _summaries.AsReadOnly();

    public LibrarySummary? PrimarySummary => _summaries.FirstOrDefault(s => s.IsPrimary && !s.IsDeleted);

    public string? Link { get; private set; }
    public string? TechStack { get; private set; }

    public bool LetAiChooseBullets { get; private set; }
    public int? MaxBulletsToChoose { get; private set; }
    private readonly List<LibraryBullet> _bullets = [];
    public IReadOnlyCollection<LibraryBullet> Bullets => _bullets.AsReadOnly();

    public LibraryExperience(int accountId, ExperienceType type, string title, string? organization, string? location, DateOnly? started, DateOnly? ended, bool letAiChooseSummary, string? link, string? techStack, bool letAiChooseBullets, int? maxBulletsToChoose)
    {
        AccountId = accountId;
        Type = type;
        SetTitle(title);
        Organization = organization;
        Location = location;
        Started = started;
        Ended = ended;
        LetAiChooseSummary = letAiChooseSummary;
        Link = link;
        TechStack = techStack;
        LetAiChooseBullets = letAiChooseBullets;
        MaxBulletsToChoose = maxBulletsToChoose;
    }

    public void Update(ExperienceType type, string title, string? organization, string? location, DateOnly? started, DateOnly? ended, bool letAiChooseSummary, string? link, string? techStack, bool letAiChooseBullets, int? maxBulletsToChoose)
    {
        Type = type;
        SetTitle(title);
        Organization = organization;
        Location = location;
        Started = started;
        Ended = ended;
        LetAiChooseSummary = letAiChooseSummary;
        Link = link;
        TechStack = techStack;
        LetAiChooseBullets = letAiChooseBullets;
        MaxBulletsToChoose = maxBulletsToChoose;
        MarkUpdated();
    }

    public void SetPrimarySummary(LibrarySummary summary)
    {
        if (!_summaries.Contains(summary))
        {
            throw new DomainValidationException("The summary does not belong to this experience");
        }
        if (summary.IsDeleted)
        {
            throw new DomainValidationException("A deleted summary cannot be primary");
        }

        foreach (var existing in _summaries)
        {
            existing.SetIsPrimary(ReferenceEquals(existing, summary));
        }

    }

    public void AddSummary(LibrarySummary summary)
    {
        _summaries.Add(summary);
    }

    public void RemoveSummary(LibrarySummary summary)
    {
        _summaries.Remove(summary);
    }

    public void AddBullet(LibraryBullet bullet)
    {
        _bullets.Add(bullet);
    }

    public void RemoveBullet(LibraryBullet bullet)
    {
        _bullets.Remove(bullet);
    }

    public void SetTitle(string title)
    {
        Title = title;
    }
}
