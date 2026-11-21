using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Infrastructure.Persistence;

namespace ResumeTailor.Infrastructure.Services;

public class CreditReservationService(IDbContextFactory<ResumeTailorDbContext> dbContextFactory) : ICreditReservationService
{
    private const int MaxAttempts = 3;

    public async Task<int> ReserveAsync(int accountId, int amount, Guid requestId, CancellationToken cancellationToken = default)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                return await ReserveOnceAsync(accountId, amount, requestId, cancellationToken);
            }
            catch (DbUpdateConcurrencyException) when (attempt < MaxAttempts)
            {
                // Retry with a fresh DbContext.
            }
        }

        throw new InvalidOperationException(
            "Unable to reserve credits after multiple attempts.");
    }

    private async Task<int> ReserveOnceAsync(int accountId, int amount, Guid requestId, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var wallet = await dbContext.CreditWallets
            .SingleOrDefaultAsync(
                w => w.AccountId == accountId,
                cancellationToken)
            ?? throw new DirectoryNotFoundException("Wallet not found.");

        wallet.ReserveCredits(amount);

        var reservation = new CreditReservation(
            walletId: wallet.Id,
            amount: amount,
            requestId: requestId,
            leaseExpiresAt: DateTime.UtcNow.AddMinutes(10));

        dbContext.CreditReservations.Add(reservation);

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return reservation.Id;
    }

    public async Task ReleaseAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                await ReleaseOnceAsync(reservationId, cancellationToken);
                return;
            } catch (DbUpdateConcurrencyException)
                  when (attempt < MaxAttempts)
            {
                // Retry using a fresh DbContext.
            }
        }
    }

    public async Task<GenerationStartResult> StartGenerationAsync(
    int accountId,
    int amount,
    Guid requestId,
    string requestHash,
    CancellationToken cancellationToken = default)
    {
        if (requestId == Guid.Empty)
            throw new ArgumentException("Request ID is required.");

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                return await StartGenerationOnceAsync(
                    accountId,
                    amount,
                    requestId,
                    requestHash,
                    cancellationToken);
            } catch (DbUpdateConcurrencyException)
                  when (attempt < MaxAttempts)
            {
                // Another request updated the wallet.
                // Start a fresh attempt with current database values.
            } catch (DbUpdateException ex)
                  when (ex.InnerException is SqlException sql &&
                        (sql.Number == 2601 || sql.Number == 2627))
            {
                // Another request may have inserted this RequestId.
                // Check using a fresh DbContext.
                var existing = await GetExistingGenerationAsync(
                    accountId,
                    requestId,
                    requestHash,
                    cancellationToken);

                if (existing != null)
                    return existing;

                // Some other unique constraint failed.
                throw;
            }
        }

        throw new InvalidOperationException(
            "Unable to start resume generation.");
    }

    private async Task<GenerationStartResult> StartGenerationOnceAsync(int accountId, int amount, Guid requestId, string requestHash, CancellationToken cancellationToken)
    {
        await using var dbContext =
            await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var existing = await dbContext.ResumeGenerationOperations
            .AsNoTracking()
            .SingleOrDefaultAsync(
                o => o.AccountId == accountId &&
                     o.RequestId == requestId,
                cancellationToken);

        if (existing != null)
        {
            if (existing.RequestHash != requestHash)
            {
                throw new InvalidOperationException("Request ID was already used with different input.");
            }

            return new GenerationStartResult(
                existing.Id,
                existing.ReservationId,
                existing.Status,
                Guid.Empty,
                existing.ResumeId);
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var wallet = await dbContext.CreditWallets
            .SingleOrDefaultAsync(
                w => w.AccountId == accountId,
                cancellationToken)
            ?? throw new DirectoryNotFoundException("Wallet not found.");

        wallet.ReserveCredits(amount);

        var leaseToken = Guid.NewGuid();
        var leaseExpiresAt = DateTime.UtcNow.AddMinutes(10);

        var reservation = new CreditReservation(
            wallet.Id,
            amount,
            requestId,
            leaseExpiresAt);

        dbContext.CreditReservations.Add(reservation);

        await dbContext.SaveChangesAsync(cancellationToken);

        var operation = new ResumeGenerationOperation(
            accountId,
            reservation.Id,
            requestId,
            requestHash,
            leaseToken,
            leaseExpiresAt);

        dbContext.ResumeGenerationOperations.Add(operation);

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new GenerationStartResult(
            operation.Id,
            reservation.Id,
            operation.Status,
            leaseToken,
            null);
    }

    private async Task<GenerationStartResult?> GetExistingGenerationAsync(int accountId, Guid requestId, string requestHash, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var existing = await dbContext.ResumeGenerationOperations
            .AsNoTracking()
            .SingleOrDefaultAsync(
                o => o.AccountId == accountId &&
                     o.RequestId == requestId,
                cancellationToken);

        if (existing == null)
            return null;

        if (existing.RequestHash != requestHash)
        {
            throw new InvalidOperationException(
                "Request ID was already used with different input.");
        }

        return new GenerationStartResult(
            existing.Id,
            existing.ReservationId,
            existing.Status,
            Guid.Empty,
            existing.ResumeId);
    }

    public async Task FailGenerationAsync(int operationId, Guid leaseToken, CancellationToken cancellationToken = default)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                await FailGenerationOnceAsync(
                    operationId,
                    leaseToken,
                    cancellationToken);

                return;
            } catch (DbUpdateConcurrencyException)
                  when (attempt < MaxAttempts)
            {
                // Retry with a fresh DbContext.
            }
        }
    }

    private async Task FailGenerationOnceAsync(int operationId, Guid leaseToken, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var operation = await dbContext.ResumeGenerationOperations
            .SingleAsync(o => o.Id == operationId, cancellationToken);

        if (operation.Status == ResumeGenerationStatus.Failed)
            return;

        if (operation.Status != ResumeGenerationStatus.Generating)
        {
            throw new InvalidOperationException("Generation operation cannot be failed in its current state.");
        }

        if (operation.LeaseToken != leaseToken)
        {
            throw new InvalidOperationException("Generation operation is not owned by this worker.");
        }

        var reservation = await dbContext.CreditReservations
            .SingleAsync(
                r => r.Id == operation.ReservationId,
                cancellationToken);

        if (reservation.Status != CreditReservationStatus.Pending)
        {
            throw new InvalidOperationException("Generation reservation is not pending.");
        }

        var wallet = await dbContext.CreditWallets
            .SingleAsync(
                w => w.Id == reservation.WalletId,
                cancellationToken);

        wallet.ReleaseCredits(reservation.Amount);
        reservation.Release();
        operation.Fail(leaseToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }

    private async Task ReleaseOnceAsync(int reservationId, CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var reservation = await dbContext.CreditReservations
            .SingleAsync(r => r.Id == reservationId, cancellationToken);

        if (reservation.Status != CreditReservationStatus.Pending)
            return;

        var wallet = await dbContext.CreditWallets
            .SingleAsync(
                w => w.Id == reservation.WalletId,
                cancellationToken);

        wallet.ReleaseCredits(reservation.Amount);
        reservation.Release();

        await dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
    }
}
