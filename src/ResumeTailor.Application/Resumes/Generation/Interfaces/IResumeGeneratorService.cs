using ResumeTailor.Application.Contracts.Resume;

namespace ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;

public interface IResumeGeneratorService
{
    Task<ResumeDetailsResponse> GenerateResumeDetailsAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default);
}
