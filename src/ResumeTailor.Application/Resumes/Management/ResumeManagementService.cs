using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Library.Interfaces;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Management.Interfaces;
using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;


namespace ResumeTailor.Application.Resumes.Management;

internal sealed class ResumeManagementService(
    IResumeRepository repository,
    IResumeUpdater resumeUpdater,
    ILibraryExperienceRepository libraryExperienceRepository) : IResumeManagementService
{
    public async Task<ResumeDetailsResponse> GetResumeDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var resumeDetails = await repository.GetResumeDetails(id, cancellationToken)
            ?? throw new NotFoundException($"Resume details with ID {id} was not found.");

        return resumeDetails;
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
            TotalUnfilteredCount: result.TotalUnfilteredCount,
            Page: result.Page,
            PageSize: result.PageSize);
    }

    public async Task UpdateResumeAsync(int id, UpdateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var existingResume = await repository.GetResumeForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Resume with ID {id} was not found while updating.");

        var accountId = existingResume.AccountId;

        // 1. Validate the request's internal consistency.
        ValidateExperienceSelections(request.Experiences);
        ValidateExistingSelections(existingResume, request.Experiences);

        // 2. Validate Library ownership and relationships.
        var selections = request.Experiences
            .Select(e => new ExperienceSelectionValidation(
                e.LibraryExperienceId,
                e.LibrarySummaryId,
                e.Bullets.Select(b => b.LibraryBulletId).ToArray()
            ))
            .ToArray();

        var valid = await libraryExperienceRepository.AreSelectionsValidAsync(
            accountId,
            selections,
            cancellationToken
        );

        if (!valid)
        {
            throw new InvalidOperationException("One or more Library selections are invalid.");
        }

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

    private static void ValidateExistingSelections(Resume resume, IReadOnlyCollection<ResumeExperienceRequest> requests)
    {
        var experiencesById = resume.Experiences
            .ToDictionary(e => e.Id);

        foreach (var request in requests)
        {
            if (request.Id is not int experienceId)
            {
                if (request.Bullets.Any(b => b.Id.HasValue))
                {
                    throw new InvalidOperationException(
                        "A new experience cannot reference existing resume bullet IDs.");
                }

                continue;
            }

            if (!experiencesById.TryGetValue(
                experienceId, out var experience))
            {
                throw new NotFoundException(
                    $"Resume experience {experienceId} was not found.");
            }

            if (experience.LibraryExperienceId != request.LibraryExperienceId)
            {
                throw new InvalidOperationException(
                    "An existing experience cannot change its LibraryExperienceId.");
            }

            var existingBulletIds = experience.Bullets
                .Select(b => b.Id)
                .ToHashSet();

            foreach (var bullet in request.Bullets)
            {
                if (bullet.Id is int bulletId &&
                    !existingBulletIds.Contains(bulletId))
                {
                    throw new NotFoundException(
                        $"Resume bullet {bulletId} does not belong to experience {experienceId}.");
                }
            }
        }
    }

    private static void ValidateExperienceSelections(IReadOnlyCollection<ResumeExperienceRequest> experiences)
    {
        var experienceIds = new HashSet<int>();
        var libraryExperienceIds = new HashSet<int>();
        var resumeBulletIds = new HashSet<int>();

        foreach (var experience in experiences)
        {
            if (experience.Id is int experienceId &&
                !experienceIds.Add(experienceId))
            {
                throw new InvalidOperationException(
                    $"Duplicate resume experience ID {experienceId}.");
            }

            if (!libraryExperienceIds.Add(experience.LibraryExperienceId))
            {
                throw new InvalidOperationException(
                    $"Duplicate Library experience ID {experience.LibraryExperienceId}.");
            }

            var libraryBulletIds = new HashSet<int>();

            foreach (var bullet in experience.Bullets)
            {
                if (bullet.Id is int bulletId && !resumeBulletIds.Add(bulletId))
                {
                    throw new InvalidOperationException(
                        $"Duplicate resume bullet ID {bulletId}.");
                }

                if (!libraryBulletIds.Add(bullet.LibraryBulletId))
                {
                    throw new InvalidOperationException(
                        $"Duplicate Library bullet ID {bullet.LibraryBulletId} " +
                        $"in experience {experience.LibraryExperienceId}.");
                }
            }
        }
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
                SalaryCurrency: resume.JobPosting.SalaryCurrency
            ),

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
