using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Profile;
using ResumeTailor.Domain.Extraction;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.Resumes.JobPositing;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.ResumeAppearance;
using ResumeTailor.Domain.Resumes.Content;

namespace ResumeTailor.Infrastructure.Persistence;

public sealed class ResumeTailorDbContext(DbContextOptions<ResumeTailorDbContext> options) : DbContext(options)
{
    public DbSet<SiteExtractionDefinition> SiteExtractionDefinitions => Set<SiteExtractionDefinition>();
    public DbSet<FieldExtractionDefinition> FieldExtractionDefinitions => Set<FieldExtractionDefinition>();
    public DbSet<FieldPattern> FieldPatterns => Set<FieldPattern>();

    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<ResumeEducationSelection> ResumeEducationSelections => Set<ResumeEducationSelection>();
    public DbSet<ResumeJobPosting> ResumeJobPostings => Set<ResumeJobPosting>();
    public DbSet<ResumeApplicationTracking> ResumeApplicationTrackings => Set<ResumeApplicationTracking>();
    public DbSet<ResumeAppearance> ResumeAppearances => Set<ResumeAppearance>();
    public DbSet<ResumeExperience> ResumeExperiences => Set<ResumeExperience>();

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Title> Titles => Set<Title>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<PersonalLink> AccountPersonalLinks => Set<PersonalLink>();
    public DbSet<LibraryEducation> Education => Set<LibraryEducation>();

    public DbSet<LibraryExperience> LibraryExperiences => Set<LibraryExperience>();
    public DbSet<LibraryBullet> LibraryBullets => Set<LibraryBullet>();
    public DbSet<LibrarySummary> LibrarySummaries => Set<LibrarySummary>();

    private void ValidateCreditTransactions()
    {
        var invalidEntries = ChangeTracker
            .Entries<CreditTransaction>()
            .Where(x =>
                x.State == EntityState.Modified ||
                x.State == EntityState.Deleted);

        if (invalidEntries.Any())
        {
            throw new InvalidOperationException("Credit transactions cannot be modified or deleted.");
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ValidateCreditTransactions();

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResumeTailorDbContext).Assembly);
    }
     
}
