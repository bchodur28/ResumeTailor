using ResumeTailor.Application.Profile.Experience.Models;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Profile.Experience.Interfaces;

public interface IExperienceRepository
{
    Task<IReadOnlyCollection<Company>> GetCompaniesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Company>> GetCompaniesWithNonDeletedBulletsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CompanyWithBulletCount>> GetCompaniesWithCountByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Company>> GetCompaniesForUpdatingByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Company>> GetCompaniesWithBulletsIncludingDeletedByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Company>> GetCompaniesWithBulletsByIds(HashSet<int> companyIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<int>> GetCompanyIdsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    void AddCompanies(IEnumerable<Company> companies);
    void RemoveCompanies(IEnumerable<Company> companies);

    Task<IReadOnlyCollection<Project>> GetProjectsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Project>> GetProjectsForUpdatingByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Project>> GetProjectsByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<int>> GetProjectIdsForCreationAsync(int accountId, CancellationToken cancellationToken = default);
    void AddProjects(IEnumerable<Project> projects);
    void RemoveProjects(IEnumerable<Project> projects);

    Task<IReadOnlyCollection<CompanyBullets>> GetCompanyBulletsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Bullet>> GetBulletsForUpdatingAsync(int accountId, HashSet<int> bulletIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Bullet>> GetBulletsNotReferencedByResumeAsync(HashSet<int> bulletIds, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CompanyAiContext>> GetCompaniesForAiGenerationAsync(int accountId, CancellationToken cancellationToken = default);
    Task<Dictionary<int, string>> GetBulletIdMappingAsync(int accountId, CancellationToken cancellationToken = default);
    void AddBullets(IEnumerable<Bullet> bullets);
    void RemoveBullets(IEnumerable<Bullet> bullets);

    Task SaveAsync(CancellationToken cancellationToken = default);

}
