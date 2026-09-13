using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.JobApplications.Interfaces;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.JobApplications;

internal class JobApplicationRepository(ResumeTailorDbContext dbContext) : IJobApplicationRepository
{
    public async Task<IReadOnlyCollection<JobApplication>> GetJobApplicationsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.JobApplications
            .AsNoTracking()
            .Where(ja => ja.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<JobApplication?> GetJobApplicationByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.JobApplications
            .Include(x => x.GeneratedResume)
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task CreateJobApplicationAsync(JobApplication jobApplication, CancellationToken cancellationToken = default)
    {
        await dbContext.JobApplications.AddAsync(jobApplication, cancellationToken);
    }

    public async Task DeleteJobApplicationAsync(int id, CancellationToken cancellationToken = default)
    {
        var jobApplication = await dbContext.JobApplications.FindAsync([id], cancellationToken);
        if (jobApplication is null)
        {
            return;
        }
        dbContext.JobApplications.Remove(jobApplication);
    }

    public async Task<bool> JobApplicationExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.JobApplications.AnyAsync(ja => ja.Id == id, cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
