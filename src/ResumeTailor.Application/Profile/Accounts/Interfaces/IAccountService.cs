using ResumeTailor.Application.Profile.Accounts.Models;

namespace ResumeTailor.Application.Profile.Accounts.Interfaces;

public interface IAccountService
{
    Task<AccountResponse?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task<int> CreateAccountAsync(string auth0UserId, AccountRequest request, CancellationToken cancellationToken = default);
    Task UpdateAccountAsync(string auth0UserId, AccountRequest request, CancellationToken cancellationToken = default);
}
