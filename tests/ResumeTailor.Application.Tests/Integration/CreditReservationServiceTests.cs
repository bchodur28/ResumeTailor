using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ResumeTailor.Domain.Common;
using ResumeTailor.Domain.Common.Exceptions;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Resumes;
using ResumeTailor.Infrastructure.Persistence;
using ResumeTailor.Infrastructure.Services;

namespace ResumeTailor.Tests.Integration;

public class CreditReservationServiceTests
{
    private const string ConnectionString =
        "Server=(localdb)\\BrianSQLServer;" +
        "Database=ResumeTailor_IntegrationTests;" +
        "Trusted_Connection=True;" +
        "TrustServerCertificate=True;";


    [Fact]
    public async Task TestDatabase_ShouldBeReady()
    {
        // Prevent accidentally connecting to development.
        var connection = new SqlConnectionStringBuilder(ConnectionString);

        Assert.Equal("ResumeTailor_IntegrationTests", connection.InitialCatalog);

        // Configure the DbContext.
        var options = new DbContextOptionsBuilder<ResumeTailorDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        await using var dbContext = new ResumeTailorDbContext(options);

        // Verify database connection.
        Assert.True(await dbContext.Database.CanConnectAsync());

        // Verify all migrations have been applied.
        var pendingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

        Assert.Empty(pendingMigrations);

        // Verify the credit tables exist and can be queried.
        await dbContext.CreditWallets.AnyAsync();
        await dbContext.ResumeGenerationOperations.AnyAsync();
    }

    [Fact]
    public async Task StartGeneration_ShouldReserveOneCredit()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var service = new CreditReservationService(factory);

        var accountId = await CreateTestAccountAsync(factory);

        var requestId = Guid.NewGuid();
        var requestHash = new string('a', 64);

        // Act
        var result = await service.StartGenerationAsync(
            accountId,
            1,
            requestId,
            requestHash);

        try
        {
            // Assert
            await using var dbContext =
                await factory.CreateDbContextAsync();

            var wallet = await dbContext.CreditWallets
                .AsNoTracking()
                .SingleAsync(w => w.AccountId == accountId);

            Assert.Equal(5, wallet.Balance);
            Assert.Equal(1, wallet.ReservedCredits);
            Assert.Equal(4, wallet.AvailableCredits);

            Assert.Equal(
                ResumeGenerationStatus.Generating,
                result.Status);

            Assert.NotEqual(Guid.Empty, result.LeaseToken);

            var operation = await dbContext.ResumeGenerationOperations
                .SingleAsync(o => o.Id == result.OperationId);

            Assert.Equal(requestId, operation.RequestId);
            Assert.Equal(result.ReservationId, operation.ReservationId);
        } finally
        {
            // Release the reservation even if assertions fail.
            await service.FailGenerationAsync(
                result.OperationId,
                result.LeaseToken);
        }

        // Verify the credit became available again.
        await using var verificationDb =
            await factory.CreateDbContextAsync();

        var restoredWallet = await verificationDb.CreditWallets
            .AsNoTracking()
            .SingleAsync(w => w.AccountId == accountId);

