using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.Content;

public sealed class ResumeProjectSelection : Entity
{
    public int GeneratedResumeId { get; private set; }
    public int ProjectId { get; private set; }
    public int SortOrder { get; private set; }

    public ResumeProjectSelection(int projectId, int sortOrder)
    {
        ProjectId = projectId;
        SortOrder = sortOrder;
    }

    public void Update(int projectId, int sortOrder)
    {
        ProjectId = projectId;
        SortOrder = sortOrder;
        MarkUpdated();
    }
}
