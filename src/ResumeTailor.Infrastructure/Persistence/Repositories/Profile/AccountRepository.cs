using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;

internal class AccountRepository(ResumeTailorDbContext dbContext) : IAccountRepository
{
    // Account
    public async Task<Account?> GetAccountByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Titles)
            .Include(a => a.PersonalLinks)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<Account?> GetAccountByAuth0UserIdAsync(string auth0UserId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AsNoTracking()
            .AsSplitQuery()
            .Include(a => a.Titles)
            .Include(a => a.PersonalLinks)
            .FirstOrDefaultAsync(a => a.Auth0UserId == auth0UserId, cancellationToken);
    }

    public async Task<int?> GetAccountIdByAuth0UserAsync(string auth0UserId, CancellationToken cancellationToken = default)
    {
        var accountId = await dbContext.Accounts
            .Where(a => a.Auth0UserId == auth0UserId)
            .Select(a => (int?)a.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        return accountId;
    }

    public async Task<Account?> GetAccountForUpdatingAsync(string auth0UserId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Accounts
            .AsSplitQuery()
            .Include(a => a.Titles)
            .Include(a => a.PersonalLinks)
            .FirstOrDefaultAsync(a => a.Auth0UserId == auth0UserId, cancellationToken);
    }

    public async Task CreateAccoutAsync(Account account, CancellationToken cancellationToken = default)
    {
        await dbContext.Accounts.AddAsync(account, cancellationToken);
    }

    //Save  
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
