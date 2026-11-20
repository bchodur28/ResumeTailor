using ResumeTailor.Application.Contracts.Experiences;

namespace ResumeTailor.Application.Library.Interfaces;

public interface ILibraryExperienceService
{
    public Task<IReadOnlyCollection<LibraryExperienceSummaryResponse>> GetExperienceSummariesAsync(string auth0UserId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<LibraryExperienceResponse>> GetExperiencesAsync(string auth0UserId, CancellationToken cancellationToken = default);
    public Task<IReadOnlyCollection<LibraryExperienceListItemResponse>> GetExperienceListItemsAsync(string auth0UserId, bool archived, CancellationToken cancellationToken = default);
    public Task<LibraryExperienceResponse> GetExperienceAsync(string auth0UserId, int experienceId, CancellationToken cancellationToken = default);
    public Task<int> CreateExperienceAsync(string auth0UserId, LibraryExperienceRequest request, CancellationToken cancellationToken = default);
    public Task UpdateExperienceAsync(string auth0UserId, int experienceId, LibraryExperienceRequest request, CancellationToken cancellationToken = default);
    public Task DeleteExperienceAsync(string auth0UserId, int experienceId, CancellationToken cancellationToken = default);
    public Task ArchiveExperienceAsync(string auth0UserId, int experienceId, CancellationToken cancellationToken = default);
    public Task RestoreExperienceAsync(string auth0UserId, int experienceId, CancellationToken cancellationToken = default);

}
