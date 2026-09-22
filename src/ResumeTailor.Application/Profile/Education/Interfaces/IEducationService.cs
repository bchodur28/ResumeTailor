using ResumeTailor.Application.Contracts.Education;

namespace ResumeTailor.Application.Profile.Education.Interfaces;

public interface IEducationService
{
    Task<IReadOnlyCollection<EducationResponse>> GetEducationAsync(string Auth0UserId, CancellationToken cancellationToken = default);
    Task CreateEducationAsync(string Auth0UserId, IReadOnlyCollection<EducationRequest> requests, CancellationToken cancellationToken = default);
    Task UpdateEducatonAsync(string Auth0UserId, IReadOnlyCollection<EducationRequest> requests, CancellationToken cancellationToken = default);
    Task DeleteEducationAsync(string Auth0UserId, IReadOnlyCollection<int> educationIds, CancellationToken cancellationToken = default);
}
