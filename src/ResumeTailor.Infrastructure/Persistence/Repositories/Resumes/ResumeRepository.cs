using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Contracts.Accounts;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;
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

        var totalUnfilteredCount = await query.CountAsync();

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

        return new PagedResult<Resume>(items, totalCount, totalUnfilteredCount, request.Page, request.PageSize);
    }

    public async Task<Resume?> GetResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.Experiences)
                .ThenInclude(x => x.Bullets)
            .Include(x => x.EducationSelections)
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


    public async Task<ResumeDetailsResponse> GetResumeDetails(int id, CancellationToken cancellationToken = default)
    {

        var data = await dbContext.Resumes
            .AsNoTracking()
            .AsSplitQuery()
            .Where(r => r.Id == id)
            .Select(r => new ResumeDetailsResponse
            (
                Resume: new ResumeResponse
                (
                    Id: r.Id,
                    AccountId: r.AccountId,
                    ResumeName: r.Name,
                    AiScore: r.AiScore,
                    AiScoreStaleness: r.AiScoreStaleness,
                    PersonName: r.Account.DisplayName,

                    Profession: r.Account.Titles
                        .Where(t => t.IsPrimary)
                        .Select(t => t.Value)
                        .FirstOrDefault() ?? "",

                    Email: r.Account.Email,
                    PhoneNumber: r.Account.PhoneNumber,
                    Location: r.Account.Location,
                    Summary: r.Account.Summary,

                    PersonalLinks: r.Account.PersonalLinks
                        .Select(pl => new PersonalLinkResponse(
                            Id: pl.Id,
                            DisplayName: pl.DisplayName,
                            Url: pl.Url
                        ))
                        .ToList(),

                    Skills: r.Account.Skills
                        .Select(s => new SkillResponse(
                            Id: s.Id,
                            Value: s.Value
                        ))
                        .ToList(),

                    Education: r.EducationSelections
                        .OrderBy(e => e.SortOrder)
                        .Select(e => new ResumeEducationResponse(
                            e.Id,
                            e.LibraryEducationId,
                            e.LibraryEducation.Type,
                            e.LibraryEducation.InstitutionName,
                            e.LibraryEducation.Qualification,
                            e.LibraryEducation.FieldOfStudy,
                            e.LibraryEducation.Gpa,
                            e.LibraryEducation.Location,
                            e.LibraryEducation.Started,
                            e.LibraryEducation.Ended,
                            e.SortOrder
                        ))
                        .ToList(),

                    Experiences: r.Experiences
                        .OrderBy(e => e.SortOrder)
                        .Select(e => new ResumeExperienceResponse
                        (
                            Id: e.Id,
                            LibraryExperienceId: e.LibraryExperienceId,

                            Type: e.LibraryExperience.Type,
                            Title: e.LibraryExperience.Title,
                            Organization: e.LibraryExperience.Organization,
                            Location: e.LibraryExperience.Location,
                            Started: e.LibraryExperience.Started,
                            Ended: e.LibraryExperience.Ended,
                            PrimarySummary: e.ResumeSummary == null
                                ? null
                                : new ResumeExperienceSummaryResponse(
                                    Id: e.ResumeSummary.Id,
                                    LibrarySummaryId: e.ResumeSummary.LibrarySummaryId,
                                    WasManuallyChanged: e.ResumeSummary.WasManuallyChanged,
                                    AiReasoning: e.ResumeSummary.Reasoning,
                                    SortOrder: e.ResumeSummary.SortOrder,
                                    Value: e.ResumeSummary.LibrarySummary.Value
                                ),
                            SortOrder: e.SortOrder,

                            Bullets: e.Bullets
                                .OrderBy(b => b.SortOrder)
                                .Select(b => new ResumeBulletResponse
                                (
                                    Id: b.Id,
                                    LibraryBulletId: b.LibraryBulletId,
                                    Value: b.LibraryBullet.Value,
                                    AiAlternativeValue: b.Alternative,
                                    AiReasoning: b.Reasoning,
                                    SortOrder: b.SortOrder,
                                    IsSourceDeleted: b.LibraryBullet.DeletedDate != null
                                ))
                                .ToList()
                        ))
                        .ToList(),

                    Appearance: new ResumeAppearanceResponse(
                        Id: r.Appearance.Id,
                        TitleFontSize: r.Appearance.TitleFontSize,
                        SectionHeaderFontSize: r.Appearance.SectionHeaderFontSize,
                        MainBodyFontSize: r.Appearance.MainBodyFontSize,
                        FontFamily: r.Appearance.FontFamily,
                        FontColor: r.Appearance.FontColor,
                        TopHeaderAlignment: r.Appearance.TopHeaderAlignment
                        )
                ),
                AiAnalysis: r.AiAnalysis == null
                ? null
                : new AiAnalysisResult
                (
                    Summary: r.AiAnalysis.Summary,
                    Strengths: r.AiAnalysis.Insights
                        .Where(i => i.Type == ResumeAiInsightType.Strength)
                        .Select(i => i.Value)
                        .ToList(),
                    Weaknesses: r.AiAnalysis.Insights
                        .Where(i => i.Type == ResumeAiInsightType.Weakness)
                        .Select(i => i.Value)
                        .ToList()
                ),
                JobPosting: new JobPostingResult(
                    CompanyName: r.JobPosting.CompanyName,
                    JobTitle: r.JobPosting.JobTitle,
                    Location: r.JobPosting.Location,
                    WorkStyle: r.JobPosting.WorkStyle,
                    SalaryMin: r.JobPosting.SalaryMin,
                    SalaryMax: r.JobPosting.SalaryMax,
                    Salary: r.JobPosting.Salary,
                    SalaryPeriod: r.JobPosting.SalaryPeriod,
                    SalaryCurrency: r.JobPosting.SalaryCurrency
                ),
                ApplicationTracking: new ApplicationTrackingResponse(
                    Status: r.ApplicationTracking.Status,
                    Applied: r.ApplicationTracking.Applied,
                    Interviewed: r.ApplicationTracking.Interviewed,
                    OfferReceived: r.ApplicationTracking.OfferReceived,
                    OfferAccepted: r.ApplicationTracking.OfferAccepted,
                    Rejected: r.ApplicationTracking.Rejected
                ),
                AiMetaData: new AiMetaDataResult(
                    Model: r.AiMetaData.Model,
                    InputTokens: r.AiMetaData.InputTokens,
                    OutputTokens: r.AiMetaData.OutputTokens,
                    TotalTokens: r.AiMetaData.TotalTokens,
                    Cost: r.AiMetaData.Cost
                )
            )).SingleOrDefaultAsync(cancellationToken)
                ?? throw new KeyNotFoundException("Resume not found.");

        return data;
    }


    public async Task<Resume?> GetResumeForUpdatingAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Resumes
            .AsSplitQuery()
            .Include(x => x.Experiences)
                .ThenInclude(x => x.Bullets)
            .Include(x => x.EducationSelections)
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
