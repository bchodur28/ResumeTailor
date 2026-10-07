using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.Resumes.Generation.Models;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeCreator
{
    Task<int> CreateResumeAsync(int accountId, ResumeAiGenerationResult result, CancellationToken cancellationToken = default);
}
