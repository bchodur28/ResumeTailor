using ResumeTailor.Domain.Common;


namespace ResumeTailor.Domain.Profile;

public sealed class CreditTransaction : BaseEntity
{
    public int AccountId { get; private set; }
    public CreditWallet CreditWallet { get; private set; } = null!;

    public int Amount { get; private set; }
    public CreditTransactionType Type { get; private set; }
    public int? ResumeId { get; private set; }
    public string? ReferenceId { get; private set; }

    private CreditTransaction() { }

    public CreditTransaction(CreditWallet wallet, int amount, CreditTransactionType type, int? resumeId = null, string? referenceId = null)
    {
        if (wallet == null)
        {
            throw new ArgumentNullException(nameof(wallet));
        }

        if (amount == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        CreditWallet = wallet;
        Amount = amount;
        Type = type;
        ResumeId = resumeId;
        ReferenceId = referenceId;
    }
}
