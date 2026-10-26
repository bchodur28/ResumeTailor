using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Resumes.Content;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeCreator
{
    Task<int> CreateResumeAsync(int accountId, AiResult<AiResumeDataOutput> aiResumeData, IReadOnlyCollection<ResolvedResumeExperience> ResolvedExperiences, CancellationToken cancellationToken = default);
}
