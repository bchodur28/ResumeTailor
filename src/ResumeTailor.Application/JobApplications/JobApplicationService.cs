using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.JobApplications.Interfaces;
using ResumeTailor.Application.JobApplications.Models;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Application.JobApplications;

public class JobApplicationService(
    IJobApplicationRepository jobApplicationRepository,
    IResumeDataProvider generatedResumeDataProvider) : IJobApplicationService
{
    public async Task<IReadOnlyCollection<JobApplicationListItemResponse>> GetJobApplicationListItemsByAccountIdAsync(int accountId, CancellationToken cancellationToken)
    {
        var applicatons = await jobApplicationRepository.GetJobApplicationsByAccountIdAsync(accountId, cancellationToken);

        return applicatons.Select(MapToListItemResponse).ToList();
    }

    public async Task<JobApplicationResponse> GetJobApplicationIdAsync(int id, CancellationToken cancellationToken)
    {
        var jobApplication = await jobApplicationRepository.GetJobApplicationByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Job application with ID {id} was not found.");

        var resume = jobApplication.GeneratedResume
            ?? throw new NotFoundException($"Job applicaton with ID {id} does not ahve a generated resume.");

        var resumeSourceData = await generatedResumeDataProvider.GetResumeSourceDataForExistingResumeAsync(resume, cancellationToken);

        return MapToJobApplicationResponse(jobApplication, resume, resumeSourceData);
    }

    public async Task CreateJobApplicationAsync(JobApplicationRequest request, CancellationToken cancellationToken)
    {
        await jobApplicationRepository.CreateJobApplicationAsync(MapRequestToDomain(request));
    }

    public async Task UpdateJobApplicationAsync(int id, JobApplicationRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteJobApplicationAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private static JobApplication MapRequestToDomain(JobApplicationRequest request)
    {
        return new JobApplication(
            request.AccountId,
            request.CompanyName,
            request.JobName,
            request.Location,
            request.WorkStyle,
            request.JobDescription,
            request.JobUrl,
            request.AppliedDate,
            request.Status);
    }

    private static JobApplicationListItemResponse MapToListItemResponse(JobApplication jobApplication)
    {
        return new JobApplicationListItemResponse(
            jobApplication.Id,
            jobApplication.AccountId,
            jobApplication.CompanyName,
            jobApplication.JobName,
            jobApplication.Location,
            jobApplication.WorkStyle,
            jobApplication.AppliedDate,
            jobApplication.Status);
    }

    private static JobApplicationResponse MapToJobApplicationResponse(JobApplication jobApplicaton, GeneratedResume resume, ResumeSourceData data)
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
                            b.Value,
                            b.AlternativeValue))
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
            new ResumeResponse(
                resume.Id,
                data.Account.Id,
                data.Account.DisplayName,
                data.Account.Titles
                    .FirstOrDefault(t => t.IsPrimary)?.Value ?? string.Empty,
                data.Account.Email,
                data.Account.PhoneNumber,
                $"{data.Account.City}, {data.Account.State}",
                data.Account.PersonalLinks
                    .Select(link => new PersonalLinkResponse(
                        link.Id,
                        link.DisplayName,
                        link.Url))
                    .ToList(),
                selectedCompanies,
                selectedEducations,
                selectedProjects),

            new ResumeSummaryResponse(
                0,
                resume.AiAnalysis?.Summary ?? string.Empty,
                resume.AiAnalysis?.Strengths
                    .Select(x => new ResumeAiInsightResult(
                        x.Type,
                        x.Value))
                    .ToList() ?? [],
                resume.AiAnalysis?.Weaknesses
                    .Select(x => new ResumeAiInsightResult(
                        x.Type,
                        x.Value))
                    .ToList() ?? []),

            new AiUsage(
                resume.AiMetaData?.InputTokens ?? 0,
                resume.AiMetaData?.OutputTokens ?? 0,
                resume.AiMetaData?.TotalTokens ?? 0,
                0)
            );

        return new JobApplicationResponse(
            jobApplicaton.Id,
            jobApplicaton.AccountId,
            resumeDetails,
            jobApplicaton.CompanyName,
            jobApplicaton.JobName,
            jobApplicaton.Location,
            jobApplicaton.WorkStyle,
            jobApplicaton.JobDescription,
            jobApplicaton.JobUrl,
            jobApplicaton.AppliedDate,
            jobApplicaton.Status);
    }
}
