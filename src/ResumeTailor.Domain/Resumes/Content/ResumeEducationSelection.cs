using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Domain.GeneratedResumes.Content;

public sealed class ResumeEducationSelection : Entity
{
    public int ResumeId { get; private set; }

    public int LibraryEducationId { get; private set; }
    public LibraryEducation LibraryEducation { get; set; } = null!;

    public int SortOrder { get; private set; }

    public ResumeEducationSelection(int libraryEducationId, int sortOrder)
    {
        LibraryEducationId = libraryEducationId;
        SortOrder = sortOrder;
    }

    public void Update(int educationId, int sortOrder)
    {
        LibraryEducationId = educationId;
        SortOrder = sortOrder;
        MarkUpdated();
    }

}
