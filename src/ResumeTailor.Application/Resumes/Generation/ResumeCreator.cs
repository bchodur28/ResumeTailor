using ResumeTailor.Application.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Library.Interfaces;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Generation.Interfaces;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.Resumes.Content;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Application.Resumes.Generation;

public sealed class ResumeCreator(IEducationRepository educationRepository, IResumeRepository resumeRepository) : IResumeCreator
{

    public async Task<int> CreateResumeAsync(int accountId, AiResult<AiResumeDataOutput> aiResumeData, IReadOnlyCollection<ResolvedResumeExperience> resolvedExperiences, CancellationToken cancellationToken = default)
    {
        var educationIds = await educationRepository.GetEducationIdsForCreationAsync(accountId, cancellationToken);
        var resume = BuildResume(accountId, aiResumeData, resolvedExperiences, educationIds, cancellationToken);

        await resumeRepository.CreateResumeAsync(resume, cancellationToken);
        await resumeRepository.SaveAsync(cancellationToken);

        return resume.Id;
    }

    private static Resume BuildResume(int accountId, AiResult<AiResumeDataOutput> aiResumeData, IReadOnlyCollection<ResolvedResumeExperience> resolvedExperience, IEnumerable<int> educationIds, CancellationToken cancellationToken = default)
    {
        var jobPosting = CreateJobPosting(aiResumeData.Value.JobPostingData);
        var metaData = CreateAiMetaData(aiResumeData.MetaData);
        var aiScore = aiResumeData.Value.JobMatchAnalysis.AiScore;

        var resume = new Resume(accountId, CreateResumeName(aiResumeData.Value.JobPostingData), aiScore, metaData, jobPosting);
        AddEducationSelections(educationIds, resume);
        AddExperienceWithBullets(resolvedExperience, resume);
        SetAiAnalysis(aiResumeData.Value.JobMatchAnalysis, resume);

        return resume;
    }

    private static ResumeAiMetaData CreateAiMetaData(AiMetaData aiMetaData)
    {
        return new ResumeAiMetaData(
            model: aiMetaData.Model,
            inputTokens: aiMetaData.InputTokens,
            outputTokens: aiMetaData.OutputTokens,
            totalTokens: aiMetaData.TotalTokens,
            cost: aiMetaData.Cost);
    }

    private static ResumeJobPosting CreateJobPosting(AiJobPostingDataOutput aiJobPosting)
    {
        return new ResumeJobPosting(
            companyName: aiJobPosting.CompanyName,
            jobTitle: aiJobPosting.JobTitle,
            location: aiJobPosting.Location,
            workStyle: aiJobPosting.WorkStyle,
            salaryMin: aiJobPosting.SalaryMin,
            salaryMax: aiJobPosting.SalaryMax,
            salary: aiJobPosting.Salary,
            salaryPeriod: aiJobPosting.SalaryPeriod,
            salaryCurrency: aiJobPosting.SalaryCurrency);
    }

    private static void SetAiAnalysis(AiJobMatchAnalysisOutput jobMatchAnalysis, Resume resume)
    {
        resume.SetAiAnalysis(new ResumeAiAnalysis(summary: jobMatchAnalysis.Summary));

        foreach (var strength in jobMatchAnalysis.Strengths)
        {
            resume.AiAnalysis?.AddInsight(ResumeAiInsightType.Strength, strength);
        }

        foreach (var weakness in jobMatchAnalysis.Weaknesses)
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

    private static void AddExperienceWithBullets(IReadOnlyCollection<ResolvedResumeExperience> resolvedExperiences, Resume resume)
    {
        var experienceOrder = 1;

        foreach (var experience in resolvedExperiences)
        {
            var resumeExperience = resume.AddExperience(experience.ExperienceId, experience.SummaryId, experienceOrder++);

            var bulletOrder = 1;

            foreach (var bullet in experience.Bullets)
            {
                resumeExperience.AddBullet(bullet.BulletId, bullet.Alternative, bullet.Reasoning, bulletOrder++);
            }
        }
    }

    private static string CreateResumeName(AiJobPostingDataOutput aiJobPosting)
    {
        var companyName = aiJobPosting.CompanyName?.Trim();
        var jobTitle = aiJobPosting.JobTitle?.Trim();
        var location = aiJobPosting.Location?.Trim();

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

        if (!string.IsNullOrWhiteSpace(location))
        {
            return $"{location} - {Random.Shared.Next(1000, 10000)}";
        }

        return $"Resume - {Random.Shared.Next(1000, 10000)}";
    }
}
