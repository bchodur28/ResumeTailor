using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal class EducationConfiguration : IEntityTypeConfiguration<Education>
{
    public void Configure(EntityTypeBuilder<Education> builder)
    {
        builder.ToTable(nameof(Education));
        builder.HasKey(education => education.Id);

        builder.Property(education => education.SchoolName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(education => education.Degree)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(education => education.Major)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(education => education.Started)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(education => education.Ended)
            .HasColumnType("date");

        builder.Property(education => education.UseForResume)
            .IsRequired();

    }
}
