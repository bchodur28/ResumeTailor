using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;


namespace ResumeTailor.Application.GeneratedResumes.Management;

internal sealed class ResumeManagementService(
    IResumeRepository repository,
    IResumeDataProvider resumeDataProvider) : IResumeManagementService
{
    // Save Generated Resume
    public async Task<int> SaveGeneratedResumeAsync(ResumeRequest request, CancellationToken cancellationToken = default)
    {
        var generatedResume = new GeneratedResume(
            request.AccountId,
            request.Name,
            request.JobApplicationId
        );

        foreach(var companyRequest in request.Companies)
        {
            var companySelection = generatedResume.AddCompanySelection(companyRequest.CompanyId, companyRequest.SortOrder);

            foreach(var bulletRequest in companyRequest.Bullets)
            {
                companySelection.AddResumeBullet(
                    bulletRequest.SourceBulletId,
                    bulletRequest.Value,
                    bulletRequest.AlternativeValue,
                    bulletRequest.SortOrder);
            }
        }

        foreach(var educationRequest in request.Education)
        {
            generatedResume.AddEducationSelection(
                educationRequest.EducationId,
                educationRequest.SortOrder);
        }

        foreach(var projectRequest in request.Projects)
        {
            generatedResume.AddProjectSelection(
                projectRequest.ProjectId,
                projectRequest.SortOrder);
        }

        var aiAnalysis = new ResumeAiAnalysis(
            request.AiAnalysis.Summary,
            request.AiAnalysis.Score);

        foreach(var insightRequest in request.AiAnalysis.Strengths)
        {
            aiAnalysis.AddInsight(ResumeAiInsightType.Strength, insightRequest.Value);
        }

        foreach (var insightRequest in request.AiAnalysis.Weaknesses)
        {
            aiAnalysis.AddInsight(ResumeAiInsightType.Weakness, insightRequest.Value);
        }

        generatedResume.SetAiAnalysis(aiAnalysis);

        generatedResume.SetAiMetaData(
            new ResumeAiMetaData(
                request.AiMetaData.InputTokens,
                request.AiMetaData.OutputTokens,
                request.AiMetaData.TotalTokens));

        await repository.CreateResumeAsync(generatedResume, cancellationToken);
        await repository.SaveAsync(cancellationToken);

        return generatedResume.Id;
    }


    // Generated Resume Management
    public async Task<ResumeDetailsResponse> GetResumeDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        var resume = await repository.GetResumeAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {id} was not found.");
        var sourceData = await resumeDataProvider.GetResumeSourceDataForExistingResumeAsync(resume, cancellationToken);

        return MapToGeneratedResumeDetailsResponse(sourceData, resume);
    }

    public async Task<IReadOnlyCollection<ResumeListItemResponse>> GetGeneratedResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var generatedResumes = await repository.GetResumesByAccountIdAsync(accountId, cancellationToken);
        return generatedResumes.Select(MapGeneratedResumeDomainToListItemResponse).ToList();
    }

    public async Task UpdateGeneratedResumeAsync(int id, GeneratedResumeRequest request, CancellationToken cancellationToken = default)
    {
        var existingResume = await repository.GetResumeForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {id} was not found while updating.");
        
        existingResume.Update(request.Name, request.JobApplicatonId);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteGeneratedResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        var generatedResume = await repository.GetResumeForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {id} was not found while deleting.");

        repository.DeleteResumeAsync(generatedResume);
        await repository.SaveAsync(cancellationToken);
    }


    // Company Selection Management
    public async Task CreateCompanySelectionsAsync(int generatedResumeId, IEnumerable<ResumeCompanySelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        var generatedResume = await repository.GetResumeForUpdatingAsync(generatedResumeId, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {generatedResumeId} was not found while creating company selections.");


        foreach (var request in requests)
        {
            var companySelection = generatedResume.AddCompanySelection(request.CompanyId, request.SortOrder);
            foreach (var bulletRequest in request.Bullets)
            {
                companySelection.AddResumeBullet(
                    bulletRequest.SourceBulletId,
                    bulletRequest.Value,
                    bulletRequest.AlternativeValue,
                    bulletRequest.SortOrder);
            }
        }

        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateCompanySelectionAsync(int id, ResumeCompanySelectionRequest request, CancellationToken cancellationToken = default)
    {
        var existingSelection = await repository.GetCompanySelectionForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Company selection with ID {id} was not found while updating.");
        
        existingSelection.Update(request.CompanyId, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteCompanySelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        var companySelection = await repository.GetCompanySelectionForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Company selection with ID {id} was not found while deleting.");

        repository.DeleteCompanySelection(companySelection);
        await repository.SaveAsync(cancellationToken);
    }


    // Education Selection Management
    public async Task CreateEducationSelectionsAsync(int generatedResumeId, IEnumerable<ResumeEducationSelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        var generatedResume = await repository.GetResumeForUpdatingAsync(generatedResumeId, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {generatedResumeId} was not found while creating education selections.");

        foreach(var request in requests)
        {
            generatedResume.AddEducationSelection(request.EducationId, request.SortOrder);
        }

        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateEducationSelectionAsync(int id, ResumeEducationSelectionRequest request, CancellationToken cancellationToken = default)
    {
        var existingSelection = await repository.GetEducationSelectionForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Education selection with ID {id} was not found when updating.");
        
        existingSelection.Update(request.EducationId, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteEducationSelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        var educationSelection = await repository.GetEducationSelectionForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Education selection with ID {id} was not found when deleting.");

        repository.DeleteEducationSelection(educationSelection);

        await repository.SaveAsync(cancellationToken);
    }

    // Project Selection Management
    public async Task CreateProjectSelectionsAsync(int generatedResumeId, IEnumerable<ResumeProjectSelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        var generatedResume = await repository.GetResumeForUpdatingAsync(generatedResumeId, cancellationToken)
            ?? throw new NotFoundException($"Generated resume with ID {generatedResumeId} was not found when creating project selections.");

        foreach(var request in requests)
        {
            generatedResume.AddProjectSelection(request.ProjectId, request.SortOrder);
        }

        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateProjectSelectionAsync(int id, ResumeProjectSelectionRequest request, CancellationToken cancellationToken = default)
    {
        var existingSelection = await repository.GetProjectSelectionForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Company selection with ID {id} was not found when updating.");
        
        existingSelection.Update(request.ProjectId, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteProjectSelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        var projectSelection = await repository.GetProjectSelectionForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Company selection with ID {id} was not found while deleting.");

        repository.DeleteProjectSelection(projectSelection);

        await repository.SaveAsync(cancellationToken);
    }

    // Resume Bullets
    public async Task CreateResumeBulletsAsycn(int resumeCompanyId, IEnumerable<ResumeBulletRequest> requests, CancellationToken cancellationToken = default)
    {
        var companySelection = await repository.GetCompanySelectionForUpdating(resumeCompanyId, cancellationToken)
            ?? throw new NotFoundException($"Company selection with ID {resumeCompanyId} was not found when creating resume bullets.");

        foreach (var request in requests)
        {
            companySelection.AddResumeBullet(
                request.SourceBulletId,
                request.Value,
                request.AlternativeValue,
                request.SortOrder);
        }

        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateResumeBulletAsync(int id, ResumeBulletRequest request, CancellationToken cancellationToken = default)
    {
        var existingBullet = await repository.GetResumeBulletForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Resume bullet with ID {id} was not found when updating.");
        
        existingBullet.Update(request.Value, request.AlternativeValue, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteResumeBulletAsync(int id, CancellationToken cancellationToken = default)
    {
        var resumeBullet = await repository.GetResumeBulletForUpdating(id, cancellationToken)
            ?? throw new NotFoundException($"Resume bullet with ID {id} was not found when deleting.");
        
        repository.DeleteResumeBullet(resumeBullet);
        await repository.SaveAsync(cancellationToken);
    }


    // Mapping Methods
    private static ResumeListItemResponse MapGeneratedResumeDomainToListItemResponse(GeneratedResume generatedResume)
    {
        return new ResumeListItemResponse(
            generatedResume.Id,
            generatedResume.AccountId,
            generatedResume.Name
            );
    }

    private static ResumeDetailsResponse MapToGeneratedResumeDetailsResponse(ResumeSourceData data, GeneratedResume resume)
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
            new ResumeResponse(
                resume.Id,
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
                0));

        return resumeDetails;
    }
}
