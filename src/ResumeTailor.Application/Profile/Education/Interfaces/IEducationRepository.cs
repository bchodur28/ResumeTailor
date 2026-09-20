using EducationEntity = ResumeTailor.Domain.Profile.Education;

namespace ResumeTailor.Application.Profile.Education.Interfaces;

public interface IEducationRepository
{
    Task<IReadOnlyCollection<EducationEntity>> GetEducationByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<EducationEntity>> GetEducationByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<EducationEntity>> GetEducationForUpdatingByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    void AddRange(IEnumerable<EducationEntity> education);
    void RemoveRange(IEnumerable<EducationEntity> education);

    Task SaveAsync(CancellationToken cancellationToken = default);
}
