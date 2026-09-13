using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Accounts;
using ResumeTailor.Domain.Extraction;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Infrastructure.Persistence;

public sealed class ResumeTailorDbContext(DbContextOptions<ResumeTailorDbContext> options) : DbContext(options)
{
    public DbSet<SiteExtractionDefinition> SiteExtractionDefinitions => Set<SiteExtractionDefinition>();
    public DbSet<FieldExtractionDefinition> FieldExtractionDefinitions => Set<FieldExtractionDefinition>();
    public DbSet<FieldPattern> FieldPatterns => Set<FieldPattern>();

    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    public DbSet<GeneratedResume> GeneratedResumes => Set<GeneratedResume>();
    public DbSet<ResumeCompanySelection> ResumeCompanySelections => Set<ResumeCompanySelection>();
    public DbSet<ResumeProjectSelection> ResumeProjectSelections => Set<ResumeProjectSelection>();
    public DbSet<ResumeEducationSelection> ResumeEducationSelections => Set<ResumeEducationSelection>();
    public DbSet<ResumeBullet> ResumeBullets => Set<ResumeBullet>();

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Company> AccountCompanies => Set<Company>();
    public DbSet<Bullet> AccountBullets => Set<Bullet>();
    public DbSet<Project> AccountProjects => Set<Project>();
    public DbSet<Title> AccountTitles => Set<Title>();
    public DbSet<PersonalLink> AccountPersonalLinks => Set<PersonalLink>();
    public DbSet<Education> AccountEducations => Set<Education>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResumeTailorDbContext).Assembly);
    }
     
}
