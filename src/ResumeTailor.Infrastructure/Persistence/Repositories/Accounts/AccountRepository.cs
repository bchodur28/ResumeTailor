using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;

internal class AccountRepository(ResumeTailorDbContext dbContext) : IAccountRepository
{
    // Account
    public async Task<Account?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AsNoTracking()
            .Include(a => a.Titles)
            .Include(a => a.PersonalLinks)
            .FirstOrDefaultAsync(a => a.Auth0UserId == auth0UserId, cancellationToken);
    }

    public async Task<Account?> GetAccountForUpdatingAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .Include(a => a.Titles)
            .Include(a => a.PersonalLinks)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task CreateAccoutAsync(Account account, CancellationToken cancellationToken = default)
    {
        await dbContext.Accounts.AddAsync(account, cancellationToken);
    }

    // Personal Link
    public async Task<PersonalLink?> GetPersonalLinkForUpdatingAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountPersonalLinks
            .FirstOrDefaultAsync(pl => pl.Id == id, cancellationToken);
    }

    //Title
    public async Task<Title?> GetTitleForUpdatingAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.AccountTitles
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    //Save  
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
