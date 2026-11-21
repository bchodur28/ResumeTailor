using ResumeTailor.Application.Resumes.Generation.Models;

namespace ResumeTailor.Application.Common.Interfaces;

public interface ICreditReservationService
{
    Task<int> ReserveAsync(int accountId, int amount, Guid requestId, CancellationToken cancellationToken = default);
    Task ReleaseAsync(int reservationId, CancellationToken cancellationToken = default);
    Task<GenerationStartResult> StartGenerationAsync(int accountId, int amount, Guid requestId, string requestHash, CancellationToken cancellationToken = default);
    Task FailGenerationAsync(int operationId, Guid leaseToken, CancellationToken cancellationToken = default);
}
