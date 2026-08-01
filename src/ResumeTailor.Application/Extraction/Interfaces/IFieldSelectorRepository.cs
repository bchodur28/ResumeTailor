using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction.Interfaces;

public interface IFieldSelectorRepository
{
    Task<IReadOnlyCollection<FieldSelector>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default);
    Task<FieldSelector?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FieldSelector?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(FieldSelector definition, CancellationToken cancellationToken = default);
    Task UpdateAsync(FieldSelector definition, CancellationToken cancellationToken = default);
}
