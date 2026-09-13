using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Application.Accounts.Models;
using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Application.Accounts;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
    //Accounts
    public async Task<AccountResponse?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.GetAccountByAuth0UserIdAsync(auth0UserId, cancellationToken);

        return account is null
            ? null
            : MapAccountDomainToResponse(account);
    }

    public async Task<int> CreateAccountAsync(AccountRequest request, CancellationToken cancellationToken = default)
    {
        var account = MapAccountRequestToDomain(request);
        await accountRepository.CreateAccoutAsync(account, cancellationToken);
        await accountRepository.SaveAsync(cancellationToken);

        return account.Id;
    }

    //Personal Links
    public async Task CreatePersonalLinksAsync(int accountId, IEnumerable<PersonalLinkRequest> requests, CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.GetAccountForUpdatingAsync(accountId, cancellationToken)
            ?? throw new NotFoundException($"Account with ID {accountId} was not found.");

        foreach(var request in requests)
        {
            account.AddPersonalLink(request.DisplayName, request.Url);
        }

        await accountRepository.SaveAsync(cancellationToken);
    }

    public async Task UpdatePersonalLinkAsync(int id, PersonalLinkRequest request, CancellationToken cancellationToken = default)
    {
        var existingLink = await accountRepository.GetPersonalLinkForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Personal link with ID {id} was not found when updating.");

        existingLink.Update(request.DisplayName, request.Url);
        await accountRepository.SaveAsync(cancellationToken);
    }

    public async Task DeletePersonalLinksAsync(int accountId, IEnumerable<int> personalLinkIds, CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.GetAccountForUpdatingAsync(accountId, cancellationToken)
            ?? throw new NotFoundException($"Account with ID {accountId} was not found when deleting.");

        var idsToDelete = personalLinkIds.ToHashSet();

        var personalLinksToDelete = account.PersonalLinks
            .Where(pl => idsToDelete.Contains(pl.Id))
            .ToList();

        foreach(var personalLink in personalLinksToDelete)
        {
            account.RemovePersonalLink(personalLink);
        }

        await accountRepository.SaveAsync(cancellationToken);
    }

    //Titles
    public async Task CreateTitlesAsync(int accountId, IEnumerable<TitleRequest> requests, CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.GetAccountForUpdatingAsync(accountId, cancellationToken)
            ?? throw new NotFoundException($"Account with ID {accountId} was not found.");

        foreach (var request in requests)
        {
            account.AddTitle(request.Value, request.IsPrimary);
        }

        await accountRepository.SaveAsync(cancellationToken);
    }

    public async Task UpdateTitleAsync(int id, TitleRequest request, CancellationToken cancellationToken = default)
    {
        var existingTitle = await accountRepository.GetTitleForUpdatingAsync(id, cancellationToken)
            ?? throw new NotFoundException($"Personal link with ID {id} was not found when updating.");

        existingTitle.Update(request.Value, request.IsPrimary);
        await accountRepository.SaveAsync(cancellationToken);
    }

    public async Task DeleteTitlesAsync(int accountId, IEnumerable<int> titleIds, CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.GetAccountForUpdatingAsync(accountId, cancellationToken)
            ?? throw new NotFoundException($"Account with ID {accountId} was not found when deleting.");

        var idsToDelete = titleIds.ToHashSet();

        var titlesToDelete = account.Titles
            .Where(pl => idsToDelete.Contains(pl.Id))
            .ToList();

        foreach (var title in titlesToDelete)
        {
            account.RemoveTitle(title);
        }

        await accountRepository.SaveAsync(cancellationToken);
    }

    private static AccountResponse MapAccountDomainToResponse(Account account)
    {
        return new AccountResponse(
            account.Id,
            account.Auth0UserId,
            account.Email,
            account.DisplayName,
            account.City,
            account.State,
            account.Country,
            account.PersonalLinks
                .Select(pl => new PersonalLinkResponse(pl.Id, pl.DisplayName, pl.Url))
                .ToList(),
            account.Titles
                .Select(t => new TitleResponse(t.Id, t.Value))
                .ToList()
        );
    }

    private static Account MapAccountRequestToDomain(AccountRequest request)
    {
        var account = new Account(
            request.Auth0UserId,
            request.Email,
            request.DisplayName,
            request.City,
            request.State,
            request.Country
        );

        foreach (var personalLinkRequest in request.PersonalLinks)
        {
            account.AddPersonalLink(personalLinkRequest.DisplayName, personalLinkRequest.Url);
        }

        foreach(var titleRequest in request.Titles)
        {
            account.AddTitle(titleRequest.Value, titleRequest.IsPrimary);
        }

        return account;
    }
}
