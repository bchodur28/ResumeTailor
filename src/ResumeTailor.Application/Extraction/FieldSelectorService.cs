using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Extraction.Models;
using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction
{
    internal sealed class FieldSelectorService(
        IFieldSelectorRepository selectorRepository,
        IFieldExtractionDefinitionRepository fieldRepository) : IFieldSelectorService
    {
        public async Task<IReadOnlyCollection<FieldSelectorResponse>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default)
        {
            var definition = await selectorRepository.GetByFieldIdAsync(fieldId, cancellationToken);

            return definition.Select(MapToResponse).ToList();
        }

        public async Task<FieldSelectorResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var definition = await selectorRepository.GetByIdAsync(id, cancellationToken);

            if (definition is null)
            {
                throw new NotFoundException($"Field selector definition with ID {id} was not found.");
            }

            return MapToResponse(definition);
        }

        public async Task<FieldSelectorResponse> CreateAsync(FieldSelectorRequest request, CancellationToken cancellationToken = default)
        {
            var fieldDefinitionExists = await fieldRepository.ExistsAsync(request.FieldExtractionDefinitionId, cancellationToken=default);

            if (!fieldDefinitionExists)
            {
                throw new NotFoundException($"Field extraction definition with ID {request.FieldExtractionDefinitionId} was not found when creating Selector definition");
            }

            var definition = new FieldSelector(request.FieldExtractionDefinitionId, request.Selector, request.Priority);

            await selectorRepository.CreateAsync(definition, cancellationToken);
            return MapToResponse(definition);
        }

        public async Task UpdateAsync(int id, FieldSelectorRequest request, CancellationToken cancellationToken = default)
        {
            var definition = await selectorRepository.GetByIdForUpdateAsync(id, cancellationToken);

            if (definition is null)
            {
                throw new NotFoundException($"Field selector defintion with ID {id} was not found.");
            }

            definition.Update(request.FieldExtractionDefinitionId, request.Selector, request.Priority);
        }

        private static FieldSelectorResponse MapToResponse(FieldSelector definition)
        {
            return new FieldSelectorResponse(
                definition.Id,
                definition.Selector,
                definition.Priority
                );
        }
    }
}
