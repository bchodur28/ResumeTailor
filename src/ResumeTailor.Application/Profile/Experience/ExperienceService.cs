using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Bullets;
using ResumeTailor.Application.Contracts.Companies;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Profile.Experience.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Profile.Experience;

public class ExperienceService(IAccountRepository accountRepository, IExperienceRepository experienceRepository) : IExperenceService
{
    // Companies
    public async Task<IReadOnlyCollection<CompanyResponse>> GetCompaniesAsync(string Auth0UserId, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when fetching companies.");

        var companies = await experienceRepository.GetCompaniesWithCountByAccountIdAsync(accountId, cancellationToken);

        return companies.Select(MapCompanyToResponse).ToList();
    }

    public async Task CreateCompaniesAsync(string Auth0UserId, IReadOnlyCollection<CompanyRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when creating companies.");

        var companies = requests
            .Select(c => MapCompanyToDomain(c, accountId))
            .ToList();

        experienceRepository.AddCompanies(companies);
        await experienceRepository.SaveAsync(cancellationToken);
    }

    public async Task UpdateCompaniesAsync(string Auth0UserId, IReadOnlyCollection<CompanyRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when updating companies.");

        var existingCompanies = await experienceRepository.GetCompaniesForUpdatingByAccountIdAsync(accountId, cancellationToken);
        var companyById = existingCompanies.ToDictionary(c => c.Id);

        foreach (var request in requests)
        {
            if(!companyById.TryGetValue(request.Id ?? 0, out var company))
            {
                throw new NotFoundException($"Company with Id {request.Id} was not found when updating.");
            }

            company.Update(request.Name,
                request.Location,
                request.Title,
                request.Started,
                request.Ended,
                request.GenerateBullets,
                request.MaxGeneratedBulletCount
            );
        }

        await experienceRepository.SaveAsync(cancellationToken);
    }

    public async Task DeleteCompaniesAsync(string Auth0UserId, IReadOnlyCollection<int> companyIds, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when deleting companies.");

        var existingCompanies = await experienceRepository.GetCompaniesForUpdatingByAccountIdAsync(accountId, cancellationToken);
        var companyById = existingCompanies.ToDictionary(e => e.Id);
        var companiesToDelete = new List<Company>();

        foreach (var companyId in companyIds)
        {
            if (!companyById.TryGetValue(companyId, out var company))
            {
                throw new NotFoundException($"Company with Id {companyId} was not found when deleting.");
            }
            companiesToDelete.Add(company);
        }

        experienceRepository.RemoveCompanies(companiesToDelete);

        await experienceRepository.SaveAsync(cancellationToken);
    }

    // Projects
    public async Task<IReadOnlyCollection<ProjectResponse>> GetProjectsAsync(string Auth0UserId, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when fetching companies.");

        var projects = await experienceRepository.GetProjectsByAccountIdAsync(accountId, cancellationToken);

        return projects.Select(MapProjectToResponse).ToList();
    }

    public async Task CreateProjectsAsync(string Auth0UserId, IReadOnlyCollection<ProjectRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when creating projects.");

        var projects = requests
            .Select(p => MapProjectToDomain(p, accountId))
            .ToList();

        experienceRepository.AddProjects(projects);
        await experienceRepository.SaveAsync(cancellationToken);
    }

    public async Task UpdateProjectsAsync(string Auth0UserId, IReadOnlyCollection<ProjectRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when updating projects.");

        var existingProjects = await experienceRepository.GetProjectsForUpdatingByAccountIdAsync(accountId, cancellationToken);
        var projectById = existingProjects.ToDictionary(c => c.Id);

        foreach (var request in requests)
        {
            if (!projectById.TryGetValue(request.Id ?? 0, out var project))
            {
                throw new NotFoundException($"Project with Id {request.Id} was not found when updating.");
            }

            project.Update(
                name: request.Name,
                description: request.Description,
                useForResume: request.UseForResume,
                started: request.Started,
                ended: request.Ended,
                techStack: request.TechStack,
                link: request.Link
            );
        }

        await experienceRepository.SaveAsync(cancellationToken);
    }

    public async Task DeleteProjectsAsync(string Auth0UserId, IReadOnlyCollection<int> projectIds, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when deleting projects.");

        var existingProjects = await experienceRepository.GetProjectsForUpdatingByAccountIdAsync(accountId, cancellationToken);
        var projectById = existingProjects.ToDictionary(e => e.Id);
        var projectsToDelete = new List<Project>();

        foreach (var projectId in projectIds)
        {
            if (!projectById.TryGetValue(projectId, out var project))
            {
                throw new NotFoundException($"Project with Id {projectId} was not found when deleting.");
            }
            projectsToDelete.Add(project);
        }

        experienceRepository.RemoveProjects(projectsToDelete);

        await experienceRepository.SaveAsync(cancellationToken);
    }

    // Bullets
    public async Task<IReadOnlyCollection<CompanyBulletsResponse>> GetCompanyBulletsAsync(string Auth0UserId, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when fetching companies.");

        var companies = await experienceRepository.GetCompanyBulletsByAccountIdAsync(accountId, cancellationToken);

        return companies.Select(MapCompanyBulletToResponse).ToList();
    }

