
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Application.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Resumes.Generation;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Infrastructure.Persistence;
using ResumeTailor.Infrastructure.Persistence.Repositories.Accounts;
using ResumeTailor.Infrastructure.Persistence.Repositories.GeneratedResumes;
using ResumeTailor.Infrastructure.Persistence.Repositories.Profile;
using ResumeTailor.Infrastructure.Persistence.Repositories.Resumes;
using ResumeTailor.Infrastructure.Services;
using System.Text.Json;

namespace ResumeTailor.Tests.Integration;

[Collection("CreditIntegration")]
public class CreditReservationRecoveryServiceTests
{
    private const string ConnectionString =
        "Server=(localdb)\\BrianSQLServer;" +
        "Database=ResumeTailor_IntegrationTests;" +
        "Trusted_Connection=True;" +
        "TrustServerCertificate=True;";

    [Fact]
    public async Task Recover_ReadyToFinalize_ShouldCompleteGeneration()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var recoveryService = provider.GetRequiredService<
            ICreditReservationRecoveryService>();

        // Create a generation with a saved AI result,
        // but do not finalize it.
        var (accountId, reservationId) =
            await CreateReadyGenerationAsync(provider);

        // Act
        await recoveryService.RecoverAsync();

        // Run recovery again to verify it cannot charge twice.
        await recoveryService.RecoverAsync();

        // Assert
        await using var db = await factory.CreateDbContextAsync();

        var resume = await db.Resumes
            .AsNoTracking()
            .SingleAsync(r => r.AccountId == accountId);

        var wallet = await db.CreditWallets
            .AsNoTracking()
            .SingleAsync(w => w.AccountId == accountId);

        Assert.Equal(4, wallet.Balance);
        Assert.Equal(0, wallet.ReservedCredits);
        Assert.Equal(4, wallet.AvailableCredits);

        var reservation = await db.CreditReservations
            .AsNoTracking()
            .SingleAsync(r => r.Id == reservationId);

        Assert.Equal(
            CreditReservationStatus.Completed,
            reservation.Status);

        Assert.Equal(resume.Id, reservation.ResumeId);

        var operation = await db.ResumeGenerationOperations
            .AsNoTracking()
            .SingleAsync(o => o.ReservationId == reservationId);

        Assert.Equal(
            ResumeGenerationStatus.Completed,
            operation.Status);

        Assert.Equal(resume.Id, operation.ResumeId);

        // Recovery must not create two resumes.
        var resumeCount = await db.Resumes
            .CountAsync(r => r.AccountId == accountId);

        Assert.Equal(1, resumeCount);

        // Recovery must not deduct two credits.
        var debitCount = await db.CreditTransactions
            .CountAsync(t =>
                t.ResumeId == resume.Id &&
                t.Amount == -1);

