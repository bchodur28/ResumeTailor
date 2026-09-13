using ResumeTailor.Application.Accounts.Interfaces;
using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;

namespace ResumeTailor.Application.GeneratedResumes.Common
{
    public sealed class GeneratedResumeDataProvider(
        IGeneratedResumeRepository generatedResumeRepository,
        IAccountRepository accountRepository,
        IExperienceRepository experienceRepository) : IGeneratedResumeDataProvider
    {
        public async Task<ResumeSourceData> GetAsync(
        int generatedResumeId,
        CancellationToken cancellationToken = default)
        {
            var generatedResume =
                await generatedResumeRepository.GetGeneratedResumeAsync(
                    generatedResumeId,
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"Generated resume with ID {generatedResumeId} not found.");

            var account =
                await accountRepository.GetAccountByIdAsync(
                    generatedResume.AccountId,
                    cancellationToken)
                ?? throw new NotFoundException(
                    $"Account with ID {generatedResume.AccountId} not found.");

            var companyIds = generatedResume.CompanySelections
                .Select(x => x.CompanyId)
                .ToHashSet();

            var educationIds = generatedResume.EducationSelections
                .Select(x => x.EducationId)
                .ToHashSet();

            var projectIds = generatedResume.ProjectSelections
            .Select(x => x.ProjectId)
            .ToHashSet();

            var companies =
                await experienceRepository.GetCompaniesByIdsAsync(
                    companyIds,
                    cancellationToken);

            var educations =
                await experienceRepository.GetEducationByIdsAsync(
                    educationIds,
                    cancellationToken);

            var projects =
            await experienceRepository.GetProjectsByIdsAsync(
                projectIds,
                cancellationToken);



            return new ResumeSourceData(
                account,
                generatedResume,
                companies,
                educations,
                projects);
        }
    }
}
