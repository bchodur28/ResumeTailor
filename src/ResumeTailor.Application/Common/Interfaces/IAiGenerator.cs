using ResumeTailor.Application.Common.Models;

namespace ResumeTailor.Application.Common.Interfaces;

public interface IAiGenerator<T>
{
    Task<AiResult<T>> GenerateAsync(AiContext context, CancellationToken cancellationToken = default);
}
