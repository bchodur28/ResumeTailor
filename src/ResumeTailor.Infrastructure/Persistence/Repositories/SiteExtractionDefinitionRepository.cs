using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Domain.Extraction;
using System.Formats.Asn1;

namespace ResumeTailor.Infrastructure.Persistence.Repositories;

internal sealed class SiteExtractionDefinitionRepository(ResumeTailorDbContext dbContext) : ISiteExtractionDefinitionRepository
{
    public async Task<SiteExtractionDefinition?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.SiteExtractionDefinitions
            .Include(definition => definition.Fields)
                .ThenInclude(field => field.Selectors)
            .SingleOrDefaultAsync(definition => definition.Id == id, cancellationToken);
    }

    public async Task<SiteExtractionDefinition?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.SiteExtractionDefinitions
            .AsNoTracking()
            .Include(definition => definition.Fields)
                .ThenInclude(field => field.Selectors)
            .SingleOrDefaultAsync(definition => definition.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<SiteExtractionDefinition>> GetEnabledAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.SiteExtractionDefinitions
            .AsNoTracking()
            .Where(definition => definition.IsEnabled)
            .Include(definition => definition.Fields.OrderBy(field => field.SortOrder))
                .ThenInclude(field => field.Selectors)
            .ToListAsync(cancellationToken);
    }

    public async Task<SiteExtractionDefinition?> GetMatchingDefinitionAsync(string hostName, string path, CancellationToken cancellationToken = default)
    {
        return await dbContext.SiteExtractionDefinitions
            .AsNoTracking()
            .Include(definition => definition.Fields)
                .ThenInclude(field => field.Selectors)
            .Where(definition =>
                definition.IsEnabled &&
                definition.Hostname == hostName &&
                path.StartsWith(definition.PathPattern))
            .OrderByDescending(definition => definition.Version)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task CreateAsync(SiteExtractionDefinition definition, CancellationToken cancellationToken = default)
    {
        await dbContext.SiteExtractionDefinitions.AddAsync(definition, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(SiteExtractionDefinition definition, CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.SiteExtractionDefinitions.AnyAsync(definition => definition.Id == id, cancellationToken);    
    }

    public async Task<int> GetNextVersionAsync(string hostname, string pathPattern, CancellationToken cancellationToken = default)
    {
        var highestVersion = await dbContext.SiteExtractionDefinitions
            .Where(definition =>
            definition.Hostname == hostname &&
            definition.PathPattern == pathPattern)
            .Select(defintion => (int?)defintion.Version)
            .MaxAsync(cancellationToken);

        return (highestVersion ?? 0) + 1;
    }
}
