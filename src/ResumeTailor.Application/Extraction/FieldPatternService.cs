using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Extraction.Interfaces;
using ResumeTailor.Application.Extraction.Models;
using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Application.Extraction
{
    internal sealed class FieldPatternService(
        IFieldPatternRepository fieldPatternRepository,
        IFieldExtractionDefinitionRepository fieldRepository) : IFieldPatternService
    {
        public async Task<IReadOnlyCollection<FieldPatternResponse>> GetByFieldIdAsync(int fieldId, CancellationToken cancellationToken = default)
        {
            var definition = await fieldPatternRepository.GetByFieldIdAsync(fieldId, cancellationToken);

            return definition.Select(MapToResponse).ToList();
        }

        public async Task<FieldPatternResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var definition = await fieldPatternRepository.GetByIdAsync(id, cancellationToken);

            if (definition is null)
            {
                throw new NotFoundException($"Field pattern definition with ID {id} was not found.");
            }

            return MapToResponse(definition);
        }

        public async Task<FieldPatternResponse> CreateAsync(FieldPatternRequest request, CancellationToken cancellationToken = default)
        {
            var fieldDefinitionExists = await fieldRepository.ExistsAsync(request.FieldExtractionDefinitionId, cancellationToken=default);

            if (!fieldDefinitionExists)
            {
                throw new NotFoundException($"Field extraction definition with ID {request.FieldExtractionDefinitionId} was not found when creating Pattern definition");
            }

            var definition = new FieldPattern(request.FieldExtractionDefinitionId, request.MatchPattern, request.Priority);

            await fieldPatternRepository.CreateAsync(definition, cancellationToken);
            return MapToResponse(definition);
        }

        public async Task UpdateAsync(int id, FieldPatternRequest request, CancellationToken cancellationToken = default)
        {
            var definition = await fieldPatternRepository.GetByIdForUpdateAsync(id, cancellationToken);

            if (definition is null)
            {
                throw new NotFoundException($"Field pattern defintion with ID {id} was not found.");
            }

            definition.Update(request.FieldExtractionDefinitionId, request.MatchPattern, request.Priority);
        }

        private static FieldPatternResponse MapToResponse(FieldPattern pattern)
        {
            return new FieldPatternResponse(
                pattern.Id,
                pattern.ScopePattern,
                pattern.MatchPattern,
                pattern.Priority
                );
        }
    }
}
