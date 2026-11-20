using ResumeTailor.Application.Contracts.Resume;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeGenerator
{
    Task<int> GenerateResumeAsync(string auth0UserId, GeneratedResumeRequest request, CancellationToken cancellationToken = default);
}
