using ResumeTailor.Application.Contracts.Education;
using EducationEntity = ResumeTailor.Domain.Profile.LibraryEducation;

namespace ResumeTailor.Application.Profile.Education.Interfaces;

public interface IEducationRepository
{
    Task<IReadOnlyCollection<LibraryEducationResponse>> GetListByAccountIdAsync(int accountId, bool archived, CancellationToken cancellationToken = default);
    Task<EducationEntity?> GetForUpdatingByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<int>> GetIdsForCreationAsync(int accountId, CancellationToken cancellationToken = default);

    Task<bool> IsUsedInResumeAsync(int educationId, CancellationToken cancellationToken = default);

    void Add(EducationEntity education);
    void Remove(EducationEntity education);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
