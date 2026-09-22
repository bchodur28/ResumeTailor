
using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Education.Interfaces;
using EducationEntity = ResumeTailor.Domain.Profile.Education;


namespace ResumeTailor.Application.Profile.Education;

public class EducationService(IAccountRepository accountRepository, IEducationRepository educationRepository) : IEducationService
{
    public async Task<IReadOnlyCollection<EducationResponse>> GetEducationAsync(string Auth0UserId, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when fetching education.");

        var educations = await educationRepository.GetEducationByAccountIdAsync(accountId, cancellationToken);

        return educations.Select(MapToReponse).ToList();
    }

    public async Task CreateEducationAsync(string Auth0UserId, IReadOnlyCollection<EducationRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when creating education.");

        var education = requests
            .Select(r => MapToDomain(r, accountId))
            .ToList();

        educationRepository.AddRange(education);
        await educationRepository.SaveAsync(cancellationToken);
    }

    public async Task UpdateEducatonAsync(string Auth0UserId, IReadOnlyCollection<EducationRequest> requests, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when updating education.");

        var existingEducations = await educationRepository.GetEducationForUpdatingByAccountIdAsync(accountId, cancellationToken);
        var educationById = existingEducations.ToDictionary(e => e.Id);

        foreach (var request in requests)
        {
            if(!educationById.TryGetValue(request.Id!.Value, out var education))
            {
                throw new NotFoundException($"Education with Id {request.Id} was not found when updating.");
            }

            education.Update(request.SchoolName, request.Degree, request.Major, request.Started, request.Ended, request.UseForResume);
        }

        await educationRepository.SaveAsync(cancellationToken);
    }

    public async Task DeleteEducationAsync(string Auth0UserId, IReadOnlyCollection<int> educationIds, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when deleting education.");

        var existingEducations = await educationRepository.GetEducationForUpdatingByAccountIdAsync(accountId, cancellationToken);
        var educationById = existingEducations.ToDictionary(e => e.Id);
        var educationsToDelete = new List<EducationEntity>();

        foreach(var educationId in educationIds)
        {
            if (!educationById.TryGetValue(educationId, out var education))
            {
                throw new NotFoundException($"Education with Id {educationId} was not found when deleting.");
            }
            educationsToDelete.Add(education);
        }

        educationRepository.RemoveRange(educationsToDelete);

        await educationRepository.SaveAsync(cancellationToken);
    }

    private static EducationResponse MapToReponse(EducationEntity education) => new EducationResponse
    (
        education.Id,
        education.SchoolName,
        education.Degree,
        education.Major,
        education.Started,
        education.Ended,
        education.UseForResume
    );

    private static EducationEntity MapToDomain(EducationRequest request, int accountId) => new EducationEntity
    (
        accountId,
        request.SchoolName,
        request.Degree,
        request.Major,
        request.Started,
        request.Ended,
        request.UseForResume
    );
}
