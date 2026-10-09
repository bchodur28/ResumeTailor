using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Models;
using ResumeTailor.Domain.Resumes.ApplicationTracking;

namespace ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

public interface IResumeManagementService
{
    Task<PagedResult<ResumeSummaryResponse>> GetPagedResumeSummariesByAccountIdAsync(ResumeSummaryQuery request, CancellationToken cancellationToken = default);
    Task<ResumeDetailsResponse> GetResumeDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateResumeAsync(int id, UpdateResumeRequest request, CancellationToken cancellationToken = default);
    Task DeleteResumeDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task UpdateResumeJobPostingAsync(int resumeId, ResumeJobPostingRequest request, CancellationToken cancellationToken = default);
    Task UpdateResumeApplicationTrackingAsync(int resumeId, ApplicationStatus request, CancellationToken cancellationToken = default);
}
