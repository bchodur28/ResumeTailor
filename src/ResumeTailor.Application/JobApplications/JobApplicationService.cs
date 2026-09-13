using ResumeTailor.Application.Accounts.Models;
using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.JobApplications.Interfaces;
using ResumeTailor.Application.JobApplications.Models;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Application.JobApplications;

public class JobApplicationService(
    IJobApplicationRepository jobApplicationRepository,
    IGeneratedResumeDataProvider generatedResumeDataProvider) : IJobApplicationService
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

        var generatedResume = jobApplication.GeneratedResume
            ?? throw new NotFoundException($"Job applicaton with ID {id} does not ahve a generated resume.");

        var generatedResumeSourceData = await generatedResumeDataProvider.GetAsync(generatedResume.Id, cancellationToken);

        return MapToJobApplicationResponse(jobApplication, generatedResumeSourceData);
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

    private static JobApplicationResponse MapToJobApplicationResponse(JobApplication jobApplicaton, ResumeSourceData data)
    {
        var selectedCompanies = data.Companies
            .Select(company =>
            {
                var selection = data.GeneratedResume.CompanySelections
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

        var selectedEducations = data.Educations
            .Select(education =>
            {
                return new ResumeEducationResponse(
                    education.Id,
                    education.SchoolName,
                    education.Degree,
                    education.Major,
                    education.Started,
                    education.Ended);
            })
            .ToList();

        var selectedProjects = data.Projects
            .Select(project =>
            {
                return new ResumeProjectResponse(
                    project.Id,
                    project.Name,
                    project.Description,
                    project.Started,
                    project.Ended,
                    project.TechStack,
                    project.Link);
            })
            .ToList();

        var resumeDetails = new GeneratedResumeDetailsResponse(
            new GeneratedResumeResponse(
                data.GeneratedResume.Id,
                data.Account.Id,
                data.Account.DisplayName,
                data.Account.Titles
                    .FirstOrDefault(t => t.IsPrimary)?.Value ?? string.Empty,
                data.Account.Email,
                data.Account.PhoneNumber,
                $"{data.Account.City}, {data.Account.State}, {data.Account.Country}",
                data.Account.PersonalLinks
                    .Select(link => new PersonalLinkResponse(
                        link.Id,
                        link.DisplayName,
                        link.Url))
                    .ToList(),
                selectedCompanies,
                selectedEducations,
                selectedProjects),

            new GeneratedSummaryResponse(
                data.GeneratedResume.AiAnalysis?.Summary ?? string.Empty,
                data.GeneratedResume.AiAnalysis?.Strengths
                    .Select(x => new ResumeAiInsightResult(
                        x.Type,
                        x.Value))
                    .ToList() ?? [],
                data.GeneratedResume.AiAnalysis?.Weaknesses
                    .Select(x => new ResumeAiInsightResult(
                        x.Type,
                        x.Value))
                    .ToList() ?? []),

            new AiUsage(
                data.GeneratedResume.AiMetaData?.InputTokens ?? 0,
                data.GeneratedResume.AiMetaData?.OutputTokens ?? 0,
                data.GeneratedResume.AiMetaData?.TotalTokens ?? 0));

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
