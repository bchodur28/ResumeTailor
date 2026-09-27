using ResumeTailor.Application.Contracts.Resume;

namespace ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;

public interface IResumeGeneratorService
{
    Task<int> GenerateResumeDetailsAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default);
}
