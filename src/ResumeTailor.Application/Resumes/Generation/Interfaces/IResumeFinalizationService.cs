using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.Resumes.Content;

namespace ResumeTailor.Application.Resumes.Generation.Interfaces;

public interface IResumeFinalizationService
{
    Task<int> FinalizeAsync(
        int accountId,
        int reservationId,
        CancellationToken cancellationToken = default);
}
