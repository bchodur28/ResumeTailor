using ResumeTailor.Application.Common.Exceptions;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.GeneratedResumes;

namespace ResumeTailor.Application.GeneratedResumes.Common
{
    public sealed class ResumeDataProvider(
        IAccountRepository accountRepository,
        IEducationRepository educationRepository,
        IExperienceRepository experienceRepository) : IResumeDataProvider
    {
        public async Task<ResumeSourceData> GetResumeSourceDataForExistingResumeAsync(GeneratedResume resume, CancellationToken cancellationToken = default)
        {
            
            var account = await accountRepository.GetAccountByIdAsync(resume.AccountId, cancellationToken)
                ?? throw new NotFoundException($"Account with ID {resume.AccountId} not found.");

            var companyIds = resume.CompanySelections
                .Select(x => x.CompanyId)
                .ToHashSet();

            var educationIds = resume.EducationSelections
                .Select(x => x.EducationId)
                .ToHashSet();

            var projectIds = resume.ProjectSelections
                .Select(x => x.ProjectId)
                .ToHashSet();

            var companies = await experienceRepository.GetCompaniesByIdsAsync(companyIds, cancellationToken);
            var educations = await educationRepository.GetEducationByIdsAsync(educationIds, cancellationToken);
            var projects = await experienceRepository.GetProjectsByIdsAsync(projectIds, cancellationToken);

            return new ResumeSourceData(account, companies, educations, projects);
        }

        public async Task<ResumeSourceData> GetResumeSourceDataForGenerationAsync(int accountId, CancellationToken cancellationToken = default)
        {
            

            var account = await accountRepository.GetAccountByIdAsync(accountId, cancellationToken)
                ?? throw new NotFoundException($"Account with ID {accountId} not found.");

            var companies = await experienceRepository.GetCompaniesWithBulletsByAccountIdAsync(account.Id, cancellationToken);
            var educations = await educationRepository.GetEducationByAccountIdAsync(account.Id, cancellationToken);
            var projects = await experienceRepository.GetProjectsByAccountIdAsync(account.Id, cancellationToken);

            return new ResumeSourceData(account, companies, educations, projects);
        }
    }
}
