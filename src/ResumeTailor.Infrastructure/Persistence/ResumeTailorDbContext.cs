using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Extraction;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.Resumes.JobApplications;
using ResumeTailor.Domain.Resumes.JobPositing;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Infrastructure.Persistence;

public sealed class ResumeTailorDbContext(DbContextOptions<ResumeTailorDbContext> options) : DbContext(options)
{
    public DbSet<SiteExtractionDefinition> SiteExtractionDefinitions => Set<SiteExtractionDefinition>();
    public DbSet<FieldExtractionDefinition> FieldExtractionDefinitions => Set<FieldExtractionDefinition>();
    public DbSet<FieldPattern> FieldPatterns => Set<FieldPattern>();

    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<ResumeCompanySelection> ResumeCompanySelections => Set<ResumeCompanySelection>();
    public DbSet<ResumeProjectSelection> ResumeProjectSelections => Set<ResumeProjectSelection>();
    public DbSet<ResumeEducationSelection> ResumeEducationSelections => Set<ResumeEducationSelection>();
    public DbSet<ResumeCompanyBullet> ResumeBullets => Set<ResumeCompanyBullet>();
    public DbSet<ResumeJobPosting> ResumeJobPostings => Set<ResumeJobPosting>();
    public DbSet<ResumeApplicationTracking> ResumeApplicationTrackings => Set<ResumeApplicationTracking>();
    public DbSet<ResumeAppearance> ResumeAppearances => Set<ResumeAppearance>();

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Bullet> Bullets => Set<Bullet>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<PersonalLink> AccountPersonalLinks => Set<PersonalLink>();
    public DbSet<Education> Education => Set<Education>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResumeTailorDbContext).Assembly);
    }
     
}
