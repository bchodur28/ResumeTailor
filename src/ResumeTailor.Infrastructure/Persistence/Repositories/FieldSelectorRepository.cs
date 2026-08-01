using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Domain.Extraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Repositories;

internal sealed class FieldSelectorRepository(ResumeTailorDbContext dbContext) : IFieldSelectorRepository
{
    

    public async Task<IReadOnlyCollection<FieldSelector>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldSelectors
            .Where(definition => definition.FieldExtractionDefinitionId == fieldId)
            .ToListAsync(cancellationToken);
    }

    public async Task<FieldSelector?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldSelectors
            .AsNoTracking()
            .Where(definition => definition.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task<FieldSelector?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.FieldSelectors
            .Where(definition => definition.Id == id)
            .SingleOrDefaultAsync();
    }

    public async Task CreateAsync(FieldSelector definition, CancellationToken cancellationToken = default)
    {
        await dbContext.FieldSelectors.AddAsync(definition, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(FieldSelector definition, CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync();
    }
}
