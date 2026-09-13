

using ResumeTailor.Application.GeneratedResumes.Common.Models;

namespace ResumeTailor.Application.GeneratedResumes.Common.Interfaces;

public interface IGeneratedResumeDataProvider
{
    Task<ResumeSourceData> GetAsync(
        int jobApplicationId,
        CancellationToken cancellationToken = default);
}
