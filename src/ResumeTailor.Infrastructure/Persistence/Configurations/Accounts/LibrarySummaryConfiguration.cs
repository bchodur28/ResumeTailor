using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class LibrarySummaryConfiguration : IEntityTypeConfiguration<LibrarySummary>
{
    public void Configure(EntityTypeBuilder<LibrarySummary> builder)
    {
        builder.ToTable(nameof(LibrarySummary));
        builder.HasKey(s => s.Id);

        builder.Property(s => s.IsPrimary)
            .IsRequired();

        builder.Property(s => s.Value)
            .IsRequired()
            .HasMaxLength(500);

        builder.HasIndex(s => s.ExperienceId)
            .IsUnique()
            .HasFilter("\"IsPrimary\" = 1");

    }
}
