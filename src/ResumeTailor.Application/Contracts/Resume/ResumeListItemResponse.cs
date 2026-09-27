using ResumeTailor.Application.Resumes.Common.Models;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeListItemResponse(
    int Id,
    int AccountId,
    string Name,
    JobPostingResult? JobPosting,
    ApplicationTrackingResponse? ApplicationTracking);
