using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Profile;

public class CreditReservation : Entity
{
    public int WalletId { get; private set; }

    public Guid RequestId { get; private set; }
    public DateTime LeaseExpiresAt { get; private set; }

    public int Amount { get; private set; }
    public CreditReservationStatus Status { get; private set; }

    public int? ResumeId { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? ReleasedAt { get; private set; }

    private CreditReservation() { }

    public CreditReservation(int walletId, int amount, Guid requestId, DateTime leaseExpiresAt)
    {
        if (amount <= 0)
            throw new ArgumentOutOfRangeException(nameof(amount));

        if (requestId == Guid.Empty)
            throw new ArgumentException("Request ID is required.", nameof(requestId));

        if (leaseExpiresAt <= DateTime.UtcNow)
            throw new ArgumentException("Lease must expire in the future.", nameof(leaseExpiresAt));

        WalletId = walletId;
        Amount = amount;
        RequestId = requestId;
        LeaseExpiresAt = leaseExpiresAt;
        Status = CreditReservationStatus.Pending;
    }

    public void Complete(int resumeId)
    {
        if (Status != CreditReservationStatus.Pending)
            throw new InvalidOperationException("Reservation is not pending.");

        ResumeId = resumeId;
        Status = CreditReservationStatus.Completed;
        CompletedAt = DateTime.UtcNow;

        MarkUpdated();
    }

    public void Release()
    {
        if (Status != CreditReservationStatus.Pending)
            throw new InvalidOperationException("Reservation is not pending.");

        Status = CreditReservationStatus.Released;
        ReleasedAt = DateTime.UtcNow;

        MarkUpdated();
    }
}
