namespace ResumeTailor.Application.Common.Interfaces;

public interface IDbTransaction : IAsyncDisposable
{
    Task CommitAsync(CancellationToken cancellationToken = default);
}
