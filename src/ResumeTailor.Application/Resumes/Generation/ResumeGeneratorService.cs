using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.GeneratedResumes.Management.Interfaces;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Application.Resumes.Generation.Models;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Resumes.JobPositing;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Application.GeneratedResumes.Generation;

public class ResumeGeneratorService(
    IResumeAiGenerator aiGenerator,
    IResumeDataProvider resumeDataProvider,
    IResumeRepository resumeRepository) : IResumeGeneratorService
{
    public async Task<int> GenerateResumeDetailsAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default)
    {
        var sourceData = await resumeDataProvider.GetResumeSourceDataForGenerationAsync(accountId, cancellationToken);

        var companyBulletContexts = GetCompanyBulletContexts(sourceData.Companies);
        var resumeAiGenerationContext = new ResumeAiGenerationContext(jobDescription, companyBulletContexts);

        var aiGenerationResult = await aiGenerator.GenerateAsync(resumeAiGenerationContext, cancellationToken);

        var companies = CreateResumeCompanies(sourceData.Companies, aiGenerationResult);

        var generatedResumeId = await SaveGeneratedResumeAsync(sourceData, companies, aiGenerationResult, cancellationToken);

        return generatedResumeId;
    }

    private async Task<int> SaveGeneratedResumeAsync(ResumeSourceData resumeSource, IReadOnlyCollection<ResumeCompanyResult> companies, ResumeAiGenerationResult resumeAiGeneration, CancellationToken cancellationToken)
    {
        var generatedResume = new Resume(
            accountId: resumeSource.Account.Id,
            name: CreateResumeName(resumeSource.Account, resumeAiGeneration)
            );

        foreach (var company in companies)
        {
            var companySelection = generatedResume.AddCompanySelection(company.CompanyId, generatedResume.CompanySelections.Count + 1);

            foreach(var bullet in company.Bullets)
            {
                companySelection.AddResumeBullet(
                    sourceBulletId: bullet.SourceBulletId,
                    value: bullet.Value,
                    alternativeValue: bullet.AlternativeValue,
                    sortOrder: companySelection.Bullets.Count + 1
                );
            }
        }

        foreach(var education in resumeSource.Education.Where(e => e.UseForResume))
        {
            generatedResume.AddEducationSelection(education.Id, generatedResume.EducationSelections.Count + 1);
        }

        foreach(var project in resumeSource.Projects.Where(p => p.UseForResume))
        {
            generatedResume.AddProjectSelection(project.Id, generatedResume.ProjectSelections.Count + 1);
        }

        var aiAnalysis = new ResumeAiAnalysis(
            summary: resumeAiGeneration.Summary,
            score: resumeAiGeneration.Score
        );

        foreach(var insight in resumeAiGeneration.Strengths)
        {
            aiAnalysis.AddInsight(ResumeAiInsightType.Strength, insight);
        }

        foreach(var insight in resumeAiGeneration.Weaknesses)
        {
            aiAnalysis.AddInsight(ResumeAiInsightType.Weakness, insight);
        }

        generatedResume.SetAiAnalysis(aiAnalysis);

        generatedResume.SetAiMetaData(new ResumeAiMetaData(
            resumeAiGeneration.AiMetaData.Model,
            resumeAiGeneration.AiMetaData.InputTokens,
            resumeAiGeneration.AiMetaData.OutputTokens,
            resumeAiGeneration.AiMetaData.TotalTokens,
            resumeAiGeneration.AiMetaData.Cost));

        generatedResume.SetJobPosting(new ResumeJobPosting(
            companyName: resumeAiGeneration.JobPosting.CompanyName,
            jobTitle: resumeAiGeneration.JobPosting.JobTitle,
            location: resumeAiGeneration.JobPosting.Location,
            workStyle: resumeAiGeneration.JobPosting.WorkStyle,
            salaryMin: resumeAiGeneration.JobPosting.SalaryMin,
            salaryMax: resumeAiGeneration.JobPosting.SalaryMax,
            salary: resumeAiGeneration.JobPosting.Salary,
            salaryPeriod: resumeAiGeneration.JobPosting.SalaryPeriod,
            salaryCurrency: resumeAiGeneration.JobPosting.SalaryCurrency
        ));

        generatedResume.SetAppearance(ResumeAppearance.CreateDefault());

        await resumeRepository.CreateResumeAsync(generatedResume, cancellationToken);
        await resumeRepository.SaveAsync(cancellationToken);

        return generatedResume.Id;
    }

    private static IReadOnlyList<CompanyBulletContext> GetCompanyBulletContexts(IReadOnlyCollection<Company> companies)
    {
        List<CompanyBulletContext> bullets = new();
        foreach (var company in companies)
        {
            if (company.Bullets.Count > 0 && company.GenerateBullets)
            {
                var bulletMetaData = new CompanyBulletContext(
                    CompanyId: company.Id,
                    Name: company.Name,
                    Title: company.Title,
                    Location: company.Location,
                    Started: company.Started,
                    Ended: company.Ended,
                    Bullets: company.Bullets.Select(b => new BulletContext(b.Id, b.Value)).ToList(),
                    MaxBullets: company.MaxGeneratedBulletCount
                    );

                bullets.Add( bulletMetaData );
            }
        }
        return bullets;
    }

    private static IReadOnlyCollection<ResumeCompanyResult> CreateResumeCompanies(IReadOnlyCollection<Company> sourceCompanies, ResumeAiGenerationResult resumeAiGeneration)
    {
        var companiesToAlwaysInclude = sourceCompanies.Where(c => !c.GenerateBullets).ToList();

        var companiesToInclude = companiesToAlwaysInclude
            .Select(c => new ResumeCompanyResult(
                CompanyId: c.Id,
                SelectionId: null,
                Name: c.Name,
                Title: c.Title,
                Location: c.Location,
                Started: c.Started,
                Ended: c.Ended,
                Bullets: c.Bullets.Select(b => new ResumeBulletResult(
                    Id: null,
                    SourceBulletId: b.Id,
                    Value: b.Value,
                    AlternativeValue: null,
                    SortOrder: null,
                    IsSourceDeleted: b.DeletedDate.HasValue
                )
            ).ToList())).ToList();

        return resumeAiGeneration.Companies
            .Concat(companiesToInclude)
            .OrderByDescending(c => c.Started)
            .ThenByDescending(c => c.Ended ?? DateOnly.MaxValue)
            .ToList();
    }

    private static string CreateResumeName(Account account, ResumeAiGenerationResult resumeAiGeneration)
    {
        var primaryTitle = account.Titles
            .FirstOrDefault(t => t.IsPrimary)?
            .Value;

        var companyName = resumeAiGeneration.JobPosting?.CompanyName;

        var possibleCompanyName = !string.IsNullOrWhiteSpace(companyName)
            ? $"_{NormalizeName(companyName)}"
            : string.Empty;

        return $"{NormalizeName(account.DisplayName)}_{NormalizeName(primaryTitle ?? "Resume")}{possibleCompanyName}_{DateTime.UtcNow:yyyy_MM_dd}";
    }

    private static string NormalizeName(string value)
    {
        return value
            .Replace(" ", "_")
            .Replace("-", "_");
    }
}
