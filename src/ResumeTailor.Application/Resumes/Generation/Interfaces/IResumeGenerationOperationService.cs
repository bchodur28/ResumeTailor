namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeGenerationOperationService
{
    Task MarkReadyAsync(int operationId, Guid leaseToken, string resultJson, CancellationToken cancellationToken = default);
}
