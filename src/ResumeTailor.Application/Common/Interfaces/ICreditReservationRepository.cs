using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Common.Interfaces;

public interface ICreditReservationRepository
{
    Task<CreditReservation?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default);

    Task<int?> GetCompletedResumeIdAsync(int reservationId, CancellationToken cancellationToken = default);
}
