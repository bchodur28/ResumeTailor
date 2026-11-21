using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Domain.Resumes.Content;
using System.Text.Json;


namespace ResumeTailor.Infrastructure.Services
{
    public class ResumeFinalizationService(IServiceScopeFactory scopeFactory) : IResumeFinalizationService
    {
        private const int MaxAttempts = 3;

        public async Task<int> FinalizeAsync(
            int accountId,
            int reservationId,
            CancellationToken cancellationToken = default)
        {
            for (var attempt = 1; attempt <= MaxAttempts; attempt++)
            {
                try
                {
                    return await FinalizeOnceAsync(
                        accountId,
                        reservationId,     
                        cancellationToken);
                }
                catch (DbUpdateConcurrencyException) when (attempt < MaxAttempts)
                {
                    
                }
            }

            throw new InvalidOperationException("Unable to finalize resume after multiple attempts.");
        }

        private async Task<int> FinalizeOnceAsync(int accountId, int reservationId, CancellationToken cancellationToken)
        {
            await using var scope = scopeFactory.CreateAsyncScope();

            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var walletRepository = scope.ServiceProvider.GetRequiredService<ICreditWalletRepository>();

            var reservationRepository = scope.ServiceProvider.GetRequiredService<ICreditReservationRepository>();

            var operationRepository = scope.ServiceProvider.GetRequiredService<IResumeGenerationOperationRepository>();

            var resumeCreator = scope.ServiceProvider.GetRequiredService<IResumeCreator>();

            await using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);

            var reservation = await reservationRepository.GetByIdAsync(reservationId, cancellationToken)
                ?? throw new InvalidOperationException("Reservation not found.");

            var operation = await operationRepository.GetByReservationIdAsync(reservationId, cancellationToken)
                ?? throw new InvalidOperationException("Generation operation not found.");

            if (operation.AccountId != accountId || operation.ReservationId != reservation.Id)
            {
                throw new InvalidOperationException("Generation operation does not match the account or reservation.");
            }

            // Prevent duplicate finalization.
            // Prevent duplicate finalization.
            if (reservation.Status == CreditReservationStatus.Completed)
            {
                if (operation.Status != ResumeGenerationStatus.Completed || operation.ResumeId != reservation.ResumeId)
                {
                    throw new InvalidOperationException("Reservation and generation operation are inconsistent.");
                }

                return reservation.ResumeId
                    ?? throw new InvalidOperationException("Completed reservation has no resume.");
            }

            if (reservation.Status != CreditReservationStatus.Pending || operation.Status != ResumeGenerationStatus.ReadyToFinalize)
            {
                throw new InvalidOperationException("Generation operation is not ready to finalize.");
            }

            

            var wallet = await walletRepository.GetByAccountIdAsync(accountId, cancellationToken)
                ?? throw new DirectoryNotFoundException("Wallet not found.");

            if (reservation.WalletId != wallet.Id)
            {
                throw new InvalidOperationException("Reservation does not belong to this wallet.");
            }

            // Retrieve the saved generation data
            if (string.IsNullOrWhiteSpace(operation.AiResultJson))
            {
                throw new InvalidOperationException("Generation operation has no saved AI result.");
            }

            var payload = JsonSerializer.Deserialize<ResumeFinalizationPayload>(operation.AiResultJson)
                ?? throw new InvalidOperationException("Could not deserialize resume finalization payload.");

            // Create the resume using the restored data
            var resumeId = await resumeCreator.CreateResumeAsync(
                accountId,
                payload.AiResult,
                payload.ResolvedExperiences,
                payload.ResumeSummary,
                payload.AiSummary,
                cancellationToken);

            wallet.SpendReservedCreditsForResume(amount: reservation.Amount, resumeId: resumeId);

            reservation.Complete(resumeId);
            operation.Complete(resumeId);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            try
            {
                await transaction.CommitAsync(cancellationToken);
            } catch (Exception ex) when (ex is not OperationCanceledException)
            {
                // The commit may have succeeded even if SQL Server
                // failed to return confirmation.
                int? completedResumeId = null;

                try
                {
                    completedResumeId = await GetCompletedResumeIdAsync(reservationId, CancellationToken.None);
                }
                catch
                {
                    // Verification also failed.
                    // Preserve the original commit exception.
                }

                if (completedResumeId.HasValue)
                {
                    return completedResumeId.Value;
                }

                // Outcome is not confirmed. Do not release credits here.
                throw;
            }

            return resumeId;

        }

        private async Task<int?> GetCompletedResumeIdAsync(int reservationId, CancellationToken cancellationToken)
        {
            await using var scope = scopeFactory.CreateAsyncScope();

            var reservationRepository = scope.ServiceProvider
                .GetRequiredService<ICreditReservationRepository>();

            return await reservationRepository.GetCompletedResumeIdAsync(
                reservationId,
                cancellationToken);
        }
    }
}
