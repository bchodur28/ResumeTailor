using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Application.Accounts.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Account?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default);
    Task CreateAccoutAsync(Account account, CancellationToken cancellationToken = default);

    Task CreatePersonalLinkAsync(PersonalLink personalLink, CancellationToken cancellationToken = default);
    Task DeletePersonalLinkAsync(int id, CancellationToken cancellationToken = default);
    Task PersonalLinkExistsAsync(int id, CancellationToken cancellationToken = default);

    Task CreateTitleAsync(Title title, CancellationToken cancellationToken = default);
    Task DeleteTitleAsync(int id, CancellationToken cancellationToken = default);
    Task TitleExistsAsync(int id, CancellationToken cancellationToken = default);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
