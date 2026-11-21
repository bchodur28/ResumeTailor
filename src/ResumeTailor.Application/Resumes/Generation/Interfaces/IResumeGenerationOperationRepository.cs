
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeGenerationOperationRepository
{
    Task<ResumeGenerationOperation?> GetByReservationIdAsync(int reservationId, CancellationToken cancellationToken = default);
}
