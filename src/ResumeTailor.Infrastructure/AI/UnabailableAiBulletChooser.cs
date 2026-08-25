using ResumeTailor.Application.Resumes.Exceptions;
using ResumeTailor.Application.Resumes.Interfaces;
using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Infrastructure.AI;

internal sealed class UnabailableAiBulletChooser : IAiBulletChooser
{
    public Task<Dictionary<string, IReadOnlyList<string>>> ChooseBullets(IReadOnlyList<BulletSelectionContext> contexts, string jobDescription)
    {
        throw new AiNotConfiguredException();
    }
}
