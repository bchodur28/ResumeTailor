using ResumeTailor.Application.JobApplications.Models;

namespace ResumeTailor.Application.JobApplications.Interfaces
{
    public interface IJobApplicationService
    {
        Task<IReadOnlyCollection<JobApplicationListItemResponse>> GetJobApplicationListItemsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
        Task<JobApplicationResponse> GetJobApplicationIdAsync(int id, CancellationToken cancellationToken = default);
        Task CreateJobApplicationAsync(JobApplicationRequest request, CancellationToken cancellationToken = default);
        Task UpdateJobApplicationAsync(int id, JobApplicationRequest request, CancellationToken cancellationToken = default);
        Task DeleteJobApplicationAsync(int id, CancellationToken cancellationToken = default);
    }
}
