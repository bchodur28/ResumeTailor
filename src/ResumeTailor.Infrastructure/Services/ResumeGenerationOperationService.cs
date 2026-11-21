using Microsoft.EntityFrameworkCore;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Infrastructure.Persistence;


namespace ResumeTailor.Infrastructure.Services;

public class ResumeGenerationOperationService(IDbContextFactory<ResumeTailorDbContext> dbContextFactory) : IResumeGenerationOperationService
{
    public async Task MarkReadyAsync(
        int operationId,
        Guid leaseToken,
        string resultJson,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext =
            await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var operation = await dbContext.ResumeGenerationOperations
            .SingleAsync(
                o => o.Id == operationId,
                cancellationToken);

        operation.MarkReadyToFinalize(leaseToken, resultJson);

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
