using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Profile.Accounts.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Account?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task<int?> GetAccountIdByAuth0UserAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task<Account?> GetAccountForUpdatingAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task CreateAccoutAsync(Account account, CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
