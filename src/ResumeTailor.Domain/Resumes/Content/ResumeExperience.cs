using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Profile;


namespace ResumeTailor.Domain.Resumes.Content;

public sealed class ResumeExperience : Entity
{
    public int ResumeId { get; private set; }

    public int LibraryExperienceId { get; private set; }
    public LibraryExperience LibraryExperience { get; private set; } = null!;

    public ResumeSummary? ResumeSummary { get; private set; }

    private readonly List<ResumeBullet> _bullets = [];
    public IReadOnlyCollection<ResumeBullet> Bullets => _bullets.AsReadOnly();

    public int SortOrder { get; private set; }

    public ResumeExperience(int libraryExperienceId, int sortOrder)
    {
        LibraryExperienceId = libraryExperienceId;
        SortOrder = sortOrder;
    }

    public void Update(int sortOrder)
    {
        if (SortOrder == sortOrder)
            return;

        SortOrder = sortOrder;
        MarkUpdated();
    }

    public void UpdateSummary(int? librarySummaryId)
    {
        if (librarySummaryId is null)
        {
            ResumeSummary = null;
            MarkUpdated();
            return;
        }

        if (ResumeSummary is null)
        {
            ResumeSummary = new ResumeSummary(librarySummaryId.Value, sortOrder: 1,reasoning: null);

            MarkUpdated();
            return;
        }

        ResumeSummary.Update(librarySummaryId.Value);
    }

    public void SetSummary(int librarySummaryId,int sortOrder,string? reasoning)
    {
        if (ResumeSummary is not null)
            throw new InvalidOperationException("This experience already has a summary.");

        ResumeSummary = new ResumeSummary(librarySummaryId, sortOrder, reasoning);
    }

    public void AddBullet(int libraryBulletId, string? alternative, string? reasoning, int sortOrder)
    {
        var bullet = new ResumeBullet(libraryBulletId, alternative, reasoning, sortOrder);
        _bullets.Add(bullet);
    }

    public void RemoveBullet(ResumeBullet bullet)
    {
        _bullets.Remove(bullet);
    }
}
