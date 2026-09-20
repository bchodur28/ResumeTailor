using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Profile.Accounts;

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

    public async Task<int> CreateAccountAsync(string auth0UserId, AccountRequest request, CancellationToken cancellationToken = default)
    {
        var account = MapAccountRequestToDomain(request, auth0UserId);
        await accountRepository.CreateAccoutAsync(account, cancellationToken);
        await accountRepository.SaveAsync(cancellationToken);

        return account.Id;
    }

    public async Task UpdateAccountAsync(string auth0UserId, AccountRequest request, CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.GetAccountForUpdatingAsync(auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when updating.");

        account.Update(request.Email, request.DisplayName, request.City, request.State, request.Country);

        UpdateTitles(account, request.Titles);
        UpdatePersonalLinks(account, request.PersonalLinks);

        await accountRepository.SaveAsync(cancellationToken);
    }

    private static void UpdateTitles(Account account, IReadOnlyCollection<TitleRequest> requests)
    {
        var existisngRequestIds = requests
            .Where(rt => rt.Id.HasValue)
            .Select(rt => rt.Id!.Value)
            .ToHashSet();

        var titlesToDelete = account.Titles
            .Where(t => !existisngRequestIds.Contains(t.Id))
            .ToList();

        foreach(var title in titlesToDelete)
        {
            account.RemoveTitle(title);
        }

        foreach(var request in requests)
        {
            if(request.Id.HasValue)
            {
                var existingTitle = account.Titles.FirstOrDefault(t => t.Id == request.Id.Value)
                    ?? throw new NotFoundException($"Title with ID {request.Id.Value} was not found when updating.");
                if (existingTitle != null)
                {
                    existingTitle.Update(request.Value, request.IsPrimary);
                }
            }
            else
            {
                account.AddTitle(request.Value, request.IsPrimary);
            }
        }
    }

    private static void UpdatePersonalLinks(Account account, IReadOnlyCollection<PersonalLinkRequest> requests)
    {
        var existingRequestIds = requests
            .Where(r => r.Id.HasValue)
            .Select(r => r.Id!.Value)
            .ToHashSet();

        var personalLinksToDelete = account.PersonalLinks
            .Where(pl => !existingRequestIds.Contains(pl.Id))
            .ToList();

        foreach (var personalLink in personalLinksToDelete)
        {
            account.RemovePersonalLink(personalLink);
        }

        foreach (var request in requests)
        {
            if (request.Id.HasValue)
            {
                var existingLink = account.PersonalLinks.FirstOrDefault(pl => pl.Id == request.Id.Value)
                    ?? throw new NotFoundException($"Personal link with ID {request.Id.Value} was not found when updating.");
                if (existingLink != null)
                {
                    existingLink.Update(request.DisplayName, request.Url);
                }
            } else
            {
                account.AddPersonalLink(request.DisplayName, request.Url);
            }
        }
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
                .Select(t => new TitleResponse(t.Id, t.Value, t.IsPrimary))
                .ToList()
        );
    }

    private static Account MapAccountRequestToDomain(AccountRequest request, string auth0UserId)
    {
        var account = new Account(
            auth0UserId,
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
