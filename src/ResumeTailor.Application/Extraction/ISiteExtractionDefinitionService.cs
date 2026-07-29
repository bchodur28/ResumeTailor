using ResumeTailor.Application.Extraction.Models;

namespace ResumeTailor.Application.Extraction;

public interface ISiteExtractionDefinitionService
{
    Task<IReadOnlyCollection<SiteExtractionDefinitionResponse>> GetEnabledAsync(CancellationToken cancellationToken = default);
    Task<SiteExtractionDefinitionResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<SiteExtractionDefinitionResponse> GetMatchingDefinitionAsync(string hostname, string path, CancellationToken cancellationToken = default);
    Task<SiteExtractionDefinitionResponse> CreateAsync(SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default);
}
