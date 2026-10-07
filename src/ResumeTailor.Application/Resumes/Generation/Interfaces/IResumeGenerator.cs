using ResumeTailor.Application.Contracts.Resume;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeGenerator
{
    Task<int> GenerateResumeAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default);
}
