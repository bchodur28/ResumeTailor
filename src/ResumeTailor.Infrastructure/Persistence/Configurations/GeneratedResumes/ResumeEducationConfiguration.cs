

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeEducationConfiguration : IEntityTypeConfiguration<ResumeEducationSelection>
{
    public void Configure(EntityTypeBuilder<ResumeEducationSelection> builder)
    {
        builder.ToTable(nameof(ResumeEducationSelection));
        builder.HasKey(education => education.Id);

        builder.Property(education => education.GeneratedResumeId)
            .IsRequired();

        builder.Property(education => education.EducationId)
            .IsRequired();

        builder.Property(education => education.SortOrder)
            .IsRequired();

    }
}
