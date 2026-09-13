using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeProjectConfiguration : IEntityTypeConfiguration<ResumeProjectSelection>
{
    public void Configure(EntityTypeBuilder<ResumeProjectSelection> builder)
    {
        builder.ToTable(nameof(ResumeProjectSelection));
        builder.HasKey(project => project.Id);


        builder.Property(project => project.GeneratedResumeId)
            .IsRequired();

        builder.Property(project => project.ProjectId)
            .IsRequired();

        builder.Property(project => project.SortOrder)
            .IsRequired();
    }
}
