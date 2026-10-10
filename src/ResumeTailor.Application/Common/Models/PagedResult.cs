namespace ResumeTailor.Application.Common.Models;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int TotalUnfilteredCount,
    int Page,
    int PageSize);

