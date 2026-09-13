using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Application.JobApplications.Models;

public sealed record JobApplicationResponse(
    int Id,
    int AccountId,
    GeneratedResumeDetailsResponse? GeneratedResumeDetails,
    string CompanyName,
    string JobName,
    string? Location,
    WorkStyle WorkStyle,
    string? JobDescription,
    string? JobUrl,
    DateOnly? AppliedDate,
    ApplicationStatus Status
    );
