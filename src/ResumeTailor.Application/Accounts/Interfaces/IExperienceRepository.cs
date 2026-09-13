using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Application.Accounts.Interfaces;

public interface IExperienceRepository
{
    Task<IReadOnlyCollection<Company>> GetCompaniesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Company>> GetCompaniesByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default);
    Task CreateCompanyAsync(Company company, CancellationToken cancellationToken = default);
    Task DeleteCompanyAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> CompanyExistsAsync(int id, CancellationToken cancellationToken = default);

    Task CreateBulletAsync(Bullet bullet, CancellationToken cancellationToken = default);
    Task DeleteBulletAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> BulletExistsAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Education>> GetEducationByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Education>> GetEducationByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Project>> GetProjectsByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Project>> GetProjectsByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default);
    Task CreateProjectAsync(Project project, CancellationToken cancellationToken = default);
    Task DeleteProjectAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ProjectExistAsync(int id, CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken = default);

}
