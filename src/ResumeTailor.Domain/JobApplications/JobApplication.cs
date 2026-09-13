using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.GeneratedResumes;


namespace ResumeTailor.Domain.JobApplications;

public sealed class JobApplication : Entity
{
    public int AccountId { get; private set; }
    public GeneratedResume? GeneratedResume { get; private set; }

    public string CompanyName { get; private set; } = string.Empty;
    public string JobName { get; private set; } = string.Empty;
    public string? Location { get; private set; }
    public WorkStyle WorkStyle { get; private set; }
    public string? JobDescription { get; private set; }
    public string? JobUrl { get; private set; }
    public DateOnly? AppliedDate { get; private set; }
    public ApplicationStatus Status { get; private set; }

    public JobApplication(int accountId, string companyName, string jobName, string? location, WorkStyle workStyle, string? jobDescription, string? jobUrl, DateOnly? appliedDate, ApplicationStatus status)
    {
        AccountId = accountId;
        CompanyName = companyName;
        JobName = jobName;
        Location = location;
        WorkStyle = workStyle;
        JobDescription = jobDescription;
        JobUrl = jobUrl;
        AppliedDate = appliedDate;
        Status = status;
    }

    public void Update(string companyName, string jobName, string? location, WorkStyle workStyle, string? jobDescription, string? jobUrl, DateOnly? appliedDate, ApplicationStatus status)
    {
        CompanyName = companyName;
        JobName = jobName;
        Location = location;
        WorkStyle = workStyle;
        JobDescription = jobDescription;
        JobUrl = jobUrl;
        AppliedDate = appliedDate;
        Status = status;
        MarkUpdated();
    }
}
