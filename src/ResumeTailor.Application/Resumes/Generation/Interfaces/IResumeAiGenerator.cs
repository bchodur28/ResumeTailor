using ResumeTailor.Application.Resumes.Generation.Models;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeAiGenerator
{
    Task<ResumeAiGenerationResult> GenerateAsync(ResumeAiContext context, CancellationToken cancellationToken = default);
}
