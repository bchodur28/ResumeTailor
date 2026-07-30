using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Extraction.Models;
using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction;

internal sealed class FieldExtractionDefinitionService(
    IFieldExtractionDefinitionRepository fieldRepository,
    ISiteExtractionDefinitionRepository siteRepository) : IFieldExtractionDefinitionService
{
    public async Task<IReadOnlyCollection<FieldExtractionDefinitionResponse>> GetBySiteIdAsync(int siteId, CancellationToken cancellationToken = default)
    {
        var definitions = await fieldRepository.GetBySiteIdAsync(siteId, cancellationToken);

        return definitions.Select(MapToResponse).ToList();
    }

    public async Task<FieldExtractionDefinitionResponse> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var definition = await fieldRepository.GetByIdAsync(id, cancellationToken);

        if (definition is null)
        {
            throw new NotFoundException($"Field extraction definition with ID {id} was not found.");
        }

        return MapToResponse(definition);
    }

    public async Task<FieldExtractionDefinitionResponse> CreateAsync(FieldExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        var siteDefinitionExists = await siteRepository.ExistsAsync(request.SiteExtractionDefinitionId, cancellationToken);

        if (!siteDefinitionExists)
        {
            throw new NotFoundException($"Site extraction definition with ID {request.SiteExtractionDefinitionId} was not found when creating Field extraction defintion.");
        }

        var definition = new FieldExtractionDefinition(
            request.SiteExtractionDefinitionId,
            request.FieldName,
            request.DisplayLabel,
            request.ExtractionType,
            request.IsRequired, request.AttributeName);

        await fieldRepository.CreateAsync(definition, cancellationToken);
        return MapToResponse(definition);
    }

    public async Task UpdateAsync(int id, FieldExtractionDefinitionRequest request, CancellationToken cancellationToken = default)
    {
        var definition = await fieldRepository.GetByIdAsync(id, cancellationToken);

        if (definition is null)
        {
            throw new NotFoundException($"Field extraction definition with ID {id} was not found.");
        }

        definition.Update(
            request.SiteExtractionDefinitionId,
            request.FieldName,
            request.DisplayLabel,
            request.ExtractionType,
            request.IsRequired,
            request.AttributeName);

        await fieldRepository.UpdateAsync(definition, cancellationToken);
    }

    private static FieldExtractionDefinitionResponse MapToResponse(FieldExtractionDefinition definition)
    {
        return new FieldExtractionDefinitionResponse(
            definition.Id,
            definition.FieldName.ToString(),
            definition.DisplayLabel,
            definition.ExtractionType.ToString(),
            definition.AttributeName,
            definition.IsRequired,
            definition.SortOrder,
            definition.Selectors
                .OrderBy(selector => selector.Priority)
                .Select(selector => new FieldSelectorResponse(
                    selector.Id,
                    selector.Selector,
                    selector.Priority
                    )).ToList()
            );
    }
}
