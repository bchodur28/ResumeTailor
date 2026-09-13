using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Application.Accounts.Models;

namespace ResumeTailor.Application.Accounts;

public class AccountService : IAccountService
{
    //Accounts
    public async Task<AccountResponse> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<AccountResponse> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task CreateAccountAsync(AccountRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    //Personal Links
    public async Task CreatePersonalLinkAsync(PersonalLinkRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdatePersonalLinkAsync(int id, PersonalLinkRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeletePersonalLinkAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    //Titles
    public async Task CreateTitleLinkAsync(TitleRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task UpdatePersonalTitleAsync(int id, TitleRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeletePersonalTitleAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
