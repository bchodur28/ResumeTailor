using ResumeTailor.Domain.Resumes.JobApplications;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeJobPostingRequest(
    string? CompanyName,
    string? JobTitle,
    string? Location,
    WorkStyle? WorkStyle,
    decimal? SalaryMin,
    decimal? SalaryMax,
    decimal? Salary,
    SalaryPeriod? SalaryPeriod,
    string? SalaryCurrency);
