using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal class LibraryEducationConfiguration : IEntityTypeConfiguration<LibraryEducation>
{
    public void Configure(EntityTypeBuilder<LibraryEducation> builder)
    {
        builder.ToTable(nameof(LibraryEducation));
        builder.HasKey(education => education.Id);

        builder.Property(education => education.InstitutionName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(education => education.Qualification)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(education => education.FieldOfStudy)
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
