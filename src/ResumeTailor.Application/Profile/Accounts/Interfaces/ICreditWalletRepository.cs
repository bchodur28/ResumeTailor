

using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Profile.Accounts.Interfaces;

public interface ICreditWalletRepository
{
    Task<CreditWallet?> GetByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
}
