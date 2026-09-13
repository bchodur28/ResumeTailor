using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.GeneratedResumes.Content;

public sealed class ResumeEducationSelection : Entity
{
    public int GeneratedResumeId { get; private set; }
    public int EducationId { get; private set; }
    public int SortOrder { get; private set; }

    public ResumeEducationSelection(int educationId, int sortOrder)
    {
        EducationId = educationId;
        SortOrder = sortOrder;
    }

    public void Update(int educationId, int sortOrder)
    {
        EducationId = educationId;
        SortOrder = sortOrder;
        MarkUpdated();
    }

}
