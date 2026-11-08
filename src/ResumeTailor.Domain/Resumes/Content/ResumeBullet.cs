

using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Domain.Resumes.Content;

public class ResumeBullet : Entity
{
    public int ResumeExperienceId { get; private set; }

    public int LibraryBulletId { get; private set; }
    public LibraryBullet LibraryBullet { get; private set; } = null!;

    public string? Alternative { get; private set; }
    public string? Reasoning { get; private set; }

    public int SortOrder { get; private set; }


    public ResumeBullet(int libraryBulletId, string? alternative, string? reasoning, int sortOrder)
    {
        LibraryBulletId = libraryBulletId;
        Alternative = alternative;
        Reasoning = reasoning;
        SortOrder = sortOrder;
    }

    public void Update(int libraryBulletId, int sortOrder)
    {
        var sourceChanged = LibraryBulletId != libraryBulletId;
        var orderChanged = SortOrder != sortOrder;

        if (!sourceChanged && !orderChanged)
            return;

        if (sourceChanged)
        {
            LibraryBulletId = libraryBulletId;
            Alternative = null;
            Reasoning = null;
        }

        SortOrder = sortOrder;
        MarkUpdated();
    }

}
