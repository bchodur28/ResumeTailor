using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction.Interfaces;

public interface IFieldExtractionDefinitionRepository
{
    Task<IReadOnlyCollection<FieldExtractionDefinition>> GetBySiteIdAsync(int siteId, CancellationToken cancellationToken = default);
    Task<FieldExtractionDefinition?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FieldExtractionDefinition?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(FieldExtractionDefinition definition, CancellationToken cancellationToken = default);
    Task UpdateAsync(FieldExtractionDefinition definition, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);

}
