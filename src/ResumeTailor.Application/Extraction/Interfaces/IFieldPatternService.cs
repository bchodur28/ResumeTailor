using ResumeTailor.Application.Extraction.Models;

namespace ResumeTailor.Application.Extraction.Interfaces;

public interface IFieldPatternService
{
    Task<IReadOnlyCollection<FieldPatternResponse>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default);
    Task<FieldPatternResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FieldPatternResponse> CreateAsync(FieldPatternRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, FieldPatternRequest request, CancellationToken cancellationToken = default);
}
