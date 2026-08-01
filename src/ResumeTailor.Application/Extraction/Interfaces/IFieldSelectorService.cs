using ResumeTailor.Application.Extraction.Models;

namespace ResumeTailor.Application.Extraction.Interfaces;

public interface IFieldSelectorService
{
    Task<IReadOnlyCollection<FieldSelectorResponse>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default);
    Task<FieldSelectorResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FieldSelectorResponse> CreateAsync(FieldSelectorRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, FieldSelectorRequest request, CancellationToken cancellationToken = default);
}
