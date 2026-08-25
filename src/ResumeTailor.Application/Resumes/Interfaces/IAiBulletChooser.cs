using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Application.Resumes.Interfaces
{
    public interface IAiBulletChooser
    {
        Task<Dictionary<string, IReadOnlyList<string>>> ChooseBullets(IReadOnlyList<BulletSelectionContext> contexts, string jobDescription);
    }
}
