using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Management.Interfaces;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;


namespace ResumeTailor.Application.Resumes.Management;

internal sealed class ResumeManagementService(
    IResumeRepository repository,
    IResumeDataProvider resumeDataProvider,
    IResumeUpdater resumeUpdater) : IResumeManagementService
{
    public async Task<ResumeDetailsResponse> GetResumeDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var resume = await repository.GetResumeAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Resume with ID {id} was not found.");

        var sourceData = await resumeDataProvider.GetResumeSourceDataForExistingResumeAsync(resume, cancellationToken);

        return ResumeDetailsAssembler.Assemble(sourceData, resume);
    }

    public async Task<PagedResult<ResumeSummaryResponse>> GetPagedResumeSummariesByAccountIdAsync(ResumeSummaryQuery request, CancellationToken cancellationToken = default)
    {
        var result = await repository.GetPagedResumesForSummaryByAccountIdAsync(request, cancellationToken);

        var summeries = result.Items
            .Select(MapToResumeSummaryResponse)
            .ToList();

        return new PagedResult<ResumeSummaryResponse>(
            Items: summeries,
            TotalCount: result.TotalCount,
            Page: result.Page,
            PageSize: result.PageSize);
    }

    public async Task UpdateResumeAsync(int id, UpdateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var existingResume = await repository.GetResumeForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Resume with ID {id} was not found while updating.");

        resumeUpdater.Update(existingResume, request);

        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteResumeDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var resume = await repository.GetResumeForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Resume with ID {id} was not found while deleting.");

        repository.DeleteResume(resume);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateResumeJobPostingAsync(int resumeId, ResumeJobPostingRequest request, CancellationToken cancellationToken = default)
    {
        var existingJobPosting = await repository.GetResumeJobPostingForUpdatingAsync(resumeId, cancellationToken)
            ?? throw new NotFoundException($"Resume job posting with resume ID {resumeId} was not found while updating job posting.");

        existingJobPosting.Update(
            request.CompanyName,
            request.JobTitle,
            request.Location,
            request.WorkStyle,
            request.SalaryMin,
            request.SalaryMax,
            request.Salary,
            request.SalaryPeriod,
            request.SalaryCurrency);

        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateResumeApplicationTrackingAsync(int resumeId, ApplicationStatus request, CancellationToken cancellationToken = default)
    {
        var existingApplicationTracking = await repository.GetResumeApplicationTrackingForUpdatingAsync(resumeId, cancellationToken)
            ?? throw new NotFoundException($"Resume application tracking with resume ID {resumeId} was not found while updating application tracking.");

        var centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Chicago");

        var centralNow = TimeZoneInfo.ConvertTimeFromUtc(
            DateTime.UtcNow,
            centralTimeZone);

        var today = DateOnly.FromDateTime(centralNow);

        existingApplicationTracking.UpdateStatus(request, today);

        await repository.SaveAsync(cancellationToken);
    }

    // Mapping Methods
    private static ResumeSummaryResponse MapToResumeSummaryResponse(Resume resume)
    {
        return new ResumeSummaryResponse(
            Id: resume.Id,
            AccountId: resume.AccountId,
            Name: resume.Name,

            JobPosting: new JobPostingResult(
                CompanyName: resume.JobPosting.CompanyName,
                JobTitle: resume.JobPosting.JobTitle,
                Location: resume.JobPosting.Location,
                WorkStyle: resume.JobPosting.WorkStyle,
                SalaryMin: resume.JobPosting.SalaryMin,
                SalaryMax: resume.JobPosting.SalaryMax,
                Salary: resume.JobPosting.Salary,
                SalaryPeriod: resume.JobPosting.SalaryPeriod,
                SalaryCurrency: resume.JobPosting.SalaryCurrency),

            ApplicationTracking: new ApplicationTrackingResponse(
                Status: resume.ApplicationTracking.Status,
                Applied: resume.ApplicationTracking.Applied,
                Interviewed: resume.ApplicationTracking.Interviewed,
                OfferReceived: resume.ApplicationTracking.OfferReceived,
                OfferAccepted: resume.ApplicationTracking.OfferAccepted,
                Rejected: resume.ApplicationTracking.Rejected)
            );
    }
}
