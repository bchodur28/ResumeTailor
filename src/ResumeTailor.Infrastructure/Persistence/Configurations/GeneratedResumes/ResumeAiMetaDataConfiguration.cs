using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.AI;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal class ResumeAiMetaDataConfiguration : IEntityTypeConfiguration<ResumeAiMetaData>
{
    public void Configure(EntityTypeBuilder<ResumeAiMetaData> builder)
    {
        builder.ToTable(nameof(ResumeAiMetaData));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.InputTokens)
            .IsRequired();

        builder.Property(x => x.OutputTokens)
            .IsRequired();

        builder.Property(x => x.TotalTokens)
            .IsRequired();

    }
}
