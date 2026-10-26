using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Library.Interfaces;

public interface ILibraryExperienceRepository
{
    public Task<IReadOnlyCollection<LibraryExperienceSummaryResponse>> GetSummeriesByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    public Task<LibraryExperience?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<IReadOnlyCollection<LibraryExperienceAiInput>> GetAiInputDataForGeneration(int accountId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<LibraryExperience>> GetByIdsAsync(IReadOnlyCollection<int> Ids, CancellationToken cancellationToken = default);
    public Task<LibraryExperience?> GetForUpdatingByIdAsync(int id, CancellationToken cancellationToken = default);
    public void Add(LibraryExperience experiences);
    public void Remove(LibraryExperience experience);
    public Task SaveAync(CancellationToken cancellationToken = default);
}
