using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Infrastructure.Persistence;

namespace ResumeTailor.Infrastructure.Services;

public class CreditReservationRecoveryService(
    IDbContextFactory<ResumeTailorDbContext> dbContextFactory,
    IResumeFinalizationService resumeFinalizationService,
    ILogger<CreditReservationRecoveryService> logger
) : ICreditReservationRecoveryService
{
    public async Task RecoverAsync(
        CancellationToken cancellationToken = default)
    {
        await using var dbContext =
            await dbContextFactory.CreateDbContextAsync(cancellationToken);

        // Find generations with saved AI results
        // that still need to be finalized.
        var readyOperations = await dbContext.ResumeGenerationOperations
            .AsNoTracking()
            .Where(o =>
                o.Status == ResumeGenerationStatus.ReadyToFinalize)
            .Select(o => new
            {
                o.Id,
                o.AccountId,
                o.ReservationId
            })
            .Take(50)
            .ToListAsync(cancellationToken);

        foreach (var operation in readyOperations)
        {
            try
            {
                await resumeFinalizationService.FinalizeAsync(
                    operation.AccountId,
                    operation.ReservationId,
                    cancellationToken);

                logger.LogInformation(
                    "Recovered resume generation {OperationId}",
                    operation.Id);
            } catch (OperationCanceledException)
                  when (cancellationToken.IsCancellationRequested)
            {
                throw;
            } catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to recover generation {OperationId}",
                    operation.Id);
            }
        }

        // Detect potentially abandoned generations.
        // Do not automatically release them.
        var expiredOperations = await dbContext.ResumeGenerationOperations
            .AsNoTracking()
            .Where(o =>
                o.Status == ResumeGenerationStatus.Generating &&
                o.LeaseExpiresAt < DateTime.UtcNow)
            .Select(o => new
            {
                o.Id,
                o.AccountId,
                o.RequestId
            })
            .Take(50)
            .ToListAsync(cancellationToken);

        foreach (var operation in expiredOperations)
        {
            logger.LogWarning(
                "Generation {OperationId} for account {AccountId} " +
                "has exceeded its lease. RequestId: {RequestId}",
                operation.Id,
                operation.AccountId,
                operation.RequestId);
        }
    }
}
