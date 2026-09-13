using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Extraction;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Extraction;

internal sealed class FieldExtractionDefinitionConfiguration : IEntityTypeConfiguration<FieldExtractionDefinition>
{
    public void Configure(EntityTypeBuilder<FieldExtractionDefinition> builder)
    {
        builder.ToTable(nameof(FieldExtractionDefinition));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FieldName)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.DisplayLabel)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.ExtractionType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.AttributeName)
            .HasMaxLength(200);

        builder.Property(x => x.IsRequired)
            .IsRequired();

        builder.Property(x => x.SortOrder)
            .IsRequired();

        builder.HasMany(x => x.Patterns)
            .WithOne()
            .HasForeignKey(x => x.FieldExtractionDefinitionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Patterns)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
