using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Extraction.Models;
using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction;

internal sealed class SiteExtractionDefinitionService(ISiteExtractionDefinitionRepository repository) : ISiteExtractionDefinitionService
{
    
    public async Task<SiteExtractionDefinitionResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var definition = await repository.GetByIdAsync(id, cancellationToken);

        if (definition is null)
        {
            throw new NotFoundException($"Site extraction definition with ID {id} was not found.");
        }

        return MapToResponse(definition);
    }

    public async Task<IReadOnlyCollection<SiteExtractionDefinitionResponse>> GetEnabledAsync(CancellationToken cancellationToken = default)
    {
        var definitions = await repository.GetEnabledAsync(cancellationToken);

        return definitions.Select(MapToResponse).ToList();
    }

    public async Task<SiteExtractionDefinitionResponse?> GetMatchingDefinitionAsync(string hostname, string path, CancellationToken cancellationToken = default)
    {
        var definition = await repository.GetMatchingDefinitionAsync(hostname, path, cancellationToken);

        return definition is null
            ? null
            : MapToResponse(definition);
    }

    public async Task<SiteExtractionDefinitionResponse> CreateAsync(SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        var version = await repository.GetNextVersionAsync(request.HostName, request.PathPattern, cancellationToken);
        var defintion = new SiteExtractionDefinition(request.SiteName, request.HostName, request.PathPattern,version);

        await repository.CreateAsync(defintion, cancellationToken);
        return MapToResponse(defintion);
    }

    public async Task UpdateAsync(int id, SiteExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        var definition = await repository.GetByIdForUpdateAsync(id, cancellationToken);

        if (definition is null)
        {
            throw new NotFoundException($"Site extraction definition with ID {id} was not found.");
        }

        definition.Update(request.SiteName, request.HostName, request.PathPattern, request.IsEnabled);

        await repository.UpdateAsync(definition, cancellationToken);
    }

    private static SiteExtractionDefinitionResponse MapToResponse(SiteExtractionDefinition definition)
    {
        return new SiteExtractionDefinitionResponse(
            definition.Id,
            definition.SiteName,
            definition.Hostname,
            definition.PathPattern,
            definition.Version,
            definition.IsEnabled,
            definition.Fields
                .OrderBy(field => field.SortOrder)
                .Select(field => new FieldExtractionDefinitionResponse(
                    field.Id,
                    field.FieldName.ToString(),
                    field.DisplayLabel,
                    field.ExtractionType.ToString(),
                    field.AttributeName,
                    field.IsRequired,
                    field.SortOrder,
                    field.Selectors
                        .OrderBy(selector => selector.Priority)
                        .Select(selector => new FieldSelectorResponse(
                            selector.Id,
                            selector.Selector,
                            selector.Priority)
                        ).ToList())
                ).ToList());
    }
}
