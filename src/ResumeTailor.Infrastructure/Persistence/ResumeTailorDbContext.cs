using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Extraction;
using ResumeTailor.Domain.Resume;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence;

public sealed class ResumeTailorDbContext(DbContextOptions<ResumeTailorDbContext> options) : DbContext(options)
{
    public DbSet<SiteExtractionDefinition> SiteExtractionDefinitions => Set<SiteExtractionDefinition>();
    public DbSet<FieldExtractionDefinition> FieldExtractionDefinitions => Set<FieldExtractionDefinition>();
    public DbSet<FieldPattern> FieldPatterns => Set<FieldPattern>();

    public DbSet<Resume> Resumes => Set<Resume>();
    public DbSet<Company> Companys => Set<Company>();
    public DbSet<Bullet> Bullets => Set<Bullet>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResumeTailorDbContext).Assembly);
    }
     
}
