using ResumeTailor.Domain.Resumes.JobApplications;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Application.Resumes.Common.Models
{
    public sealed record JobPostingResult(
        int? Id,
        string? CompanyName,
        string? JobTitle,
        string? Location,
        WorkStyle? WorkStyle,
        decimal? SalaryMin,
        decimal? SalaryMax,
        decimal? Salary,
        SalaryPeriod? SalaryPeriod,
        string? SalaryCurrency);
}
