using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Application.Resumes.Interfaces
{
    public interface IAiBulletChooser
    {
        Task<BulletSelectionResult> ChooseBullets(IReadOnlyList<BulletSelectionContext> contexts, string jobDescription, CancellationToken cancellationToken = default);
    }
}
