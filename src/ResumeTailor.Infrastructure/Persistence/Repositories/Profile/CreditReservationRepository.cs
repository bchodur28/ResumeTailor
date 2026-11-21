using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Profile;

public class CreditReservationRepository(ResumeTailorDbContext dbContext) : ICreditReservationRepository
{
    public async Task<CreditReservation?> GetByIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.CreditReservations
            .SingleOrDefaultAsync(cr => cr.Id == reservationId, cancellationToken);
    }

    public async Task<int?> GetCompletedResumeIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.CreditReservations
            .AsNoTracking()
            .Where(r =>
                r.Id == reservationId &&
                r.Status == CreditReservationStatus.Completed)
            .Select(r => r.ResumeId)
            .SingleOrDefaultAsync(cancellationToken);
    }
}
