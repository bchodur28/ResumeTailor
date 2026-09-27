using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Resumes.JobApplications;

namespace ResumeTailor.Domain.Resumes.JobPositing;

public class ResumeJobPosting : Entity
{
    public int ResumeId { get; private set; }

    public string? CompanyName { get; private set; }
    public string? JobTitle { get; private set; }
    public string? Location { get; private set; }
    public WorkStyle? WorkStyle { get; private set; }
    public decimal? SalaryMin { get; private set; }
    public decimal? SalaryMax { get; private set; }
    public decimal? Salary { get; private set; }
    public SalaryPeriod? SalaryPeriod { get; private set; }
    public string? SalaryCurrency { get; private set; }

    public ResumeJobPosting(string? companyName, string? jobTitle, string? location, WorkStyle? workStyle, decimal? salaryMin, decimal? salaryMax, decimal? salary, SalaryPeriod? salaryPeriod, string? salaryCurrency)
    {
        CompanyName = companyName;
        JobTitle = jobTitle;
        Location = location;
        WorkStyle = workStyle;
        SalaryMin = salaryMin;
        SalaryMax = salaryMax;
        Salary = salary;
        SalaryPeriod = salaryPeriod;
        SalaryCurrency = salaryCurrency;
    }

    public void Update(string? companyName, string? jobTitle, string? location, WorkStyle? workStyle, decimal? salaryMin, decimal? salaryMax, decimal? salary, SalaryPeriod? salaryPeriod, string? salaryCurrency)
    {
        CompanyName = companyName;
        JobTitle = jobTitle;
        Location = location;
        WorkStyle = workStyle;
        SalaryMin = salaryMin;
        SalaryMax = salaryMax;
        Salary = salary;
        SalaryPeriod = salaryPeriod;
        SalaryCurrency = salaryCurrency;
        MarkUpdated();
    }
}
