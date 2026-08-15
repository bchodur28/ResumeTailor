

using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Resume;

public class Project : Entity
{
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? Status { get; private set; }
    public string? TechStack { get; private set; }
    public string? Link { get; private set; }
}
