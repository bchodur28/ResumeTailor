using ResumeTailor.Application.Accounts.Models;

namespace ResumeTailor.Application.Accounts.Interfaces;

public interface IAccountService
{
    Task<AccountResponse> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task CreateAccountAsync(AccountRequest request, CancellationToken cancellationToken = default);

    Task CreatePersonalLinksAsync(int accountId, IEnumerable<PersonalLinkRequest> requests, CancellationToken cancellationToken = default);
    Task UpdatePersonalLinkAsync(int id, PersonalLinkRequest request, CancellationToken cancellationToken = default);
    Task DeletePersonalLinksAsync(int accountId, IEnumerable<int> personalLinkIds, CancellationToken cancellationToken = default);

    Task CreateTitlesAsync(int accountId, IEnumerable<TitleRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateTitleAsync(int id, TitleRequest request, CancellationToken cancellationToken = default);
    Task DeleteTitlesAsync(int accountId, IEnumerable<int> titleIds, CancellationToken cancellationToken = default);

}
