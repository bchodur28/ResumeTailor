using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Domain.Resumes.ApplicationTracking;


namespace ResumeTailor.Application.GeneratedResumes.Management;

internal sealed class ResumeManagementService(
    IResumeRepository repository,
    IResumeDataProvider resumeDataProvider) : IResumeManagementService
{
    public async Task<ResumeDetailsResponse> GetResumeDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var resume = await repository.GetResumeAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {id} was not found.");
        var sourceData = await resumeDataProvider.GetResumeSourceDataForExistingResumeAsync(resume, cancellationToken);

        return MapToGeneratedResumeDetailsResponse(sourceData, resume);
    }

    public async Task<IReadOnlyCollection<ResumeListItemResponse>> GetResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var generatedResumes = await repository.GetResumesByAccountIdAsync(accountId, cancellationToken);
        return generatedResumes.Select(MapGeneratedResumeDomainToListItemResponse).ToList();
    }

    public async Task UpdateResumeAsync(int id, UpdateResumeRequest request, CancellationToken cancellationToken = default)
    {
        var existingResume = await repository.GetResumeForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {id} was not found while updating.");
        
        existingResume.Update(request.Name);
        UpdateCompanySelections(existingResume, request.Companies);
        UpdateEducationSelections(existingResume, request.Education);
        UpdateProjectSelections(existingResume, request.Projects);

        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteResumeDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var generatedResume = await repository.GetResumeForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {id} was not found while deleting.");

        repository.DeleteResumeAsync(generatedResume);
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

    public async Task UpdateResumeApplicationTrackingAsync(int resumeId, ResumeApplicationTrackingRequest request, CancellationToken cancellationToken = default)
    {
        var existingApplicationTracking = await repository.GetResumeApplicationTrackingForUpdatingAsync(resumeId, cancellationToken)
            ?? throw new NotFoundException($"Resume application tracking with resume ID {resumeId} was not found while updating application tracking.");

        existingApplicationTracking.Update(
            request.Status,
            request.Applied,
            request.Interviewed,
            request.OfferReceived,
            request.OfferAccepted,
            request.Rejected);

        await repository.SaveAsync(cancellationToken);
    }

    private static void UpdateCompanySelections(Resume resume, IReadOnlyCollection<ResumeCompanySelectionRequest> companySelectionRequests)
    {

        var requestedCompanyIds = companySelectionRequests
            .Select(r => r.CompanyId)
            .ToHashSet();

        var companySelectionsToDelete = resume.CompanySelections
            .Where(s => !requestedCompanyIds.Contains(s.CompanyId))
            .ToList();

        foreach (var selection in companySelectionsToDelete)
        {
            resume.MarkAiScoreStale(AiScoreStaleness.High);
            resume.RemoveCompanySelection(selection);
        }

        foreach (var companyRequest in companySelectionRequests)
        {
            if (companyRequest.Id.HasValue)
            {
                var existingCompanySelection = resume.CompanySelections.FirstOrDefault(s => s.Id == companyRequest.Id.Value)
                    ?? throw new NotFoundException($"Company selection with ID {companyRequest.Id.Value} was not found while updating.");

                if (existingCompanySelection.CompanyId != companyRequest.CompanyId)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.High);
                }
                else if (existingCompanySelection.SortOrder != companyRequest.SortOrder)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.Medium);
                }

                existingCompanySelection.Update(companyRequest.CompanyId, companyRequest.SortOrder);

                var requestedBulletIds = companyRequest.Bullets
                    .Where(b => b.Id.HasValue)
                    .Select(b => b.Id!.Value)
                    .ToHashSet();

                var bulletsToDelete = existingCompanySelection.Bullets
                    .Where(b => !requestedBulletIds.Contains(b.Id))
                    .ToList();

                foreach(var bullet in bulletsToDelete)
                {
                    resume.MarkAiScoreStale(AiScoreStaleness.High);
                    existingCompanySelection.RemoveResumeBullet(bullet);
                }

                foreach (var bulletRequest in companyRequest.Bullets)
                {
                    if (bulletRequest.Id.HasValue)
                    {
                        var existingBullet = existingCompanySelection.Bullets.FirstOrDefault(b => b.Id == bulletRequest.Id.Value)
                            ?? throw new NotFoundException($"Bullet with ID {bulletRequest.Id} was not found while updating.");

                        var staleness = GetBulletStaleness(existingBullet, bulletRequest);
                        resume.MarkAiScoreStale(staleness);

                        existingBullet.Update(bulletRequest.SourceBulletId, bulletRequest.Value, bulletRequest.AlternativeValue, bulletRequest.SortOrder);
                    }
                    else
                    {
                        resume.MarkAiScoreStale(AiScoreStaleness.High);

                        existingCompanySelection.AddResumeBullet(
                            bulletRequest.SourceBulletId,
                            bulletRequest.Value,
                            bulletRequest.AlternativeValue,
                            bulletRequest.SortOrder);
                    }

                }
            }
            else
            {
                resume.MarkAiScoreStale(AiScoreStaleness.High);

                var newCompanySelection = resume.AddCompanySelection(companyRequest.CompanyId, companyRequest.SortOrder);

                foreach (var bulletRequest in companyRequest.Bullets)
                {
                    newCompanySelection.AddResumeBullet(
                        bulletRequest.SourceBulletId,
                        bulletRequest.Value,
                        bulletRequest.AlternativeValue,
                        bulletRequest.SortOrder);
                }
            }
        }
    }

    private static void UpdateEducationSelections(Resume resume, IReadOnlyCollection<ResumeEducationSelectionRequest> educationSelectionRequests)
    {
        var requestedEducationIds = educationSelectionRequests
            .Select(r => r.EducationId)
            .ToHashSet();

        var educationSelectionsToDelete = resume.EducationSelections
            .Where(s => !requestedEducationIds.Contains(s.EducationId))
            .ToList();

        foreach (var selection in educationSelectionsToDelete)
        {
            resume.RemoveEducationSelection(selection);
        }

        foreach (var educationRequest in educationSelectionRequests)
        {
            if (educationRequest.Id.HasValue)
            {
                var existingEducationSelection = resume.EducationSelections.FirstOrDefault(s => s.Id == educationRequest.Id)
                    ?? throw new NotFoundException($"Education selection with ID {educationRequest.EducationId} was not found while updating.");

                existingEducationSelection.Update(educationRequest.EducationId, educationRequest.SortOrder);
            }else
            {
                resume.AddEducationSelection(educationRequest.EducationId, educationRequest.SortOrder);
            }
        }
    }

    private static void UpdateProjectSelections(Resume resume, IReadOnlyCollection<ResumeProjectSelectionRequest> projectSelectionRequests)
    {
        var requestedProjectIds = projectSelectionRequests
            .Select(r => r.ProjectId)
            .ToHashSet();

        var projectSelectionsToDelete = resume.ProjectSelections
            .Where(s => !requestedProjectIds.Contains(s.ProjectId))
            .ToList();

        foreach (var selection in projectSelectionsToDelete)
        {
            resume.RemoveProjectSelection(selection);
        }

        foreach (var projectRequest in projectSelectionRequests)
        {
            if (projectRequest.Id.HasValue)
            {
                var existingProjectSelection = resume.ProjectSelections.FirstOrDefault(s => s.Id == projectRequest.Id)
                    ?? throw new NotFoundException($"Project selection with ID {projectRequest.ProjectId} was not found while updating.");

                existingProjectSelection.Update(projectRequest.ProjectId, projectRequest.SortOrder);
            } else
            {
                resume.AddProjectSelection(projectRequest.ProjectId, projectRequest.SortOrder);
            }
        }
    }

    // Mapping Methods
    private static ResumeListItemResponse MapGeneratedResumeDomainToListItemResponse(Resume generatedResume)
    {
        return new ResumeListItemResponse(
            generatedResume.Id,
            generatedResume.AccountId,
            generatedResume.Name
            );
    }

    private static ResumeDetailsResponse MapToGeneratedResumeDetailsResponse(ResumeSourceData data, Resume resume)
    {
        var selectedCompanies = data.Companies
            .Select(company =>
            {
                var selection = resume.CompanySelections
                    .Single(x => x.CompanyId == company.Id);

                return new ResumeCompanyResult(
                    company.Id,
                    company.Name,
                    company.Title,
                    company.Location,
                    company.Started,
                    company.Ended,
                    selection.Bullets
                        .OrderBy(b => b.SortOrder)
                        .Select(b => new ResumeBulletResult(
                            SourceBulletId: b.SourceBulletId,
                            Value: b.Value,
                            AlternativeValue: b.AlternativeValue))
                        .ToList());
            })
            .ToList();

        var selectedEducations = data.Education
            .Select(education =>
            {
                return new EducationResponse(
                    education.Id,
                    education.SchoolName,
                    education.Degree,
                    education.Major,
                    education.Started,
                    education.Ended,
                    education.UseForResume);
            })
            .ToList();

        var selectedProjects = data.Projects
            .Select(project =>
            {
                return new ProjectResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.Started,
                    project.Ended,
                    project.TechStack,
                    project.Link,
                    project.UseForResume);
            })
            .ToList();

        var resumeDetails = new ResumeDetailsResponse(
            Resume: new ResumeResponse(
                Id: resume.Id,
                AccountId: data.Account.Id,
                ResumeName: resume.Name,
                AiScoreStaleness: resume.AiScoreStaleness,
                PersonName: data.Account.DisplayName,
                Profession: data.Account.Titles
                    .FirstOrDefault(t => t.IsPrimary)?.Value ?? string.Empty,
                Email: data.Account.Email,
                PhoneNumber: data.Account.PhoneNumber,
                Location: $"{data.Account.City}, {data.Account.State}, {data.Account.Country}",
                PersonalLinks: data.Account.PersonalLinks
                    .Select(link => new PersonalLinkResponse(
                        link.Id,
                        link.DisplayName,
                        link.Url))
                    .ToList(),
                Companies: selectedCompanies,
                Education: selectedEducations,
                Projects: selectedProjects),

            AiAnalysis: new ResumeAiAnalysisResponse(
                resume.AiAnalysis?.Score,
                resume.AiAnalysis?.Summary ?? string.Empty,
                resume.AiAnalysis?.Strengths
                    .Select(x => x.Value)
                    .ToList() ?? [],
                resume.AiAnalysis?.Weaknesses
                    .Select(x => x.Value)
                    .ToList() ?? []),

            JobPosting: new JobPostingResult(
                Id: resume.JobPosting?.Id ?? -1,
                CompanyName: resume.JobPosting?.CompanyName,
                JobTitle: resume.JobPosting?.JobTitle,
                Location: resume.JobPosting?.Location,
                WorkStyle: resume.JobPosting?.WorkStyle,
                SalaryMin: resume.JobPosting?.SalaryMin,
                SalaryMax: resume.JobPosting?.SalaryMax,
                Salary: resume.JobPosting?.Salary,
                SalaryPeriod: resume.JobPosting?.SalaryPeriod,
                SalaryCurrency: resume.JobPosting?.SalaryCurrency),

            ApplicationTracking: new ApplicationTrackingResponse(
                Id: resume.ApplicationTracking?.Id ?? -1,
                Status: resume.ApplicationTracking?.Status ?? ApplicationStatus.Interested,
                Applied: resume.ApplicationTracking?.Applied,
                Interviewed: resume.ApplicationTracking?.Interviewed,
                OfferReceived: resume.ApplicationTracking?.OfferReceived,
                OfferAccepted: resume.ApplicationTracking?.OfferAccepted,
                Rejected: resume.ApplicationTracking?.Rejected),

            AiMetaData: new AiMetaDataResult(
                resume.AiMetaData?.Model ?? string.Empty,
                resume.AiMetaData?.InputTokens ?? 0,
                resume.AiMetaData?.OutputTokens ?? 0,
                resume.AiMetaData?.TotalTokens ?? 0,
                resume.AiMetaData?.Cost ?? 0));

        return resumeDetails;
    }

    private static AiScoreStaleness GetBulletStaleness(ResumeCompanyBullet existing, ResumeBulletRequest request)
    {
        var wasAlternativeSwap =
            existing.Value == request.AlternativeValue &&
            existing.AlternativeValue == request.Value;

        if (!wasAlternativeSwap &&
            (existing.Value != request.Value ||
             existing.AlternativeValue != request.AlternativeValue ||
             existing.SourceBulletId != request.SourceBulletId))
        {
            return AiScoreStaleness.High;
        }

        if (existing.SortOrder != request.SortOrder)
        {
            return AiScoreStaleness.Medium;
        }

        if (wasAlternativeSwap)
        {
            return AiScoreStaleness.Low;
        }

        return AiScoreStaleness.None;
    }
}