    public async Task CreateBulletsAsync(string Auth0UserId, IReadOnlyCollection<BulletRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when creating bullets.");

        var bullets = requests
            .Select(MapBulletToDomain)
            .ToList();

        experienceRepository.AddBullets(bullets);
        await experienceRepository.SaveAsync(cancellationToken);
    }

    public async Task UpdateBulletsAsync(string Auth0UserId, IReadOnlyCollection<BulletRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when updating bullets.");

        var companyIds = requests.Select(r => r.CompanyId).ToHashSet();
        var bulletIds = requests.Select(r => r.Id ?? 0).ToHashSet();

        var existingBullets = await experienceRepository.GetBulletsForUpdatingByCompanyIdsAsync(accountId, companyIds, bulletIds, cancellationToken);
        var bulletById = existingBullets.ToDictionary(c => c.Id);

        foreach (var request in requests)
        {
            if (!bulletById.TryGetValue(request.Id ?? 0, out var bullet))
            {
                throw new NotFoundException($"Bullet with Id {request.Id} was not found when updating.");
            }

            bullet.Update(request.Value, request.AiScore);
        }

        await experienceRepository.SaveAsync(cancellationToken);
    }

    public async Task DeleteBulletsAsync(string auth0UserId, IReadOnlyCollection<BulletDeleteRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository
            .GetAccountIdByAuth0UserAsync(auth0UserId, cancellationToken)
            ?? throw new NotFoundException(
                "Account was not found when deleting bullets.");

        var companyIds = requests
            .Select(r => r.CompanyId)
            .ToHashSet();

        var bulletIds = requests
            .Select(r => r.BulletId)
            .ToHashSet();

        var existingBullets =
            await experienceRepository.GetBulletsForUpdatingByCompanyIdsAsync(
                accountId,
                companyIds,
                bulletIds,
                cancellationToken);

        var bulletById = existingBullets.ToDictionary(b => b.Id);

        // Validate that every requested bullet actually belongs
        // to the requested company/account.
        foreach (var request in requests)
        {
            if (!bulletById.TryGetValue(request.BulletId, out var bullet) ||
                bullet.CompanyId != request.CompanyId)
            {
                throw new NotFoundException(
                    $"Bullet with Id {request.BulletId} was not found " +
                    $"for company with id {request.CompanyId} when deleting.");
            }
        }

        var unreferencedBullets =
            await experienceRepository.GetBulletsNotReferencedByResumeAsync(
                bulletIds,
                cancellationToken);

        var unreferencedBulletIds = unreferencedBullets
            .Select(b => b.Id)
            .ToHashSet();

        var bulletsToHardDelete = new List<Bullet>();

        foreach (var bullet in existingBullets)
        {
            if (unreferencedBulletIds.Contains(bullet.Id))
            {
                bulletsToHardDelete.Add(bullet);
            } else
            {
                bullet.Delete();
            }
        }

        experienceRepository.RemoveBullets(bulletsToHardDelete);

        await experienceRepository.SaveAsync(cancellationToken);
    }

    private static CompanyResponse MapCompanyToResponse(CompanyWithBulletCount company) => new CompanyResponse
    (
        Id: company.company.Id,
        Name: company.company.Name,
        Title: company.company.Title,
        Location: company.company.Location,
        Started: company.company.Started,
        Ended: company.company.Ended,
        GenerateBullets: company.company.GenerateBullets,
        MaxGeneratedBulletCount: company.company.MaxGeneratedBulletCount,
        BulletCount: company.BulletCount
    );

    private static CompanyBulletsResponse MapCompanyBulletToResponse(CompanyBullets companyBullet) => new CompanyBulletsResponse
    (
        CompanyId: companyBullet.CompanyId,
        CompanyName: companyBullet.CompanyName,
        Bullets: companyBullet.Bullets
            .Select(b => new BulletReponse(Id: b.Id, Value: b.Value, AiScore: b.AiScore, b.ResumeCount))
            .ToList()
    );

    private static Company MapCompanyToDomain(CompanyRequest request, int accountId) => new Company
    (
        accountId: accountId,
        name: request.Name,
        title: request.Title,
        location: request.Location,
        started: request.Started,
        ended: request.Ended,
        generateBullets: request.GenerateBullets,
        maxGeneratedBulletCount: request.MaxGeneratedBulletCount
    );

    private static ProjectResponse MapProjectToResponse(Project project) => new ProjectResponse
    (
        Id: project.Id,
        SelectionId: null,
        Name: project.Name,
        Description: project.Description,
        Started: project.Started,
        Ended: project.Ended,
        TechStack: project.TechStack,
        Link: project.Link,
        UseForResume: project.UseForResume
    );

    private static Project MapProjectToDomain(ProjectRequest request, int accountId) => new Project
    (
        accountId: accountId,
        name: request.Name,
        description: request.Description,
        started: request.Started,
        ended: request.Ended,
        techStack: request.TechStack,
        link: request.Link,
        useForResume: request.UseForResume
    );

    private static Bullet MapBulletToDomain(BulletRequest request) => new Bullet
    (
        companyId: request.CompanyId,
        value: request.Value,
        aiScore: request.AiScore
    );
}
