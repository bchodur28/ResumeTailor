namespace ResumeTailor.Application.Resumes.Generation.Models;

public sealed record AiExperienceOutput(int ExperienceId, AiSummaryOutput? ChosenSummary, IReadOnlyCollection<AiBulletOutput> ChosenBullets);

