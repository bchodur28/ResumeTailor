using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Accounts;

public sealed class Project : Entity
{
    public int AccountId { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public DateOnly Started { get; private set; }
    public DateOnly? Ended { get; private set; }
    public string? TechStack { get; private set; }
    public string? Link { get; private set; }

    public bool UseForResume { get; private set; }

    public Project(string name, string description, bool useForResume, DateOnly started, DateOnly? ended = null, string? techStack = null, string? link = null)
    {
        Name = name;
        Description = description;
        Started = started;
        Ended = ended;
        TechStack = techStack;
        UseForResume = useForResume;
        Link = link;
    }

    public void Update(string name, string description, bool useForResume, DateOnly started, DateOnly? ended = null, string? techStack = null, string? link = null)
    {
        Name = name;
        Description = description;
        Started = started;
        Ended = ended;
        TechStack = techStack;
        Link = link;
        UseForResume = useForResume;
        MarkUpdated();
    }
}


