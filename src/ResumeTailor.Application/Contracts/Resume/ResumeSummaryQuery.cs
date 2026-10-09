using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.Resumes.ApplicationTracking;

namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeSummaryQuery
{
    public int AccountId { get; set; }

    public List<ApplicationStatus>? Statuses { get; init; }
    public ResumeSummaryDateFilter DateFilter { get; init; } = ResumeSummaryDateFilter.All;

    public ResumeSummarySortBy SortBy { get; init; } = ResumeSummarySortBy.AppliedDate;

    public bool Descending { get; init; } = true;

    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 12;

}
