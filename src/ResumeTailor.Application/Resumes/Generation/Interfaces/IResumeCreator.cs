using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Resumes.Content;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeCreator
{
    Task<int> CreateResumeAsync(int accountId, AiResult<AiResumeDataOutput> aiResumeData, IReadOnlyCollection<ResolvedResumeExperience> ResolvedExperiences, string? summary, string? aiSummary, CancellationToken cancellationToken = default);
}
