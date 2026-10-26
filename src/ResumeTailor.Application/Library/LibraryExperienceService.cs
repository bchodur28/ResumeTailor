using ResumeTailor.Application.Contracts.Experiences;
using ResumeTailor.Application.Library.Interfaces;
using ResumeTailor.Application.Mappers;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.Library;

public sealed class LibraryExperienceService(
    ILibraryExperienceRepository experienceRepository,
    IAccountRepository accountRepository) : ILibraryExperienceService
{
    public async Task<IReadOnlyCollection<LibraryExperienceSummaryResponse>> GetExperienceSummariesAsync(string Auth0UserId, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Account was not found when fetching experience summaries");

        return await experienceRepository.GetSummeriesByAccountIdAsync(accountId, cancellationToken);
    }

    public async Task<LibraryExperienceResponse> GetExperienceAsync(string Auth0UserId, int experienceId, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Account was not found when creating an experience");

        var experience = await experienceRepository.GetByIdAsync(experienceId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Experience with Id {experienceId} was not found when fetching");

        CheckAuthorizationForExperience(experience.AccountId, accountId);

        return LibraryExperienceMapper.MapEntityToResponse(experience);

    }

    public async Task<int> CreateExperienceAsync(string Auth0UserId, LibraryExperienceRequest request, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Account was not found when creating an experience");

        var experience = LibraryExperienceMapper.MapRequestToEntity(accountId, request);

        AddInitialSummaries(experience, request.Summeries);
        AddInitialBullets(experience, request.Bullets);
        
        experienceRepository.Add(experience);
        await experienceRepository.SaveAync(cancellationToken);

        return experience.Id;
    }

    public async Task UpdateExperienceAsync(string Auth0UserId, int experienceId, LibraryExperienceRequest request, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Account was not found when updating an experience");

        var experience = await experienceRepository.GetForUpdatingByIdAsync(experienceId)
            ?? throw new DirectoryNotFoundException($"Experience with Id {experienceId} was not found during updating.");

        CheckAuthorizationForExperience(experience.AccountId, accountId);

        experience.Update(
            type: request.Type,
            title: request.Title,
            organization: request.Organization,
            location: request.Location,
            started: request.Started,
            ended: request.Ended,
            letAiChooseSummary: request.LetAiChooseSummary,
            link: request.Link,
            techStack: request.TechStack,
            letAiChooseBullets: request.LetAiChooseBullets,
            maxBulletsToChoose: request.MaxBulletsToChoose);

        UpdateSummaries(experience, request.Summeries);
        UpdateBullets(experience, request.Bullets);

        await experienceRepository.SaveAync(cancellationToken);
    }

    public async Task DeleteExperienceAsync(string Auth0UserId, int experienceId, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Account was not found when deleting an experience");

        var experience = await experienceRepository.GetForUpdatingByIdAsync(experienceId)
            ?? throw new DirectoryNotFoundException($"Experience with Id {experienceId} was not found during updating.");

        CheckAuthorizationForExperience(experience.AccountId, accountId);

        experienceRepository.Remove(experience);
        await experienceRepository.SaveAync(cancellationToken);
    }

    private static void CheckAuthorizationForExperience(int experienceAccountId, int authAccountId)
    {
        if (experienceAccountId != authAccountId)
        {
            throw new UnauthorizedAccessException("Experience does not belong to this account.");
        }
    }

    private static void AddInitialBullets(LibraryExperience experience, IReadOnlyCollection<LibraryBulletRequest> requestBullets)
    {
        foreach (var bulletRequest in requestBullets)
        {
            experience.AddBullet(LibraryExperienceMapper.MapBulletRequestToEntity(bulletRequest));
        }
    }

    private static void AddInitialSummaries(LibraryExperience experience, IReadOnlyCollection<LibrarySummaryRequest> requestSummaries)
    {
        foreach (var summaryRequest in requestSummaries)
        {
            var summary = LibraryExperienceMapper.MapSummaryRequestToEntity(summaryRequest);
            experience.AddSummary(summary);

            if (summaryRequest.IsPrimary)
            {
                experience.SetPrimarySummary(summary);
            }
        }
    }

    private static void UpdateSummaries(LibraryExperience experience, IReadOnlyCollection<LibrarySummaryRequest> requestSummaries)
    {
        foreach (var requestSummary in requestSummaries)
        {
            if (requestSummary.Id is null)
            {
                experience.AddSummary(
                    LibraryExperienceMapper.MapSummaryRequestToEntity(requestSummary));
                continue;
            }

            var summary = experience.Summaries
                .FirstOrDefault(s => s.Id == requestSummary.Id)
                ?? throw new DirectoryNotFoundException(
                    $"Summary with Id {requestSummary.Id} was not found.");

            if (requestSummary.IsDeleting)
                experience.RemoveSummary(summary);
            else
                summary.Update(requestSummary.Value);
        }
    }

    private static void UpdateBullets(LibraryExperience experience, IReadOnlyCollection<LibraryBulletRequest> requestbullets)
    {
        foreach (var requestBullet in requestbullets)
        {
            if (requestBullet.Id is null)
            {
                experience.AddBullet(
                    LibraryExperienceMapper.MapBulletRequestToEntity(requestBullet));
                continue;
            }

            var bullet = experience.Bullets
                .FirstOrDefault(s => s.Id == requestBullet.Id)
                ?? throw new DirectoryNotFoundException(
                    $"Bullet with Id {requestBullet.Id} was not found.");

            if (requestBullet.IsDeleting)
                experience.RemoveBullet(bullet);
            else
                bullet.Update(requestBullet.Value);
        }
    }
}
