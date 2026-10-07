using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.GeneratedResumes;

public class ResumeRepository(ResumeTailorDbContext dbContext) : IResumeRepository
{
    // Resumes
    public async Task<IReadOnlyCollection<Resume>> GetResumesForSummaryByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes
            .AsNoTracking()
            .Include(x => x.JobPosting)
            .Include(x => x.ApplicationTracking)
            .Where(gr => gr.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Resume?> GetResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.CompanySelections)
                .ThenInclude(x => x.Bullets)
            .Include(x => x.EducationSelections)
            .Include(x => x.ProjectSelections)
            .Include(x => x.AiAnalysis!)
                .ThenInclude(x => x.Insights)
            .Include(x => x.AiMetaData)
            .Include(x => x.JobPosting)
            .Include(x => x.ApplicationTracking)
            .Include(x => x.Appearance)
            .SingleOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<Resume?> GetResumeForUpdatingAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes
            .AsSplitQuery()
            .Include(x => x.CompanySelections)
                .ThenInclude(x => x.Bullets)
            .Include(x => x.EducationSelections)
            .Include(x => x.ProjectSelections)
            .Include(x => x.Appearance)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task CreateResumeAsync(Resume resume, CancellationToken cancellationToken = default)
    {
        await dbContext.Resumes.AddAsync(resume, cancellationToken);
    }

    public void DeleteResume(Resume generatedResume)
    {
        dbContext.Resumes.Remove(generatedResume);
    }

    public async Task<bool> ResumeExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes.AnyAsync(gr => gr.Id == id, cancellationToken);
    }

    // Resume Job Posting
    public async Task<ResumeJobPosting?> GetResumeJobPostingForUpdatingAsync(int resumeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeJobPostings
            .SingleOrDefaultAsync(x => x.ResumeId == resumeId, cancellationToken);
    }

    // Resume Application Tracking
    public async Task<ResumeApplicationTracking?> GetResumeApplicationTrackingForUpdatingAsync(int resumeId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeApplicationTrackings
            .SingleOrDefaultAsync(x => x.ResumeId == resumeId, cancellationToken);
    }

    // Save Changes
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    
}
