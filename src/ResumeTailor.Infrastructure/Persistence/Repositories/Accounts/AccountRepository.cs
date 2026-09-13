using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;

internal class AccountRepository(ResumeTailorDbContext dbContext) : IAccountRepository
{
    // Account
    public async Task<Account?> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AsNoTracking()
            .Include(a => a.Titles)
            .Include(a => a.PersonalLinks)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Account?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AsNoTracking()
            .Include(a => a.Titles)
            .Include(a => a.PersonalLinks)
            .FirstOrDefaultAsync(a => a.Auth0UserId == auth0UserId, cancellationToken);
    }

    public async Task CreateAccoutAsync(Account account, CancellationToken cancellationToken = default)
    {
        await dbContext.Accounts.AddAsync(account, cancellationToken);
    }

    // Personal Link
    public async Task CreatePersonalLinkAsync(PersonalLink personalLink, CancellationToken cancellationToken = default)
    {
        await dbContext.AccountPersonalLinks.AddAsync(personalLink, cancellationToken);
    }

    public async Task DeletePersonalLinkAsync(int id, CancellationToken cancellationToken = default)
    {
        var personalLink = await dbContext.AccountPersonalLinks.FindAsync([id], cancellationToken);

        if (personalLink is null)
        {
            return;
        }

        dbContext.AccountPersonalLinks.Remove(personalLink);
    }

    public Task PersonalLinkExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return dbContext.AccountPersonalLinks.AnyAsync(pl => pl.Id == id, cancellationToken);
    }

    //Title
    public async Task CreateTitleAsync(Title title, CancellationToken cancellationToken = default)
    {
        await dbContext.AccountTitles.AddAsync(title, cancellationToken);
    }

    public async Task DeleteTitleAsync(int id, CancellationToken cancellationToken = default)
    {
        var title = await dbContext.AccountTitles.FindAsync([id], cancellationToken);

        if(title is null)
        {
            return;
        }

        dbContext.AccountTitles.Remove(title);
    }

    public Task TitleExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return dbContext.AccountTitles.AnyAsync(t => t.Id == id, cancellationToken);
    }

    //Save  
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
