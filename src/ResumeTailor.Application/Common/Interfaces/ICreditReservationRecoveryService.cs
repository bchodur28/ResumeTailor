namespace ResumeTailor.Application.Common.Interfaces;

public interface ICreditReservationRecoveryService
{
    Task RecoverAsync(CancellationToken cancellationToken = default);
}
