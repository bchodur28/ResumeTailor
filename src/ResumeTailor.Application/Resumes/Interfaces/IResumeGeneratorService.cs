using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Application.Resumes.Interfaces;

public interface IResumeGeneratorService
{
    Task<ResumeGenerationResult> GenerateResumeReviewAsync(int resumeId, string jobDescription, CancellationToken cancellationToken = default);
}
