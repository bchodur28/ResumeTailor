using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.GeneratedResumes;

public class ResumeRepository(ResumeTailorDbContext dbContext) : IResumeRepository
{
    // Resumes
    public async Task<PagedResult<Resume>> GetPagedResumesForSummaryByAccountIdAsync(ResumeSummaryQuery request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Resumes
            .AsNoTracking()
            .Include(r => r.ApplicationTracking)
            .Include(r => r.JobPosting)
            .Where(r => r.AccountId == request.AccountId);

        if (request.Statuses is { Count: > 0 })
        {
            query = query.Where(r => request.Statuses.Contains(r.ApplicationTracking.Status));
        }

        var today = DateOnly.FromDateTime(DateTime.Now);
        var startOfWeek = today.AddDays(-((int)today.DayOfWeek + 6) % 7);
        var startOfMonth = new DateOnly(today.Year, today.Month, 1);

        query = request.DateFilter switch
        {
            ResumeSummaryDateFilter.Today => query.Where(r => r.ApplicationTracking.Applied == today),

            ResumeSummaryDateFilter.ThisWeek => query.Where(r => r.ApplicationTracking.Applied >= startOfWeek),

            ResumeSummaryDateFilter.ThisMonth => query.Where(r => r.ApplicationTracking.Applied >= startOfMonth),

            _ => query
        };

        var totalCount = await query.CountAsync(cancellationToken);

        IOrderedQueryable<Resume> orderedQuery = request.SortBy switch
        {
            ResumeSummarySortBy.AppliedDate =>
                request.Descending
                    ? query.OrderByDescending(r => r.ApplicationTracking.Applied)
                    : query.OrderBy(r => r.ApplicationTracking.Applied),

            ResumeSummarySortBy.InterviededDate =>
                request.Descending
                    ? query.OrderByDescending(r => r.ApplicationTracking.Interviewed)
                    : query.OrderBy(r => r.ApplicationTracking.Interviewed),

            ResumeSummarySortBy.Salary =>
                request.Descending
                    ? query.OrderByDescending(r => r.JobPosting.Salary ?? r.JobPosting.SalaryMax)
                    : query.OrderBy(r => r.JobPosting.Salary ?? r.JobPosting.SalaryMin),

            _ => query.OrderByDescending(j => j.ApplicationTracking.Applied)
        };

        var items = await orderedQuery
            .ThenBy(j => j.Id)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<Resume>(items, totalCount, request.Page, request.PageSize);
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
