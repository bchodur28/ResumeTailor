using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeDetailsResponse(
    ResumeResponse Resume,
    ResumeAiAnalysisResponse? AiAnalysis,
    JobPostingResult? JobPosting,
    ApplicationTrackingResponse? ApplicationTracking,
    AiMetaDataResult? AiMetaData
    );
