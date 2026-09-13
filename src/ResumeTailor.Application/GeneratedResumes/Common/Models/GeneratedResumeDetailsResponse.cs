namespace ResumeTailor.Application.GeneratedResumes.Common.Models;

public sealed record GeneratedResumeDetailsResponse(
    GeneratedResumeResponse GeneratedResume,
    GeneratedSummaryResponse GeneratedSummary,
    AiUsage Usage
    );
