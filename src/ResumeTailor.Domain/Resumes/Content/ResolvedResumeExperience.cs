

namespace ResumeTailor.Domain.Resumes.Content;

public sealed record ResolvedResumeExperience(
    int ExperienceId,
    int? SummaryId,
    string? SummaryReasoning,
    IReadOnlyCollection<ResolvedResumeBullet> Bullets);
