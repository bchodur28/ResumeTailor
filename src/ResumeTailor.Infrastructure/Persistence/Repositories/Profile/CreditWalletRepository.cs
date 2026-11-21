using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Domain.Profile;


namespace ResumeTailor.Infrastructure.Persistence.Repositories.Profile
{
    public class CreditWalletRepository(ResumeTailorDbContext dbContext) : ICreditWalletRepository
    {
        public async Task<CreditWallet?> GetByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
        {
            return await dbContext.CreditWallets
                .Where(cw => cw.AccountId == accountId)
                .FirstOrDefaultAsync();
        }
    }
}
