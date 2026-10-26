namespace ResumeTailor.Application.Contracts.Experiences;

public sealed record LibraryBulletResponse(
    int Id,
    string Value,
    string? AiRewrite,
    string? AiOpinion,
    int? AiScore,
    bool IsDeleted);
