using ResumeTailor.Application.Contracts.Education;

namespace ResumeTailor.Application.Profile.Education.Interfaces;

public interface IEducationService
{
    Task<IReadOnlyCollection<LibraryEducationResponse>> GetListAsync(string Auth0UserId, bool archived, CancellationToken cancellationToken = default);
    Task CreateAsync(string auth0UserId, EducationRequest requests, CancellationToken cancellationToken = default);
    Task UpdateAsync(string auth0UserId, int id, EducationRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(string auth0UserId, int id, CancellationToken cancellationToken = default);
    Task ArchiveAsync(string auth0UserId, int id, CancellationToken cancellationToken = default);
    Task RestoreAsync(string auth0UserId, int id, CancellationToken cancellationToken = default);
}
