using ResumeTailor.Application.Resumes.Exceptions;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Infrastructure.AI;

internal sealed class UnabailableAiBulletChooser : IAiBulletChooser
{
    public Task<BulletSelectionResult> ChooseBullets(IReadOnlyList<BulletSelectionContext> contexts, string jobDescription, CancellationToken cancellationToken = default)
    {
        throw new AiNotConfiguredException();
    }
}
