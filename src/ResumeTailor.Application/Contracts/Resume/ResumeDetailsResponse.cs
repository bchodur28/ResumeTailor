using ResumeTailor.Application.GeneratedResumes.Common.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeDetailsResponse(
    ResumeResponse Resume,
    ResumeSummaryResponse ResumeSummary,
    AiUsage Usage
    );
