using ResumeTailor.Application.GeneratedResumes.Common.Exceptions;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;

namespace ResumeTailor.Infrastructure.AI;

internal sealed class UnabailableAiBulletChooser : IResumeAiGenerator
{
    public Task<ResumeAiGenerationResult> GenerateAsync(ResumeAiGenerationContext context, CancellationToken cancellationToken = default)
    {
        throw new AiNotConfiguredException();
    }
}
