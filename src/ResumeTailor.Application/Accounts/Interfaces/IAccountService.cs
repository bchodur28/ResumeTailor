using ResumeTailor.Application.Accounts.Models;

namespace ResumeTailor.Application.Accounts.Interfaces;

public interface IAccountService
{
    Task<AccountResponse> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<AccountResponse> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task CreateAccountAsync(AccountRequest request, CancellationToken cancellationToken = default);

    Task CreatePersonalLinkAsync(PersonalLinkRequest request, CancellationToken cancellationToken = default);
    Task UpdatePersonalLinkAsync(int id, PersonalLinkRequest request, CancellationToken cancellationToken = default);
    Task DeletePersonalLinkAsync(int id, CancellationToken cancellationToken = default);

    Task CreateTitleLinkAsync(TitleRequest request, CancellationToken cancellationToken = default);
    Task UpdatePersonalTitleAsync(int id, TitleRequest request, CancellationToken cancellationToken = default);
    Task DeletePersonalTitleAsync(int id, CancellationToken cancellationToken = default);

}
