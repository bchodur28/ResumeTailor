using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction.Interfaces;

public interface ISiteExtractionDefinitionRepository
{

    Task<SiteExtractionDefinition?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SiteExtractionDefinition?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<SiteExtractionDefinition>> GetEnabledAsync(CancellationToken cancellationToken = default);
    Task<SiteExtractionDefinition?> GetMatchingDefinitionAsync(string hostname, string path, CancellationToken cancellationToken = default);
    Task CreateAsync(SiteExtractionDefinition definition, CancellationToken cancellationToken = default);
    Task UpdateAsync(SiteExtractionDefinition definition, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
    Task<int> GetNextVersionAsync(string hostname, string pathPattern, CancellationToken cancellationToken = default);

}
