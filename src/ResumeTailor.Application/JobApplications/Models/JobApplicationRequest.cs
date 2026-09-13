using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Application.JobApplications.Models;

public sealed record JobApplicationRequest(
    int AccountId,
    string CompanyName,
    string JobName,
    string? Location,
    WorkStyle WorkStyle,
    string JobDescription,
    string JobUrl,
    DateOnly? AppliedDate,
    ApplicationStatus Status
    );
