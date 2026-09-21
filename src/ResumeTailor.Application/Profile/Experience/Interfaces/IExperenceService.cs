using ResumeTailor.Application.Profile.Experience.Models;

namespace ResumeTailor.Application.Profile.Experience.Interfaces;

public interface IExperenceService
{
    Task<IReadOnlyCollection<CompanyResponse>> GetCompaniesAsync(string Auth0UserId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CompanyWithBulletsResponse>> GetCompaniesWithBulletsAsync(string Auth0UserId, CancellationToken cancellationToken = default);
    Task CreateCompaniesAsync(string Auth0UserId, IReadOnlyCollection<CompanyRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateCompaniesAsync(string Auth0UserId, IReadOnlyCollection<CompanyRequest> requests, CancellationToken cancellationToken = default);
    Task DeleteCompaniesAsync(string Auth0UserId, IReadOnlyCollection<int> companyIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ProjectResponse>> GetProjectsAsync(string Auth0UserId, CancellationToken cancellationToken = default);
    Task CreateProjectsAsync(string Auth0UserId, IReadOnlyCollection<ProjectRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateProjectsAsync(string Auth0UserId, IReadOnlyCollection<ProjectRequest> requests, CancellationToken cancellationToken = default);
    Task DeleteProjectsAsync(string Auth0UserId, IReadOnlyCollection<int> projectIds, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<CompanyBulletsResponse>> GetCompanyBulletsAsync(string Auth0UserId, CancellationToken cancellationToken = default);
    Task CreateBulletsAsync(string Auth0UserId, IReadOnlyCollection<BulletRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateBulletsAsync(string Auth0UserId, IReadOnlyCollection<BulletRequest> requests, CancellationToken cancellationToken = default);
    Task DeleteBulletsAsync(string Auth0UserId, IReadOnlyCollection<BulletDeleteRequest> requests, CancellationToken cancellationToken = default);
}
