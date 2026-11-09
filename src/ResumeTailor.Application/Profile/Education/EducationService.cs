
using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Domain.Common.Exceptions;
using EducationEntity = ResumeTailor.Domain.Profile.LibraryEducation;


namespace ResumeTailor.Application.Profile.Education;

public class EducationService(IAccountRepository accountRepository, IEducationRepository educationRepository) : IEducationService
{
    public async Task<IReadOnlyCollection<LibraryEducationResponse>> GetListAsync(string Auth0UserId, bool archived, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when fetching education.");

        return await educationRepository.GetListByAccountIdAsync(accountId, archived, cancellationToken);
    }

    public async Task CreateAsync(string Auth0UserId, EducationRequest request, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when creating education.");

        var education = MapToDomain(request, accountId);

        educationRepository.Add(education);
        await educationRepository.SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(string Auth0UserId, int id, EducationRequest request, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when updating education.");

        var education = await educationRepository.GetForUpdatingByIdAsync(accountId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Education with Id {id} was not found during updating.");

        CheckAuthorizationForEducation(education.AccountId, accountId);

        education.Update(
            institutionName: request.InstitutionName,
            qualification: request.Qualification,
            fieldOfStudy: request.FieldOfStudy,
            gpa: request.Gpa,
            location: request.Location,
            started: request.Started,
            ended: request.Ended,
            useForResume: request.UseForResume);
        

        await educationRepository.SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(string Auth0UserId, int id, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new NotFoundException($"Account was not found when deleting education.");

        var education = await educationRepository.GetForUpdatingByIdAsync(id)
            ?? throw new DirectoryNotFoundException($"Education with Id {id} was not found during deletion.");

        CheckAuthorizationForEducation(education.AccountId, accountId);

        var isUsed = await educationRepository.IsUsedInResumeAsync(id, cancellationToken);
        if (isUsed)
        {
            throw new EducationInUseException();
        }

        educationRepository.Remove(education);

        await educationRepository.SaveAsync(cancellationToken);
    }

    public async Task ArchiveAsync(string Auth0UserId, int id, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Account was not found when deleting an experience");

        var experience = await educationRepository.GetForUpdatingByIdAsync(id, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Education with Id {id} was not found during archiving.");

        CheckAuthorizationForEducation(experience.AccountId, accountId);

        experience.Archive();
        await educationRepository.SaveAsync(cancellationToken);
    }

    public async Task RestoreAsync(string Auth0UserId, int id, CancellationToken cancellationToken = default)
    {
        var accountId = await accountRepository.GetAccountIdByAuth0UserAsync(Auth0UserId, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Account was not found when deleting an experience");

        var experience = await educationRepository.GetForUpdatingByIdAsync(id, cancellationToken)
            ?? throw new DirectoryNotFoundException($"Education with Id {id} was not found during restoring.");

        CheckAuthorizationForEducation(experience.AccountId, accountId);

        experience.Restore();
        await educationRepository.SaveAsync(cancellationToken);
    }

    private static void CheckAuthorizationForEducation(int educationAccountId, int authAccountId)
    {
        if (educationAccountId != authAccountId)
        {
            throw new UnauthorizedAccessException("Experience does not belong to this account.");
        }
    }

    private static EducationEntity MapToDomain(EducationRequest request, int accountId) => new EducationEntity
    (
        accountId: accountId,
        type: request.Type,
        institutionName: request.InstitutionName,
        qualification: request.Qualification,
        fieldOfStudy: request.FieldOfStudy,
        gpa: request.Gpa,
        location: request.Location,
        started: request.Started,
        ended: request.Ended,
        useForResume: request.UseForResume
    );
}
