

using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Resumes.Content;

public class ResumeBullet : Entity
{
    public int ResumeExperienceId { get; private set; }
    public int LibraryBulletId { get; private set; }

    public string? Alternative { get; private set; }
    public string? Reasoning { get; private set; }

    public int SortOrder { get; private set; }


    public ResumeBullet(int libraryBulletId, string? alternative, string reasoning, int sortOrder)
    {
        LibraryBulletId = libraryBulletId;
        Alternative = alternative;
        Reasoning = reasoning;
        SortOrder = sortOrder;
    }

    public void Update(string? alternative, string reasoning, int sortOrder)
    {
        Alternative = alternative;
        Reasoning = reasoning;
        SortOrder = sortOrder;
        MarkUpdated();
    }


}
