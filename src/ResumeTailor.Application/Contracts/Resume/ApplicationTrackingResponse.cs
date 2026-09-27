using ResumeTailor.Domain.Resumes.ApplicationTracking;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ApplicationTrackingResponse(
    ApplicationStatus Status,
    DateOnly? Applied,
    DateOnly? Interviewed,
    DateOnly? OfferReceived,
    DateOnly? OfferAccepted,
    DateOnly? Rejected);
