using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

public interface IResumeRepository
{
    // Generated Resume
    Task<IReadOnlyCollection<Resume>> GetResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<Resume?> GetResumeAsync(int id, CancellationToken cancellationToken = default);
    Task<Resume?> GetResumeForUpdatingAsync(int id, CancellationToken cancellationToken = default);
    Task CreateResumeAsync(Resume generatedResume, CancellationToken cancellationToken = default);
    void DeleteResumeAsync(Resume generatedResume);

    Task<ResumeJobPosting?> GetResumeJobPostingForUpdatingAsync(int resumeId, CancellationToken cancellationToken = default);
    Task<ResumeApplicationTracking?> GetResumeApplicationTrackingForUpdatingAsync(int resumeId, CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
