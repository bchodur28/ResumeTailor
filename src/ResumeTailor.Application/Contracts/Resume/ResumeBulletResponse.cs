namespace ResumeTailor.Application.Contracts.Resume;

public sealed record ResumeBulletResponse(
    int Id,
    int LibraryBulletId,
    string Value,
    string? AiAlternativeValue,
    string? AiReasoning,
    int? SortOrder,
    bool IsSourceDeleted);

