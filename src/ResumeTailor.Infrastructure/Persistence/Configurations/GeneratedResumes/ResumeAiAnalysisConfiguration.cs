using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.AI;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeAiAnalysisConfiguration : IEntityTypeConfiguration<ResumeAiAnalysis>
{
    public void Configure(EntityTypeBuilder<ResumeAiAnalysis> builder)
    {
        builder.ToTable(nameof(ResumeAiAnalysis));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Summary)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(x => x.Score)
            .IsRequired();

        builder.HasMany(x => x.Insights)
            .WithOne()
            .HasForeignKey(x => x.ResumeAiAnalysisId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Insights)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}
