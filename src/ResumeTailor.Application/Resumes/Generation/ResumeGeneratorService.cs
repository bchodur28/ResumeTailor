using ResumeTailor.Application.Contracts.Education;
using ResumeTailor.Application.Contracts.Projects;
using ResumeTailor.Application.Contracts.Resume;
using ResumeTailor.Application.GeneratedResumes.Common.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Resumes.Common.Models;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.GeneratedResumes.Generation;

public class ResumeGeneratorService(
    IResumeAiGenerator aiGenerator,
    IResumeDataProvider resumeDataProvider) : IResumeGeneratorService
{
    public async Task<ResumeDetailsResponse> GenerateResumeDetailsAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default)
    {
        var sourceData = await resumeDataProvider.GetResumeSourceDataForGenerationAsync(accountId, cancellationToken);

        var companyBulletContexts = GetCompanyBulletContexts(sourceData.Companies);
        var companiesToAlwaysInclude = sourceData.Companies.Where(c => !c.GenerateBullets).ToList();

        var resumeAiGenerationContext = new ResumeAiGenerationContext(jobDescription,companyBulletContexts);
        var aiGenerationResult = await aiGenerator.GenerateAsync(resumeAiGenerationContext, cancellationToken);

        return CreateResumeDetailsResponse(sourceData, companiesToAlwaysInclude, aiGenerationResult);
    }

    private ResumeDetailsResponse CreateResumeDetailsResponse(ResumeSourceData resumeSourceData, IReadOnlyCollection<Company> companiesToAlwaysInclude, ResumeAiGenerationResult resumeAiGeneration)
    {
        var companiesToInclude = companiesToAlwaysInclude
            .Select(c => new ResumeCompanyResult(
                CompanyId: c.Id,
                Name: c.Name,
                Title: c.Title,
                Location: c.Location,
                Started: c.Started,
                Ended: c.Ended,
                Bullets: c.Bullets.Select(b => new ResumeBulletResult(
                    Value: b.Value,
                    AlternativeValue: null
                )
            ).ToList())).ToList();

        var allCompanies = resumeAiGeneration.Companies
            .Concat(companiesToInclude)
            .OrderByDescending(c => c.Started)
            .ThenByDescending(c => c.Ended ?? DateOnly.MaxValue)
            .ToList();


        var resumeGenerationReponse = new ResumeDetailsResponse(
            Resume: new ResumeResponse(
                Id: null,
                AccountId: resumeSourceData.Account.Id,
                PersonName: resumeSourceData.Account.DisplayName,
                Profession: resumeSourceData.Account.Titles?
                    .Where(t => t.IsPrimary)?
                    .FirstOrDefault()?
                    .Value ?? string.Empty,
                Email: resumeSourceData.Account.Email,
                PhoneNumber: resumeSourceData.Account.PhoneNumber,
                Location: $"{resumeSourceData.Account.City}, {resumeSourceData.Account.State}",
                PersonalLinks: resumeSourceData.Account.PersonalLinks?
                    .Select(pl => new PersonalLinkResponse(
                        Id: pl.Id,
                        DisplayName: pl.DisplayName,
                        Url: pl.Url
                    ))?
                    .ToList() ?? new List<PersonalLinkResponse>(),
                Companies: allCompanies,
                Education: resumeSourceData.Education
                    .Where(e => e.UseForResume)
                    .Select(e => new EducationResponse(
                        Id: e.Id,
                        SchoolName: e.SchoolName,
                        Degree: e.Degree,
                        Major: e.Major,
                        Started: e.Started,
                        Ended: e.Ended,
                        UseForResume: e.UseForResume
                    ))
                    .ToList(),
                Projects: resumeSourceData.Projects
                    .Where(p => p.UseForResume)
                    .Select(p => new ProjectResponse(
                        Id: p.Id,
                        Name: p.Name,
                        Description: p.Description,
                        Started: p.Started,
                        Ended: p.Ended,
                        TechStack: p.TechStack,
                        Link: p.Link,
                        UseForResume: p.UseForResume
                    ))
                    .ToList()
            ),
            ResumeSummary: new ResumeSummaryResponse(
                AiScore: resumeAiGeneration.score,
                AiSummary: resumeAiGeneration.Summary,
                Strengths: resumeAiGeneration.Strengths,
                Weaknesses: resumeAiGeneration.Weaknesses
            ),
            Usage: resumeAiGeneration.AiUsage
        );

        return resumeGenerationReponse;
    }

    private IReadOnlyList<CompanyBulletContext> GetCompanyBulletContexts(IReadOnlyCollection<Company> companies)
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
                    Bullets: company.Bullets.Select(b => b.Value).ToList(),
                    MaxBullets: company.MaxGeneratedBulletCount
                    );

                bullets.Add( bulletMetaData );
            }
        }
        return bullets;
    }
}
