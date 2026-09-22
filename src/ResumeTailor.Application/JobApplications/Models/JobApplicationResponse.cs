using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Application.JobApplications.Models;

public sealed record JobApplicationResponse(
    int Id,
    int AccountId,
    ResumeDetailsResponse? GeneratedResumeDetails,
    string CompanyName,
    string JobName,
    string? Location,
    WorkStyle WorkStyle,
    string? JobDescription,
    string? JobUrl,
    DateOnly? AppliedDate,
    ApplicationStatus Status
    );
