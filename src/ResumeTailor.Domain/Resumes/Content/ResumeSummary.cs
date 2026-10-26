using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Resumes.Content;

public class ResumeSummary : Entity
{
    public int ExperienceId { get; private set; }

    public int LibrarySummaryId { get; private set; }
    public bool WasManuallyChanged { get; private set; } = false;

    public int SortOrder { get; private set; }
    public string Reasoning { get; private set; }

    public ResumeSummary(int experienceId, int librarySummaryId, int sortOrder, string reasoning)
    {
        ExperienceId = experienceId;
        LibrarySummaryId = librarySummaryId;
        SortOrder = sortOrder;
        Reasoning = reasoning;
    }

    public void Update(int librarySummaryId, int sortOrder)
    {
        if (LibrarySummaryId != librarySummaryId)
        {
            WasManuallyChanged = true;
            Reasoning = "User changed the selected summary. Original AI reasoning is no longer applicable.";
        }

        LibrarySummaryId = librarySummaryId;
        SortOrder = sortOrder;

        MarkUpdated();
    }
}
