using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Profile;


namespace ResumeTailor.Domain.Resumes.Content;

public class ResumeSummary : Entity
{
    public int ResumeExperienceId { get; private set; }

    public int LibrarySummaryId { get; private set; }
    public LibrarySummary LibrarySummary { get; private set; } = null!;

    public bool WasManuallyChanged { get; private set; } = false;

    public int SortOrder { get; private set; }
    public string? Reasoning { get; private set; }

    public ResumeSummary() { }

    public ResumeSummary(int librarySummaryId, int sortOrder, string? reasoning)
    {
        LibrarySummaryId = librarySummaryId;
        SortOrder = sortOrder;
        Reasoning = reasoning;
    }

    public void Update(int librarySummaryId)
    {
        if (LibrarySummaryId == librarySummaryId)
            return;

        LibrarySummaryId = librarySummaryId;
        WasManuallyChanged = true;
        Reasoning = null;

        MarkUpdated();
    }
}
