using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Extraction;

internal sealed class SiteExtractionDefinitionConfiguration : IEntityTypeConfiguration<SiteExtractionDefinition>
{
    public void Configure(EntityTypeBuilder<SiteExtractionDefinition> builder)
    {
        builder.ToTable(nameof(SiteExtractionDefinition));

        builder.HasKey(defintion => defintion.Id);

        builder.Property(definition => definition.SiteName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(definition => definition.Version).IsRequired();
        builder.Property(definition => definition.IsEnabled).IsRequired();
        builder.Property(definiton => definiton.CreatedDate).IsRequired();

        builder.HasIndex(definition => new
        {
            definition.Hostname,
            definition.PathPattern,
            definition.Version
        }).IsUnique();

        builder.HasMany(definition => definition.Fields)
            .WithOne()
            .HasForeignKey(field => field.SiteExtractionDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(definition => definition.Fields)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
