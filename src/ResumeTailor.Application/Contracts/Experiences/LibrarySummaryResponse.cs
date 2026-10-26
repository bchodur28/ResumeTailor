namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibrarySummaryResponse(
    int Id,
    bool IsPrimary,
    string Value,
    string? AiRewrite,
    string? AiOpinion,
    int? AiScore,
    bool IsDeleted);
