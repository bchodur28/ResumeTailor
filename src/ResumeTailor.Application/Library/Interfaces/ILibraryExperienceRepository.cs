using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Library.Interfaces;

public interface ILibraryExperienceRepository
{
    Task<IReadOnlyCollection<LibraryExperienceSummaryResponse>> GetSummeriesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<LibraryExperience>> GetAllByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<LibraryExperienceListItemResponse>> GetListItemsAsync(int accountId, bool archived, CancellationToken cancellationToken = default);
    Task<LibraryExperience?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<LibraryExperienceAiInput>> GetAiInputDataForGeneration(int accountId, CancellationToken cancellationToken = default);
    Task<bool> AreSelectionsValidAsync(int accountId, IReadOnlyCollection<ExperienceSelectionValidation> selections, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<LibraryExperience>> GetByIdsAsync(IReadOnlyCollection<int> Ids, CancellationToken cancellationToken = default);
    Task<LibraryExperience?> GetForUpdatingByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> IsUsedInResumeAsync(int experienceId, CancellationToken cancellationToken = default);
    void Add(LibraryExperience experiences);
    void Remove(LibraryExperience experience);
    Task SaveAync(CancellationToken cancellationToken = default);
}
