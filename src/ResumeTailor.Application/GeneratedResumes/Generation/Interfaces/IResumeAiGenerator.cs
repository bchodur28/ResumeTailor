using ResumeTailor.Application.GeneratedResumes.Generation.Models;

namespace ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;

public interface IResumeAiGenerator
{
    Task<ResumeAiGenerationResult> GenerateAsync(ResumeAiGenerationContext context, CancellationToken cancellationToken = default);
}