        Assert.Equal(1, debitCount);
    }

    [Fact]
    public async Task Recover_ExpiredGenerating_ShouldNotReleaseCredits()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var reservationService = provider.GetRequiredService<
            ICreditReservationService>();

        var recoveryService = provider.GetRequiredService<
            ICreditReservationRecoveryService>();

        var accountId = await CreateTestAccountAsync(factory);

        var start = await reservationService.StartGenerationAsync(
            accountId,
            1,
            Guid.NewGuid(),
            new string('b', 64));

        try
        {
            // Simulate an abandoned generation by expiring
            // its lease directly in the integration database.
            await using (var updateDb =
                await factory.CreateDbContextAsync())
            {
                var expiredAt = DateTime.UtcNow.AddMinutes(-1);

                await updateDb.ResumeGenerationOperations
                    .Where(o => o.Id == start.OperationId)
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(
                            o => o.LeaseExpiresAt,
                            expiredAt));
            }

            // Act
            await recoveryService.RecoverAsync();

            // Assert
            await using var db =
                await factory.CreateDbContextAsync();

            var wallet = await db.CreditWallets
                .AsNoTracking()
                .SingleAsync(w => w.AccountId == accountId);

            // The credit remains reserved.
            Assert.Equal(5, wallet.Balance);
            Assert.Equal(1, wallet.ReservedCredits);
            Assert.Equal(4, wallet.AvailableCredits);

            var operation = await db.ResumeGenerationOperations
                .AsNoTracking()
                .SingleAsync(o => o.Id == start.OperationId);

            Assert.Equal(
                ResumeGenerationStatus.Generating,
                operation.Status);

            var reservation = await db.CreditReservations
                .AsNoTracking()
                .SingleAsync(r => r.Id == start.ReservationId);

            Assert.Equal(
                CreditReservationStatus.Pending,
                reservation.Status);

            Assert.Null(reservation.ResumeId);

            // No resume was created.
            Assert.False(await db.Resumes.AnyAsync(
                r => r.AccountId == accountId));
        } finally
        {
            // Clean up the intentionally abandoned operation.
            await reservationService.FailGenerationAsync(
                start.OperationId,
                start.LeaseToken);
        }
    }

    private static async Task<(int AccountId, int ReservationId)>
        CreateReadyGenerationAsync(ServiceProvider provider)
    {
        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var reservationService = provider.GetRequiredService<
            ICreditReservationService>();

        var operationService = provider.GetRequiredService<
            IResumeGenerationOperationService>();

        var accountId = await CreateTestAccountAsync(factory);

        // Step 1: Reserve a credit.
        var start = await reservationService.StartGenerationAsync(
            accountId,
            1,
            Guid.NewGuid(),
            new string('a', 64));

        // Step 2: Save the AI result.
        await operationService.MarkReadyAsync(
            start.OperationId,
            start.LeaseToken,
            CreateTestPayloadJson());

        // Deliberately skip finalization.
        // The recovery service should perform it.

        return (accountId, start.ReservationId);
    }

    private static async Task<int> CreateTestAccountAsync(
        IDbContextFactory<ResumeTailorDbContext> factory)
    {
        await using var db = await factory.CreateDbContextAsync();

        var uniqueId = Guid.NewGuid().ToString("N");

        var account = new Account(
            auth0UserId: $"test|{uniqueId}",
            email: $"test-{uniqueId}@example.com",
            displayName: "Integration Test Account",
            location: "Test",
            dateFormat: ResumeDateFormat.MMM_YYYY,
            summary: null);

        account.CreditWallet.GrantPromotionalCredits(5);

        db.Accounts.Add(account);
        await db.SaveChangesAsync();

        return account.Id;
    }

    private static string CreateTestPayloadJson()
    {
        // Same simulated AI payload that already passes
        // your ResumeFinalizationService integration tests.
        var payload = new
        {
            AiResult = new
            {
                MetaData = new
                {
                    Model = "test-model",
                    InputTokens = 100,
                    OutputTokens = 50,
                    TotalTokens = 150,
                    Cost = 0.01m
                },
                Value = new
                {
                    Experiences = Array.Empty<object>(),
                    JobMatchAnalysis = new
                    {
                        Summary = "Integration test analysis",
                        AiScore = 85,
                        Strengths = Array.Empty<string>(),
                        Weaknesses = Array.Empty<string>()
                    },
                    JobPostingData = new
                    {
                        CompanyName = "Test Company",
                        JobTitle = "Software Engineer",
                        Location = "Remote",
                        WorkStyle = (string?)null,
                        SalaryMin = (decimal?)null,
                        SalaryMax = (decimal?)null,
                        Salary = (decimal?)null,
                        SalaryPeriod = (string?)null,
                        SalaryCurrency = (string?)null
                    },
                    ResumeSummary = (string?)null
                }
            },
            ResolvedExperiences = Array.Empty<object>(),
            ResumeSummary = (string?)null,
            AiSummary = (string?)null
        };

        return JsonSerializer.Serialize(payload);
    }

    private static ServiceProvider CreateTestServices()
    {
        var connection = new SqlConnectionStringBuilder(ConnectionString);

        if (connection.InitialCatalog != "ResumeTailor_IntegrationTests")
        {
            throw new InvalidOperationException(
                "Tests must use the integration database.");
        }

        var services = new ServiceCollection();

        services.AddDbContextFactory<ResumeTailorDbContext>(
            options => options.UseSqlServer(ConnectionString));

        // Required for ILogger<CreditReservationRecoveryService>.
        services.AddLogging();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<
            ICreditWalletRepository,
            CreditWalletRepository>();

        services.AddScoped<
            ICreditReservationRepository,
            CreditReservationRepository>();

        services.AddScoped<
            IResumeGenerationOperationRepository,
            ResumeGenerationOperationRepository>();

        services.AddScoped<IEducationRepository, EducationRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();

        services.AddScoped<IResumeCreator, ResumeCreator>();

        services.AddScoped<
            ICreditReservationService,
            CreditReservationService>();

        services.AddScoped<
            IResumeGenerationOperationService,
            ResumeGenerationOperationService>();

        services.AddScoped<
            IResumeFinalizationService,
            ResumeFinalizationService>();

        services.AddScoped<
            ICreditReservationRecoveryService,
            CreditReservationRecoveryService>();

        return services.BuildServiceProvider();
    }
}
