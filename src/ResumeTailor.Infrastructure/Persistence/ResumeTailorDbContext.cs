using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Extraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence;

public sealed class ResumeTailorDbContext(DbContextOptions<ResumeTailorDbContext> options) : DbContext(options)
{
    public DbSet<SiteExtractionDefinition> ExtractionDefinitions => Set<SiteExtractionDefinition>();
    public DbSet<FieldExtractionDefinition> FieldExtractionDefinitions => Set<FieldExtractionDefinition>();
    public DbSet<FieldSelector> FieldSelectors => Set<FieldSelector>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResumeTailorDbContext).Assembly);
    }
     
}
