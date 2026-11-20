using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Domain.Resumes.ApplicationTracking;

namespace ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

public interface IResumeManagementService
{
    Task<PagedResult<ResumeSummaryResponse>> GetPagedResumeSummariesByAccountIdAsync(string auth0UserId, ResumeSummaryQuery request, CancellationToken cancellationToken = default);
    Task<ResumeDetailsResponse> GetResumeDetailsAsync(string auth0UserId, int id, CancellationToken cancellationToken = default);
    Task UpdateResumeAsync(string auth0UserId, int id, UpdateResumeRequest request, CancellationToken cancellationToken = default);
    Task DeleteResumeDetailsAsync(string auth0UserId, int id, CancellationToken cancellationToken = default);

    Task UpdateResumeJobPostingAsync(string auth0UserId, int resumeId, ResumeJobPostingRequest request, CancellationToken cancellationToken = default);
    Task UpdateResumeApplicationTrackingAsync(string auth0UserId, int resumeId, ApplicationStatus request, CancellationToken cancellationToken = default);
}