        Assert.Equal(5, restoredWallet.Balance);
        Assert.Equal(0, restoredWallet.ReservedCredits);
        Assert.Equal(5, restoredWallet.AvailableCredits);
    }



    [Fact]
    public async Task StartGeneration_SameRequestId_ShouldNotReserveTwice()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var service = new CreditReservationService(factory);

        var accountId = await CreateTestAccountAsync(factory);

        var requestId = Guid.NewGuid();
        var requestHash = new string('b', 64);

        // Act
        var first = await service.StartGenerationAsync(
            accountId,
            1,
            requestId,
            requestHash);

        try
        {
            var second = await service.StartGenerationAsync(
                accountId,
                1,
                requestId,
                requestHash);

            // Assert: both calls reference the same operation.
            Assert.Equal(first.OperationId, second.OperationId);
            Assert.Equal(first.ReservationId, second.ReservationId);

            // The duplicate request cannot start another generation.
            Assert.Equal(Guid.Empty, second.LeaseToken);

            await using var dbContext =
                await factory.CreateDbContextAsync();

            var wallet = await dbContext.CreditWallets
                .AsNoTracking()
                .SingleAsync(w => w.AccountId == accountId);

            // Only one credit was reserved.
            Assert.Equal(5, wallet.Balance);
            Assert.Equal(1, wallet.ReservedCredits);

            var operationCount =
                await dbContext.ResumeGenerationOperations.CountAsync(
                    o => o.AccountId == accountId &&
                         o.RequestId == requestId);

            Assert.Equal(1, operationCount);
        } finally
        {
            await service.FailGenerationAsync(
                first.OperationId,
                first.LeaseToken);
        }
    }


    [Fact]
    public async Task StartGeneration_InsufficientCredits_ShouldRejectReservation()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var service = new CreditReservationService(factory);

        // Create an account with no credits.
        var accountId = await CreateTestAccountAsync(factory, credits: 0);

        var requestId = Guid.NewGuid();
        var requestHash = new string('c', 64);

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientCreditsException>(
            () => service.StartGenerationAsync(
                accountId,
                1,
                requestId,
                requestHash));

        // Verify the database was not modified.
        await using var dbContext = await factory.CreateDbContextAsync();

        var wallet = await dbContext.CreditWallets
            .AsNoTracking()
            .SingleAsync(w => w.AccountId == accountId);

        Assert.Equal(0, wallet.Balance);
        Assert.Equal(0, wallet.ReservedCredits);

        var reservationCount = await dbContext.CreditReservations
            .CountAsync(r => r.RequestId == requestId);

        var operationCount = await dbContext.ResumeGenerationOperations
            .CountAsync(o => o.AccountId == accountId &&
                             o.RequestId == requestId);

        Assert.Equal(0, reservationCount);
        Assert.Equal(0, operationCount);
    }



    [Fact]
    public async Task StartGeneration_ConcurrentRequests_ShouldNotOverspend()
    {
        // Arrange
        using var provider = CreateTestServices();

        var factory = provider.GetRequiredService<
            IDbContextFactory<ResumeTailorDbContext>>();

        var service = new CreditReservationService(factory);

        // Only one credit available.
        var accountId = await CreateTestAccountAsync(factory, credits: 1);

        var requestHash = new string('d', 64);

        // Attempt a generation and capture success or failure.
        async Task<(int? OperationId, Guid LeaseToken, Exception? Error)>
            TryStartAsync(Guid requestId)
        {
            try
            {
                var result = await service.StartGenerationAsync(
                    accountId,
                    1,
                    requestId,
                    requestHash);

                return (result.OperationId, result.LeaseToken, null);
            } catch (Exception ex)
            {
                return (null, Guid.Empty, ex);
            }
        }

        // Act: start two requests concurrently.
        var results = await Task.WhenAll(
            TryStartAsync(Guid.NewGuid()),
            TryStartAsync(Guid.NewGuid()));

        try
        {
            // Exactly one request should succeed.
            var successful = Assert.Single(
                results.Where(r => r.OperationId.HasValue));

            var failed = Assert.Single(
                results.Where(r => r.Error != null));

            Assert.IsType<InsufficientCreditsException>(failed.Error);

            // Verify the actual wallet state in SQL Server.
            await using var dbContext =
                await factory.CreateDbContextAsync();

            var wallet = await dbContext.CreditWallets
                .AsNoTracking()
                .SingleAsync(w => w.AccountId == accountId);

            Assert.Equal(1, wallet.Balance);
            Assert.Equal(1, wallet.ReservedCredits);
            Assert.Equal(0, wallet.AvailableCredits);

            var operationCount =
                await dbContext.ResumeGenerationOperations
                    .CountAsync(o => o.AccountId == accountId);

            Assert.Equal(1, operationCount);
        } finally
        {
            // Release any successfully reserved credit.
            foreach (var result in results.Where(r => r.OperationId.HasValue))
            {
                await service.FailGenerationAsync(
                    result.OperationId!.Value,
                    result.LeaseToken);
            }
        }

        // Verify the wallet is restored.
        await using var verificationDb =
            await factory.CreateDbContextAsync();

        var restored = await verificationDb.CreditWallets
            .AsNoTracking()
            .SingleAsync(w => w.AccountId == accountId);

        Assert.Equal(1, restored.Balance);
        Assert.Equal(0, restored.ReservedCredits);
        Assert.Equal(1, restored.AvailableCredits);
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

        return services.BuildServiceProvider();
    }

    private static async Task<int> CreateTestAccountAsync(
    IDbContextFactory<ResumeTailorDbContext> factory,
    int credits = 5)
    {
        await using var dbContext =
            await factory.CreateDbContextAsync();

        // Unique values so repeated test runs don't conflict.
        var uniqueId = Guid.NewGuid().ToString("N");

        var account = new Account(
            auth0UserId: $"test|{uniqueId}",
            email: $"test-{uniqueId}@example.com",
            displayName: "Integration Test Account",
            location: "Test",
            dateFormat: ResumeDateFormat.MMM_YYYY,
            summary: null);

        // Your Account constructor already creates the wallet.
        if (credits > 0)
        {
            account.CreditWallet.GrantPromotionalCredits(credits);
        }

        dbContext.Accounts.Add(account);

        await dbContext.SaveChangesAsync();

        return account.Id;
    }


}
