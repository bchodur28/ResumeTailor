using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Application.Accounts.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Account?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task<Account?> GetAccountForUpdatingAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAccoutAsync(Account account, CancellationToken cancellationToken = default);

    Task<PersonalLink?> GetPersonalLinkForUpdatingAsync(int id, CancellationToken cancellationToken = default);
    Task<Title?> GetTitleForUpdatingAsync(int id, CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
