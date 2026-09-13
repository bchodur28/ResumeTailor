using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Application.JobApplications.Interfaces;

public interface IJobApplicationRepository
{
    Task<IReadOnlyCollection<JobApplication>> GetJobApplicationsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<JobApplication?> GetJobApplicationByIdAsync(int id, CancellationToken cancellationToken = default);
    Task CreateJobApplicationAsync(JobApplication jobApplication, CancellationToken cancellationToken = default);
    Task DeleteJobApplicationAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> JobApplicationExistsAsync(int id, CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken = default);

}
