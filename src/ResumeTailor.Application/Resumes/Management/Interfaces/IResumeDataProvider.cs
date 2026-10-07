using ResumeTailor.Application.Resumes.Management.Models;
using ResumeTailor.Domain.GeneratedResumes;

namespace ResumeTailor.Application.Resumes.Management.Interfaces;

public interface IResumeDataProvider
{
    Task<ResumeSourceData> GetResumeSourceDataForExistingResumeAsync(Resume resume, CancellationToken cancellationToken = default);
}
