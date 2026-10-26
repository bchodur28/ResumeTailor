using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.GeneratedResumes.Content;


namespace ResumeTailor.Domain.Resumes.Content;

public sealed class ResumeExperience : Entity
{
    public int ResumeId { get; private set; }
    public int LibraryExperienceId { get; private set; }
    public int? ResumeSummaryId { get; private set; }

    private readonly List<ResumeBullet> _bullets = [];
    public IReadOnlyCollection<ResumeBullet> Bullets => _bullets.AsReadOnly();

    public int SortOrder { get; private set; }


    public ResumeExperience(int libraryExperienceId, int? resumeSummaryId, int sortOrder)
    {
        LibraryExperienceId = libraryExperienceId;
        ResumeSummaryId = resumeSummaryId;
        SortOrder = sortOrder;
    }

    public void Update(int resumeSummaryId, int sortOrder)
    {
        ResumeSummaryId = resumeSummaryId;
        SortOrder = sortOrder;
        MarkUpdated();
    }

    public void AddBullet(int libraryBulletId, string? alternative, string reasoning, int sortOrder)
    {
        var bullet = new ResumeBullet(libraryBulletId, alternative, reasoning, sortOrder);
        _bullets.Add(bullet);
    }

    public void RemoveBullet(ResumeBullet bullet)
    {
        _bullets.Remove(bullet);
    }
}
