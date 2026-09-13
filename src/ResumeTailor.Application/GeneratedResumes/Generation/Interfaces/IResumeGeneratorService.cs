using ResumeTailor.Application.GeneratedResumes.Common.Models;

namespace ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;

public interface IResumeGeneratorService
{
    Task<GeneratedResumeDetailsResponse> GenerateResumeReviewAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default);
}
