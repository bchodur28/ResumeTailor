using ResumeTailor.Application.Accounts.Models;
using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.GeneratedResumes.Content;


namespace ResumeTailor.Application.GeneratedResumes.Management;

internal sealed class GeneratedResumeManagementService(
    IGeneratedResumeRepository repository,
    IGeneratedResumeDataProvider generatedResumeDataProvider) : IGeneratedResumeManagementService
{
    // Save Generated Resume
    public async Task<int> SaveGeneratedResumeAsync(SaveGeneratedResumeRequest request, CancellationToken cancellationToken = default)
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

        await repository.CreateGeneratedResumeAsync(generatedResume, cancellationToken);
        await repository.SaveAsync(cancellationToken);

        return generatedResume.Id;
    }


    // Generated Resume Management
    public async Task<GeneratedResumeDetailsResponse> GetGeneratedResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        var generatedResumeSourceData = await generatedResumeDataProvider.GetAsync(id, cancellationToken);
        return MapToGeneratedResumeDetailsResponse(generatedResumeSourceData);
    }

    public async Task<IReadOnlyCollection<GeneratedResumeListItemResponse>> GetGeneratedResumesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        var generatedResumes = await repository.GetGeneratedResumesByAccountIdAsync(accountId, cancellationToken);
        return generatedResumes.Select(MapGeneratedResumeDomainToListItemResponse).ToList();
    }

    public async Task UpdateGeneratedResumeAsync(int id, GeneratedResumeRequest request, CancellationToken cancellationToken = default)
    {
        var existingResume = await repository.GetGeneratedResumeForUpdatingAsync(id, cancellationToken);
        if (existingResume is null)
        {
            throw new InvalidOperationException($"Generated resume with ID {id} was not found while updating.");
        }

        existingResume.Update(request.Name, request.JobApplicatonId);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteGeneratedResumeAsync(int id, CancellationToken cancellationToken = default)
    {
        var generatedResume = await repository.GetGeneratedResumeForUpdatingAsync(id, cancellationToken);

        if (generatedResume is null)
        {
            throw new NotFoundException($"Generated resume with ID {id} was not found while deleting.");
        }

        repository.DeleteGeneratedResumeAsync(generatedResume);
        await repository.SaveAsync(cancellationToken);
    }


    // Company Selection Management
    public async Task CreateCompanySelectionsAsync(int generatedResumeId, IEnumerable<ResumeCompanySelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        var generatedResumeExists = await repository.GeneratedResumeExistsAsync(generatedResumeId, cancellationToken);

        if (!generatedResumeExists)
        {
            throw new NotFoundException($"Generated resume with ID {generatedResumeId} was not found while creating company selection.");
        }

        var companySelections = requests
            .Select(MapCompanySelectionRequestToDomain)
            .ToList();

        await repository.CreateCompanySelectionsAsync(companySelections, cancellationToken);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateCompanySelectionAsync(int id, ResumeCompanySelectionRequest request, CancellationToken cancellationToken = default)
    {
        var existingSelection = await repository.GetCompanySelectionForUpdating(id, cancellationToken);
        if (existingSelection is null)
        {
            throw new InvalidOperationException($"Company selection with ID {id} was not found while updating.");
        }
        existingSelection.Update(request.CompanyId, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteCompanySelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        var companySelection = await repository.GetCompanySelectionForUpdating(id, cancellationToken);

        if(companySelection is null)
        {
            throw new NotFoundException($"Company selection with ID {id} was not found while deleting.");
        }

        repository.DeleteCompanySelection(companySelection);
        await repository.SaveAsync(cancellationToken);
    }


    // Education Selection Management
    public async Task CreateEducationSelectionsAsync(int generatedResumeId, IEnumerable<ResumeEducationSelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        var generatedResumeExists = await repository.GeneratedResumeExistsAsync(generatedResumeId, cancellationToken);

        if (!generatedResumeExists)
        {
            throw new NotFoundException($"Generated resume with ID {generatedResumeId} was not found when creating education selections.");
        }

        var educationSelections = requests
            .Select(MapEducationSelectionRequestToDomain)
            .ToList();

        await repository.CreateEducationSelectionsAsync(educationSelections, cancellationToken);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateEducationSelectionAsync(int id, ResumeEducationSelectionRequest request, CancellationToken cancellationToken = default)
    {
        var existingSelection = await repository.GetEducationSelectionForUpdating(id, cancellationToken);
        if (existingSelection is null)
        {
            throw new InvalidOperationException($"Company selection with ID {id} was not found when updating.");
        }
        existingSelection.Update(request.EducationId, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteEducationSelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        var educationSelection = await repository.GetEducationSelectionForUpdating(id, cancellationToken);

        if (educationSelection is null)
        {
            throw new NotFoundException($"Company selection with ID {id} was not found when deleting.");
        }

        repository.DeleteEducationSelection(educationSelection);

        await repository.SaveAsync(cancellationToken);
    }

    // Project Selection Management
    public async Task CreateProjectSelectionsAsync(int generatedResumeId, IEnumerable<ResumeProjectSelectionRequest> requests, CancellationToken cancellationToken = default)
    {
        var generatedResumeExists = await repository.GeneratedResumeExistsAsync(generatedResumeId, cancellationToken);

        if (!generatedResumeExists)
        {
            throw new NotFoundException($"Generated resume with ID {generatedResumeId} was not found when creating project selections.");
        }

        var projectSelections = requests
            .Select(MapProjectSelectionRequestToDomain)
            .ToList();

        await repository.CreateProjectSelectionsAsync(projectSelections, cancellationToken);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateProjectSelectionAsync(int id, ResumeProjectSelectionRequest request, CancellationToken cancellationToken = default)
    {
        var existingSelection = await repository.GetProjectSelectionForUpdating(id, cancellationToken);
        if (existingSelection is null)
        {
            throw new InvalidOperationException($"Company selection with ID {id} was not found when updating.");
        }
        existingSelection.Update(request.ProjectId, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteProjectSelectionAsync(int id, CancellationToken cancellationToken = default)
    {
        var projectSelection = await repository.GetProjectSelectionForUpdating(id, cancellationToken);

        if (projectSelection is null)
        {
            throw new NotFoundException($"Company selection with ID {id} was not found while deleting.");
        }

        repository.DeleteProjectSelection(projectSelection);

        await repository.SaveAsync(cancellationToken);
    }

    // Resume Bullets
    public async Task CreateResumeBulletsAsycn(int resumeCompanyId, IEnumerable<ResumeBulletRequest> requests, CancellationToken cancellationToken = default)
    {
        var companySelectionExists = await repository.CompanySelectionExistsAsync(resumeCompanyId, cancellationToken);

        if (!companySelectionExists)
        {
            throw new NotFoundException($"Company selection with ID {resumeCompanyId} was not found when creating resume bullets.");
        }

        var resumeBullets = requests
            .Select(MapResumeBulletRequestToDomain)
            .ToList();

        await repository.CreateResumeBulletsAysnc(resumeBullets, cancellationToken);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task UpdateResumeBulletAsync(int id, ResumeBulletRequest request, CancellationToken cancellationToken = default)
    {
        var existingBullet = await repository.GetResumeBulletForUpdating(id, cancellationToken);
        if (existingBullet is null)
        {
            throw new InvalidOperationException($"Resume bullet with ID {id} was not found when updating.");
        }
        existingBullet.Update(request.Value, request.AlternativeValue, request.SortOrder);
        await repository.SaveAsync(cancellationToken);
    }

    public async Task DeleteResumeBulletAsync(int id, CancellationToken cancellationToken = default)
    {
        var resumeBullet = await repository.GetResumeBulletForUpdating(id, cancellationToken);
        if (resumeBullet is null)
        {
            throw new NotFoundException($"Resume bullet with ID {id} was not found when deleting.");
        }
        repository.DeleteResumeBullet(resumeBullet);
        await repository.SaveAsync(cancellationToken);
    }


    // Mapping Methods
    private static GeneratedResumeListItemResponse MapGeneratedResumeDomainToListItemResponse(GeneratedResume generatedResume)
    {
        return new GeneratedResumeListItemResponse(
            generatedResume.Id,
            generatedResume.AccountId,
            generatedResume.Name
            );
    }

    private static ResumeCompanySelection MapCompanySelectionRequestToDomain(ResumeCompanySelectionRequest request)
    {
        return new ResumeCompanySelection(
            request.CompanyId,
            request.SortOrder
            );
    }

    private static ResumeEducationSelection MapEducationSelectionRequestToDomain(ResumeEducationSelectionRequest request)
    {
        return new ResumeEducationSelection(
            request.EducationId,
            request.SortOrder
            );
    }

    private static ResumeProjectSelection MapProjectSelectionRequestToDomain(ResumeProjectSelectionRequest request)
    {
        return new ResumeProjectSelection(
            request.ProjectId,
            request.SortOrder
            );
    }

    private static ResumeBullet MapResumeBulletRequestToDomain(ResumeBulletRequest request)
    {
        return new ResumeBullet(
            request.SourceBulletId,
            request.Value,
            request.AlternativeValue,
            request.SortOrder
            );
    }

    private static GeneratedResumeDetailsResponse MapToGeneratedResumeDetailsResponse(ResumeSourceData data)
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

        return resumeDetails;
    }
}
