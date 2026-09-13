using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Application.Accounts.Interfaces;

public interface IEducationRepository
{
    Task<IReadOnlyCollection<Education>> GetEducationByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task CreateEducationAsync(Education education, CancellationToken cancellationToken = default);
    Task UpdateEducationAsync(Education education, CancellationToken cancellationToken = default);
    Task<bool> ExistsEducationAsync(int id, CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
