using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Application.JobApplications.Models;

public sealed record JobApplicationListItemResponse(
    int Id,
    int AccountId,
    string CompanyName,
    string JobName,
    string? Location,
    WorkStyle WorkStyle,
    DateOnly? AppliedDate,
    ApplicationStatus Status
    );
