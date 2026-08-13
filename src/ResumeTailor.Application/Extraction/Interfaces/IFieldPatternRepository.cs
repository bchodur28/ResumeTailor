using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction.Interfaces;

public interface IFieldPatternRepository
{
    Task<IReadOnlyCollection<FieldPattern>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default);
    Task<FieldPattern?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FieldPattern?> GetByIdForUpdateAsync(int id, CancellationToken cancellationToken = default);
    Task CreateAsync(FieldPattern definition, CancellationToken cancellationToken = default);
    Task UpdateAsync(FieldPattern definition, CancellationToken cancellationToken = default);
}
