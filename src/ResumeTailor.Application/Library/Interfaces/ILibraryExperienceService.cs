using ResumeTailor.Application.Contracts.Experiences;

namespace ResumeTailor.Application.Library.Interfaces;

public interface ILibraryExperienceService
{
    public Task<IReadOnlyCollection<LibraryExperienceSummaryResponse>> GetExperienceSummariesAsync(string Auth0UserId, CancellationToken cancellationToken = default);
    public Task<LibraryExperienceResponse> GetExperienceAsync(string Auth0UserId, int experienceId, CancellationToken cancellationToken = default);
    public Task<int> CreateExperienceAsync(string Auth0UserId, LibraryExperienceRequest request, CancellationToken cancellationToken = default);
    public Task UpdateExperienceAsync(string Auth0UserId, int experienceId, LibraryExperienceRequest request, CancellationToken cancellationToken = default);
    public Task DeleteExperienceAsync(string Auth0UserId, int experienceId, CancellationToken cancellationToken = default);

}
