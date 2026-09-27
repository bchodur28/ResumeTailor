using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.GeneratedResumes;

namespace ResumeTailor.Application.GeneratedResumes.Common.Interfaces;

public interface IResumeDataProvider
{
    Task<ResumeSourceData> GetResumeSourceDataForExistingResumeAsync(Resume resume, CancellationToken cancellationToken = default);
    Task<ResumeSourceData> GetResumeSourceDataForGenerationAsync(int accountId, CancellationToken cancellationToken = default);
}
