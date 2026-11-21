
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
public class ResumeFinalizationServiceTests
{
    private const string ConnectionString =
        "Server=(localdb)\\BrianSQLServer;" +
        "Database=ResumeTailor_IntegrationTests;" +
        "Trusted_Connection=True;" +
        "TrustServerCertificate=True;";

    [Fact]
    public async Task Finalize_GenerationNotReady_ShouldNotSpendCredits()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var reservationService =
            provider.GetRequiredService<ICreditReservationService>();

        var finalizationService =
            provider.GetRequiredService<IResumeFinalizationService>();

        var accountId = await CreateTestAccountAsync(factory);

        var start = await reservationService.StartGenerationAsync(
            accountId,
            1,
            Guid.NewGuid(),
            new string('a', 64));

        try
        {
            // Act: attempt finalization before the AI result is ready.
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => finalizationService.FinalizeAsync(
                    accountId,
                    start.ReservationId));

            // Assert
            Assert.Equal(
                "Generation operation is not ready to finalize.",
                exception.Message);

            await using var dbContext =
                await factory.CreateDbContextAsync();

            var wallet = await dbContext.CreditWallets
                .AsNoTracking()
                .SingleAsync(w => w.AccountId == accountId);

            Assert.Equal(5, wallet.Balance);
            Assert.Equal(1, wallet.ReservedCredits);
            Assert.Equal(4, wallet.AvailableCredits);

            var operation = await dbContext.ResumeGenerationOperations
                .AsNoTracking()
                .SingleAsync(o => o.Id == start.OperationId);

            Assert.Equal(
                ResumeGenerationStatus.Generating,
                operation.Status);

            var reservation = await dbContext.CreditReservations
                .AsNoTracking()
                .SingleAsync(r => r.Id == start.ReservationId);

            Assert.Equal(
                CreditReservationStatus.Pending,
                reservation.Status);

            Assert.Null(reservation.ResumeId);
        } finally
        {
            // Restore the test wallet.
            await reservationService.FailGenerationAsync(
                start.OperationId,
                start.LeaseToken);
        }
    }

    [Fact]
    public async Task Finalize_ShouldCreateResumeAndSpendReservedCredit()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var finalizationService = provider.GetRequiredService<
            IResumeFinalizationService>();

        var (accountId, reservationId) =
            await CreateReadyGenerationAsync(provider);

        // Act
        var resumeId = await finalizationService.FinalizeAsync(
            accountId,
            reservationId);

        // Assert
        await using var db = await factory.CreateDbContextAsync();

        // Resume was created.
        var resume = await db.Resumes
            .AsNoTracking()
            .SingleAsync(r => r.Id == resumeId);

        Assert.Equal(accountId, resume.AccountId);

        // Credit was spent, not merely reserved.
        var wallet = await db.CreditWallets
            .AsNoTracking()
            .SingleAsync(w => w.AccountId == accountId);

        Assert.Equal(4, wallet.Balance);
        Assert.Equal(0, wallet.ReservedCredits);
        Assert.Equal(4, wallet.AvailableCredits);

        // Reservation was completed.
        var reservation = await db.CreditReservations
            .AsNoTracking()
            .SingleAsync(r => r.Id == reservationId);

        Assert.Equal(
            CreditReservationStatus.Completed,
            reservation.Status);

        Assert.Equal(resumeId, reservation.ResumeId);

        // Generation operation was completed.
        var operation = await db.ResumeGenerationOperations
            .AsNoTracking()
            .SingleAsync(o => o.ReservationId == reservationId);

        Assert.Equal(
            ResumeGenerationStatus.Completed,
            operation.Status);

        Assert.Equal(resumeId, operation.ResumeId);

        // A debit transaction was recorded.
        var debitCount = await db.CreditTransactions
            .CountAsync(t =>
                t.ResumeId == resumeId &&
                t.Amount == -1);

        Assert.Equal(1, debitCount);
    }

    [Fact]
    public async Task Finalize_CalledTwice_ShouldNotChargeTwice()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var finalizationService = provider.GetRequiredService<
            IResumeFinalizationService>();

        var (accountId, reservationId) =
            await CreateReadyGenerationAsync(provider);

        // Act
        var firstResumeId = await finalizationService.FinalizeAsync(
            accountId,
            reservationId);

        var secondResumeId = await finalizationService.FinalizeAsync(
            accountId,
            reservationId);

        // Assert
        Assert.Equal(firstResumeId, secondResumeId);

        await using var db = await factory.CreateDbContextAsync();

        // Only one resume exists for this account.
        var resumeCount = await db.Resumes
            .CountAsync(r => r.AccountId == accountId);

        Assert.Equal(1, resumeCount);

        // Only one credit was spent.
        var wallet = await db.CreditWallets
            .AsNoTracking()
            .SingleAsync(w => w.AccountId == accountId);

        Assert.Equal(4, wallet.Balance);
        Assert.Equal(0, wallet.ReservedCredits);

        // Only one debit transaction exists.
        var debitCount = await db.CreditTransactions
            .CountAsync(t =>
                t.ResumeId == firstResumeId &&
                t.Amount == -1);

        Assert.Equal(1, debitCount);
    }

    [Fact]
    public async Task Finalize_CompletedReservationFromDifferentAccount_ShouldReject()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var service = provider.GetRequiredService<IResumeFinalizationService>();

        var (accountId, reservationId) =
            await CreateReadyGenerationAsync(provider);

        var resumeId = await service.FinalizeAsync(
            accountId,
            reservationId);

        var otherAccountId = await CreateTestAccountAsync(factory);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
    () => service.FinalizeAsync(
        otherAccountId,
        reservationId));

        Assert.Equal(
            "Generation operation does not match the account or reservation.",
            exception.Message);

        // Verify the original resume still exists.
        await using var db = await factory.CreateDbContextAsync();

        Assert.True(await db.Resumes.AnyAsync(
            r => r.Id == resumeId && r.AccountId == accountId));
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

        // Create an account with five credits.
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

        // Reserve one credit.
        var start = await reservationService.StartGenerationAsync(
            account.Id,
            1,
            Guid.NewGuid(),
            new string('a', 64));

        // Save simulated AI output.
        await operationService.MarkReadyAsync(
            start.OperationId,
            start.LeaseToken,
            CreateTestPayloadJson());

        return (account.Id, start.ReservationId);
    }

    private static string CreateTestPayloadJson()
    {
        // Matches the serialized ResumeFinalizationPayload shape.
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

        // Persistence dependencies used by finalization.
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

        // Register the real education and resume repositories.
        services.AddScoped<IEducationRepository, EducationRepository>();
        services.AddScoped<IResumeRepository, ResumeRepository>();

        // Register the real application and infrastructure services.
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

        return services.BuildServiceProvider();
    }

    private static async Task<int> CreateTestAccountAsync(
        IDbContextFactory<ResumeTailorDbContext> factory)
    {
        await using var dbContext =
            await factory.CreateDbContextAsync();

        var uniqueId = Guid.NewGuid().ToString("N");

        var account = new Account(
            auth0UserId: $"test|{uniqueId}",
            email: $"test-{uniqueId}@example.com",
            displayName: "Integration Test Account",
            location: "Test",
            dateFormat: ResumeDateFormat.MMM_YYYY,
            summary: null);

        account.CreditWallet.GrantPromotionalCredits(5);

        dbContext.Accounts.Add(account);
        await dbContext.SaveChangesAsync();

        return account.Id;
    }
}
