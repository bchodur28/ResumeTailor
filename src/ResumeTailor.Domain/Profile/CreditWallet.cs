using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Common.Exceptions;
using System.Diagnostics.CodeAnalysis;

namespace ResumeTailor.Domain.Profile;

public class CreditWallet : Entity
{
    public int AccountId { get; private set; }
    public Account Account { get; private set; } = null!;

    public int Balance { get; private set; }
    public int ReservedCredits { get; private set; }

    public int AvailableCredits => Balance - ReservedCredits;

    private readonly List<CreditTransaction> _transactions = [];
    public IReadOnlyCollection<CreditTransaction> Transactions => _transactions.AsReadOnly();

    //Concurrency
    public byte[] RowVersion { get; private set; } = [];

    private CreditWallet() { }

    public CreditWallet(Account account)
    {
        Account = account ?? throw new ArgumentNullException(nameof(account));
        Balance = 0;
    }

    public bool HasEnoughCredit(int amount)
    {
        return amount > 0 && AvailableCredits >= amount;
    }

    public void ReserveCredits(int amount)
    {
        if (!HasEnoughCredit(amount))
        {
            throw new InsufficientCreditsException("Insufficient funds while reserving credits.");
        }
            

        ReservedCredits = checked(ReservedCredits + amount);
    }

    public void ReleaseCredits(int amount)
    {
        if (amount <= 0 || amount > ReservedCredits)
        {
            throw new InvalidOperationException("Invalid credit release.");
        }
            

        ReservedCredits -= amount;
    }

    private void AddCredits(int amount, CreditTransactionType type, int? resumeId = null, string? referenceId = null)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        var newBalance = checked(Balance + amount);

        _transactions.Add(new CreditTransaction(
            wallet: this,
            amount: amount,
            type: type,
            resumeId: resumeId,
            referenceId: referenceId
        ));

        Balance = newBalance;
        MarkUpdated();
    }

    private void DeductCredits(int amount, CreditTransactionType type, int? resumeId = null, string? referenceId = null)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        if (Balance < amount)
        {
            throw new InvalidOperationException("Insufficient credits.");
        }

        _transactions.Add(new CreditTransaction(
            wallet: this,
            amount: -amount,
            type: type,
            resumeId: resumeId,
            referenceId: referenceId
        ));

        Balance -= amount;
        MarkUpdated();
    }

    public void SpendReservedCreditsForResume(int amount, int resumeId)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        if (ReservedCredits < amount)
        {
            throw new InvalidOperationException("Insufficient reserved credits.");
        }
            

        if (Balance < amount)
        {
            throw new InsufficientCreditsException("Insufficient funds");
        }
            

        SpendCreditsForResumeInternal(amount, resumeId);

        ReservedCredits -= amount;
        MarkUpdated();
    }

    private void SpendCreditsForResumeInternal(int amount, int resumeId)
    {
        Balance -= amount;

        _transactions.Add(new CreditTransaction(
            wallet: this,
            amount: -amount,
            type: CreditTransactionType.ResumeGeneration,
            resumeId: resumeId
        ));
    }

    public void PurchaseCredits(int amount, string referenceId)
    {
        if (string.IsNullOrWhiteSpace(referenceId))
        {
            throw new ArgumentException("A payment reference is required.", nameof(referenceId));
        }
        AddCredits(amount, CreditTransactionType.Purchase, referenceId: referenceId);
    }

    public void GrantPromotionalCredits(int amount)
    {
        AddCredits(amount, CreditTransactionType.PromotionalGrant);
    }

    public void SpendCreditsForResume(int amount, int resumeId)
    {
        DeductCredits(amount, CreditTransactionType.ResumeGeneration, resumeId);
    }

    public void RefundCredits(int amount, int? resumeId = null)
    {
        AddCredits(amount, CreditTransactionType.Refund, resumeId);
    }
}
