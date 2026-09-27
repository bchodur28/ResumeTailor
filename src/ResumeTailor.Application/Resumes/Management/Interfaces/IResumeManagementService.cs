using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Models;

namespace ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

public interface IResumeManagementService
{
    Task<IReadOnlyCollection<ResumeListItemResponse>> GetResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<ResumeDetailsResponse> GetResumeDetailsAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateResumeAsync(int id, UpdateResumeRequest request, CancellationToken cancellationToken = default);
    Task DeleteResumeDetailsAsync(int id, CancellationToken cancellationToken = default);

    Task UpdateResumeJobPostingAsync(int resumeId, ResumeJobPostingRequest request, CancellationToken cancellationToken = default);
    Task UpdateResumeApplicationTrackingAsync(int resumeId, ResumeApplicationTrackingRequest request, CancellationToken cancellationToken = default);
}
