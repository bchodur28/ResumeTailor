using ResumeTailor.Domain.Common;

namespace ResumeTailor.Domain.Resumes.ApplicationTracking;

public class ResumeApplicationTracking : Entity
{
    public int ResumeId { get; private set; }

    public ApplicationStatus Status { get; private set; }
    public DateOnly? Applied { get; private set; }
    public DateOnly? Interviewed { get; private set; }
    public DateOnly? OfferReceived { get; private set; }
    public DateOnly? OfferAccepted { get; private set; }
    public DateOnly? Rejected { get; private set; }

    public ResumeApplicationTracking(ApplicationStatus status, DateOnly? applied, DateOnly? interviewed, DateOnly? offerReceived, DateOnly? offerAccepted, DateOnly? rejected)
    {
        Status = status;
        Applied = applied;
        Interviewed = interviewed;
        OfferReceived = offerReceived;
        OfferAccepted = offerAccepted;
        Rejected = rejected;
    }

    public void UpdateStatus(ApplicationStatus status, DateOnly date)
    {
        if (Status == status)
            return;

        Status = status;

        switch (status)
        {
            case ApplicationStatus.Applied:
                Applied ??= date;
                break;

            case ApplicationStatus.Interviewing:
                Interviewed ??= date;
                break;

            case ApplicationStatus.OfferReceived:
                OfferReceived ??= date;
                break;

            case ApplicationStatus.OfferAccepted:
                OfferAccepted ??= date;
                break;

            case ApplicationStatus.Rejected:
                Rejected ??= date;
                break;
        }

        MarkUpdated();
    }
}
