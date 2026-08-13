using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Domain.Extraction;


namespace ResumeTailor.Infrastructure.Persistence.Repositories;

internal sealed class FieldPatternRepository(ResumeTailorDbContext dbContext) : IFieldPatternRepository
{
    

    public async Task<IReadOnlyCollection<FieldPattern>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldPatterns
            .Where(definition => definition.FieldExtractionDefinitionId == fieldId)
            .ToListAsync(cancellationToken);
    }

    public async Task<FieldPattern?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldPatterns
            .AsNoTracking()
            .Where(definition => definition.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<FieldPattern?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldPatterns
            .Where(definition => definition.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task CreateAsync(FieldPattern definition, CancellationToken cancellationToken = default)
    {
        await dbContext.FieldPatterns.AddAsync(definition, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FieldPattern definition, CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync();
    }
}
