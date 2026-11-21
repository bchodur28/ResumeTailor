using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Domain.Resumes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Repositories.Resumes;

public class ResumeGenerationOperationRepository(ResumeTailorDbContext dbContext) : IResumeGenerationOperationRepository
{
    public async Task<ResumeGenerationOperation?> GetByReservationIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return await dbContext.ResumeGenerationOperations
            .SingleOrDefaultAsync(
                o => o.ReservationId == reservationId,
                cancellationToken);
    }
}
