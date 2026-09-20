using ResumeTailor.Application.GeneratedResumes.Common.Models;
using ResumeTailor.Application.GeneratedResumes.Generation.Interfaces;
using ResumeTailor.Application.GeneratedResumes.Generation.Models;
using ResumeTailor.Application.Profile.Accounts.Interfaces;
using ResumeTailor.Application.Profile.Accounts.Models;
using ResumeTailor.Application.Profile.Education.Interfaces;
using ResumeTailor.Application.Profile.Education.Models;
using ResumeTailor.Application.Profile.Experience.Interfaces;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Application.GeneratedResumes.Generation;

public class ResumeGeneratorService(
    IAccountRepository accountRepository,
    IEducationRepository educationRepository,
    IExperienceRepository experienceRepository,
    IResumeAiGenerator aiGenerator) : IResumeGeneratorService
{
    public async Task<GeneratedResumeDetailsResponse> GenerateResumeReviewAsync(int accountId, string jobDescription, CancellationToken cancellationToken = default)
    {
        var account = await accountRepository.GetAccountByIdAsync(accountId, cancellationToken)
            ?? throw new KeyNotFoundException($"Account for resume with id {accountId} was not found during resume generation");

        var companies = await experienceRepository.GetCompaniesByAccountIdAsync(accountId, cancellationToken);

        var companiesToAlwaysInclude = companies.Where(c => !c.GenerateBullets).ToList();

        var education = await educationRepository.GetEducationByAccountIdAsync(accountId, cancellationToken);

        var projects = await experienceRepository.GetProjectsByAccountIdAsync(accountId, cancellationToken);

        var companyBulletContexts = GetCompanyBulletContexts(companies);

        var resumeAiGenerationContext = new ResumeAiGenerationContext(
            jobDescription,
            companyBulletContexts
        );


        var aiGenerationResult = await aiGenerator.GenerateAsync(resumeAiGenerationContext, cancellationToken);

        return CreateResumeGenerationResponse(account, companiesToAlwaysInclude, education, projects, aiGenerationResult);
    }

    private GeneratedResumeDetailsResponse CreateResumeGenerationResponse(Account account, IReadOnlyCollection<Company> companiesToAlwaysInclude, IReadOnlyCollection<Education> education, IReadOnlyCollection<Project> projects, ResumeAiGenerationResult resumeAiGeneration)
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


        var resumeGenerationReponse = new GeneratedResumeDetailsResponse(
            GeneratedResume: new GeneratedResumeResponse(
                Id: null,
                AccountId: account.Id,
                PersonName: account.DisplayName,
                Profession: account.Titles?
                    .Where(t => t.IsPrimary)?
                    .FirstOrDefault()?
                    .Value ?? string.Empty,
                Email: account.Email,
                PhoneNumber: account.PhoneNumber,
                Location: $"{account.City}, {account.State}, {account.Country}",
                PersonalLinks: account.PersonalLinks?
                    .Select(pl => new PersonalLinkResponse(
                        Id: pl.Id,
                        DisplayName: pl.DisplayName,
                        Url: pl.Url
                    ))?
                    .ToList() ?? new List<PersonalLinkResponse>(),
                Companies: allCompanies,
                Education: education
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
                Projects: projects
                    .Where(p => p.UseForResume)
                    .Select(p => new ResumeProjectResponse(
                        Id: p.Id,
                        Name: p.Name,
                        Description: p.Description,
                        Started: p.Started,
                        Ended: p.Ended,
                        TechStack: p.TechStack,
                        Link: p.Link
                    ))
                    .ToList()
            ),
            GeneratedSummary: new GeneratedSummaryResponse(
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
