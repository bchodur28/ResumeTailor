using ResumeTailor.Application.Extraction.Models;


namespace ResumeTailor.Application.Extraction.Interfaces;

public interface IFieldExtractionDefinitionService
{
    Task<IReadOnlyCollection<FieldExtractionDefinitionResponse>> GetBySiteIdAsync(int siteId, CancellationToken cancellationToken = default);
    Task<FieldExtractionDefinitionResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<FieldExtractionDefinitionResponse> CreateAsync(FieldExtractionDefinitionRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, FieldExtractionDefinitionRequest request, CancellationToken cancellationToken = default);
}
