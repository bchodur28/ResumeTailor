using ResumeTailor.Application.Resumes.Generation.Models;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeAiGenerator
{
    Task<ResumeAiGenerationResult> GenerateAsync(ResumeAiInput context, CancellationToken cancellationToken = default);
}
