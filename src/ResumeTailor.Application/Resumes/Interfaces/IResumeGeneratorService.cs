using ResumeTailor.Application.Resumes.Models;

namespace ResumeTailor.Application.Resumes.Interfaces;

public interface IResumeGeneratorService
{
    Task<ResumeResponse> GenerateResumeReviewAsync(int resumeId, string jobDescription);
}
