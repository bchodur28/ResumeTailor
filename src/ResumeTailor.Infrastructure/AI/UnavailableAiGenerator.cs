using ResumeTailor.Application.GeneratedResumes.Common.Exceptions;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;

namespace ResumeTailor.Infrastructure.AI;

internal sealed class UnavailableAiGenerator : IResumeAiGenerator
{
    public Task<ResumeAiGenerationResult> GenerateAsync(ResumeAiContext context, CancellationToken cancellationToken = default)
    {
        throw new AiNotConfiguredException();
    }
}
