using ResumeTailor.Domain.Resumes.ApplicationTracking;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ApplicationTrackingResponse(
    int Id,
    ApplicationStatus Status,
    DateOnly? Applied,
    DateOnly? Interviewed,
    DateOnly? OfferReceived,
    DateOnly? OfferAccepted,
    DateOnly? Rejected);
