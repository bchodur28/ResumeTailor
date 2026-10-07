using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Application.Resumes.Generation;

public sealed class ResumeCreator(
    IExperienceRepository experienceRepository,
    IEducationRepository educationRepository,
    IResumeRepository resumeRepository) : IResumeCreator
{
    public async Task<int> CreateResumeAsync(int accountId, ResumeAiGenerationResult result, CancellationToken cancellationToken = default)
    {
        var educationIds = await educationRepository.GetEducationIdsForCreationAsync(accountId, cancellationToken);
        var projectIds = await experienceRepository.GetProjectIdsForCreationAsync(accountId, cancellationToken);
        var bulletIdMapping = await experienceRepository.GetBulletIdMappingAsync(accountId, cancellationToken);
        var remainingCompanyBullets = await GetRemainingCompanyBullets(accountId, result, cancellationToken);

        var companyBullets = result.CompanyBullets
            .Concat(remainingCompanyBullets)
            .ToList();

        var resume = BuildResume(accountId, result, companyBullets, educationIds, projectIds, bulletIdMapping);

        await resumeRepository.CreateResumeAsync(resume, cancellationToken);
        await resumeRepository.SaveAsync(cancellationToken);

        return resume.Id;
    }

    private async Task<List<CompanyBulletAiResult>> GetRemainingCompanyBullets(int accountId, ResumeAiGenerationResult result, CancellationToken cancellationToken)
    {
        var companyIds = await experienceRepository.GetCompanyIdsByAccountIdAsync(accountId, cancellationToken);
        var remainingCompanyIds = companyIds.Except(result.CompanyBullets.Select(cb => cb.CompanyId)).ToHashSet();

        var remainingCompaniesToAdd = await experienceRepository.GetCompaniesWithBulletsByIds(remainingCompanyIds, cancellationToken);

        var remainingCompanyBullets = remainingCompaniesToAdd
            .SelectMany(company => company.Bullets.Select(bullet =>
                new CompanyBulletAiResult(
                    CompanyId: company.Id,
                    BulletId: bullet.Id,
                    AlternativeValue: null)))
            .ToList();
        return remainingCompanyBullets;
    }

    private static Resume BuildResume(int accountId, ResumeAiGenerationResult result, IReadOnlyList<CompanyBulletAiResult> companyBullets, IEnumerable<int> educationIds, IEnumerable<int> projectIds, IReadOnlyDictionary<int, string> bulletIdMapping)
    {
        var resume = new Resume(accountId, CreateResumeName(result.JobPosting), result.AiScore, MapToAiMetaData(result), MapToJobPosting(result));

        AddEducationSelections(educationIds, resume);
        AddProjectSelections(projectIds, resume);
        AddCompanySelectionsWithBullets(companyBullets, bulletIdMapping, resume);

        SetAiAnalysis(result, resume);
        
        return resume;
    }

    private static ResumeAiMetaData MapToAiMetaData(ResumeAiGenerationResult result)
    {
        return new ResumeAiMetaData(
            model: result.MetaData.Model,
            inputTokens: result.MetaData.InputTokens,
            outputTokens: result.MetaData.OutputTokens,
            totalTokens: result.MetaData.TotalTokens,
            cost: result.MetaData.Cost);
    }

    private static ResumeJobPosting MapToJobPosting(ResumeAiGenerationResult result)
    {
        return new ResumeJobPosting(
            companyName: result.JobPosting.CompanyName,
            jobTitle: result.JobPosting.JobTitle,
            location: result.JobPosting.Location,
            workStyle: result.JobPosting.WorkStyle,
            salaryMin: result.JobPosting.SalaryMin,
            salaryMax: result.JobPosting.SalaryMax,
            salary: result.JobPosting.Salary,
            salaryPeriod: result.JobPosting.SalaryPeriod,
            salaryCurrency: result.JobPosting.SalaryCurrency);
    }

    private static void SetAiAnalysis(ResumeAiGenerationResult result, Resume resume)
    {
        resume.SetAiAnalysis(new ResumeAiAnalysis(summary: result.AiAnalysis.Summary));

        foreach (var strength in result.AiAnalysis.Strengths)
        {
            resume.AiAnalysis?.AddInsight(ResumeAiInsightType.Strength, strength);
        }

        foreach (var weakness in result.AiAnalysis.Weaknesses)
        {
            resume.AiAnalysis?.AddInsight(ResumeAiInsightType.Weakness, weakness);
        }
    }

    private static void AddEducationSelections(IEnumerable<int> educationIds, Resume resume)
    {
        var educationOrder = 1;
        foreach (var educationId in educationIds)
        {
            resume.AddEducationSelection(educationId, educationOrder);
            educationOrder++;
        }
    }

    private static void AddProjectSelections(IEnumerable<int> projectIds, Resume resume)
    {
        var projectOrder = 1;
        foreach (var projectId in projectIds)
        {
            resume.AddProjectSelection(projectId, projectOrder);
            projectOrder++;
        }
    }

    private static void AddCompanySelectionsWithBullets(IReadOnlyList<CompanyBulletAiResult> companyBullets, IReadOnlyDictionary<int, string> bulletIdMapping, Resume resume)
    {
        var companyOrder = 1;
        foreach (var companyGroup in companyBullets.GroupBy(cb => cb.CompanyId))
        {
            var companySelection = resume.AddCompanySelection(companyGroup.Key, companyOrder);

            var bulletOrder = 1;
            foreach (var bullet in companyGroup)
            {
                if (!bulletIdMapping.TryGetValue(bullet.BulletId, out var bulletValue))
                {
                    continue;
                }

                companySelection.AddResumeBullet(
                    sourceBulletId: bullet.BulletId,
                    value: bulletValue,
                    alternativeValue: bullet.AlternativeValue,
                    sortOrder: bulletOrder);

                bulletOrder++;
            }

            companyOrder++;
        }
    }

    private static string CreateResumeName(JobPostingResult jobPosting)
    {
        var companyName = jobPosting.CompanyName?.Trim();
        var jobTitle = jobPosting.JobTitle?.Trim();
        var location = jobPosting.Location?.Trim();

        if (!string.IsNullOrWhiteSpace(companyName) &&
            !string.IsNullOrWhiteSpace(jobTitle))
        {
            return $"{companyName} - {jobTitle}";
        }

        if (!string.IsNullOrWhiteSpace(companyName))
        {
            return companyName;
        }

        if (!string.IsNullOrWhiteSpace(jobTitle) &&
            !string.IsNullOrWhiteSpace(location))
        {
            return $"{jobTitle} - {location} - {Random.Shared.Next(1000, 10000)}";
        }

        if (!string.IsNullOrWhiteSpace(jobTitle))
        {
            return $"{jobTitle} - {Random.Shared.Next(1000, 10000)}";
        }

        if(!string.IsNullOrWhiteSpace(location))
        {
            return $"{location} - {Random.Shared.Next(1000, 10000)}";
        }

        return $"Resume - {Random.Shared.Next(1000, 10000)}";
    }
}
