
using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Profile.Education.Interfaces;
using EducationEntity = ResumeTailor.Domain.Profile.Education;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;

public class EducationRepository(ResumeTailorDbContext dbContext) : IEducationRepository
{
    public async Task<IReadOnlyCollection<EducationEntity>> GetEducationByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Education
            .AsNoTracking()
            .Where(e => e.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<EducationEntity>> GetEducationByIdsAsync(HashSet<int> ids, CancellationToken cancellationToken = default)
    {
        return await dbContext.Education
            .AsNoTracking()
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<EducationEntity>> GetEducationForUpdatingByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await dbContext.Education
            .Where(e => e.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }

    public void AddRange(IEnumerable<EducationEntity> education)
    {
        dbContext.Education.AddRange(education);
    }

    public void RemoveRange(IEnumerable<EducationEntity> education)
    {
        dbContext.Education.RemoveRange(education);
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
