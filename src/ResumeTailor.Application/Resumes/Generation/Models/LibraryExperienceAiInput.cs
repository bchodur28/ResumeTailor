namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record LibraryExperienceAiInput(
    int ExperienceId,
    bool LetAiChooseSummary,
    Dictionary<int, string> Summaries,
    bool LetAiChooseBullets,
    int? MaxBullets,
    Dictionary<int, string> Bullets,
    DateOnly? Started,
    DateOnly? Ended);
