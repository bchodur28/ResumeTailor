using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.AI;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeAiInsightConfiguration : IEntityTypeConfiguration<ResumeAiInsight>
{
    public void Configure(EntityTypeBuilder<ResumeAiInsight> builder)
    {
        builder.ToTable(nameof(ResumeAiInsight));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Type)
            .IsRequired();

        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(1000);
    }
}
