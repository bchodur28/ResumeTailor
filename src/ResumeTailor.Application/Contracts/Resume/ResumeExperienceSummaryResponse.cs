namespace ResumeTailor.Application.Contracts.Resume;


public sealed record ResumeExperienceSummaryResponse(
    int Id,
    int LibrarySummaryId,
    bool WasManuallyChanged,
    string? AiReasoning,
    int SortOrder,
    string Value
    );
