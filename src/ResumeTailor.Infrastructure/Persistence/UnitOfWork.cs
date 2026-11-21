using ResumeTailor.Application.Common.Interfaces;


namespace ResumeTailor.Infrastructure.Persistence;

public class UnitOfWork(ResumeTailorDbContext dbContext) : IUnitOfWork
{
    public async Task<IDbTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        return new EfDbTransaction(transaction);
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
