

using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Extraction;
using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Infrastructure.Persistence.Repositories;

internal class FieldExtractionDefinitionRepository(ResumeTailorDbContext dbContext) : IFieldExtractionDefinitionRepository
{
    public async Task<IReadOnlyCollection<FieldExtractionDefinition>> GetBySiteIdAsync(int siteId, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldExtractionDefinitions
            .Where(definition => definition.SiteExtractionDefinitionId == siteId)
            .Include(definition => definition.Selectors)
            .ToListAsync(cancellationToken);
    }

    public async Task<FieldExtractionDefinition?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldExtractionDefinitions
            .AsNoTracking()
            .Where(definition => definition.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<FieldExtractionDefinition?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldExtractionDefinitions
            .Where(definition => definition.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(FieldExtractionDefinition definition, CancellationToken cancellationToken = default)
    {
        await dbContext.FieldExtractionDefinitions.AddAsync(definition, cancellationToken);
    }

    public async Task UpdateAsync(FieldExtractionDefinition definition, CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

