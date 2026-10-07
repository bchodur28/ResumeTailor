using ResumeTailor.Application.Resumes.Common.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeDetailsResponse(
    ResumeResponse Resume,
    AiAnalysisResult? AiAnalysis,
    JobPostingResult JobPosting,
    ApplicationTrackingResponse ApplicationTracking,
    AiMetaDataResult AiMetaData
    );
