using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Resumes;

public class ResumeGenerationOperation : Entity
{
    public int AccountId { get; private set; }
    public int ReservationId { get; private set; }

    public Guid RequestId { get; private set; }
    public string RequestHash { get; private set; } = string.Empty;

    public ResumeGenerationStatus Status { get; private set; }

    public Guid LeaseToken { get; private set; }
    public DateTime LeaseExpiresAt { get; private set; }

    public string? AiResultJson { get; private set; }
    public int? ResumeId { get; private set; }

    private ResumeGenerationOperation() { }

    public ResumeGenerationOperation(
        int accountId,
        int reservationId,
        Guid requestId,
        string requestHash,
        Guid leaseToken,
        DateTime leaseExpiresAt)
    {
        AccountId = accountId;
        ReservationId = reservationId;
        RequestId = requestId;
        RequestHash = requestHash;
        LeaseToken = leaseToken;
        LeaseExpiresAt = leaseExpiresAt;
        Status = ResumeGenerationStatus.Generating;
    }

    public void MarkReadyToFinalize(Guid leaseToken, string aiResultJson)
    {
        if (Status != ResumeGenerationStatus.Generating ||
            LeaseToken != leaseToken)
        {
            throw new InvalidOperationException("Generation operation is no longer owned by this worker.");
        }

        if (string.IsNullOrWhiteSpace(aiResultJson))
        {
            throw new ArgumentException("AI result cannot be empty.", nameof(aiResultJson));
        }

        AiResultJson = aiResultJson;
        Status = ResumeGenerationStatus.ReadyToFinalize;

        MarkUpdated();
    }

    public void Complete(int resumeId)
    {
        if (Status != ResumeGenerationStatus.ReadyToFinalize)
            throw new InvalidOperationException("Generation is not ready to finalize.");

        ResumeId = resumeId;
        Status = ResumeGenerationStatus.Completed;
        MarkUpdated();
    }

    public void Fail(Guid leaseToken)
    {
        if (Status != ResumeGenerationStatus.Generating)
            throw new InvalidOperationException("Only generating operations can be failed.");

        if (LeaseToken != leaseToken)
            throw new InvalidOperationException("Generation operation is not owned by this worker.");

        Status = ResumeGenerationStatus.Failed;
        MarkUpdated();
    }
}
