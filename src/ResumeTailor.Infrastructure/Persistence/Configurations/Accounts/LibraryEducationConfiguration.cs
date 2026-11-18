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
            .HasMaxLength(200);

        builder.Property(x => x.Gpa)
            .HasPrecision(4, 3);

        builder.Property(education => education.Started);

        builder.Property(education => education.Ended);

        builder.Property(education => education.UseForResume)
            .IsRequired();

    }
}
