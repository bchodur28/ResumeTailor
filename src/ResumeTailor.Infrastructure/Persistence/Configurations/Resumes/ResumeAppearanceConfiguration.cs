

using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal class ResumeAppearanceConfiguration : IEntityTypeConfiguration<ResumeAppearance>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ResumeAppearance> builder)
    {
        builder.ToTable(nameof(ResumeAppearance));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ResumeId)
            .IsRequired();

        builder.Property(x => x.TitleFontSize)
            .IsRequired();

        builder.Property(x => x.SectionHeaderFontSize)
            .IsRequired();

        builder.Property(x => x.MainBodyFontSize)
            .IsRequired();

        builder.Property(x => x.FontFamily)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.FontColor)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.TopHeaderAlignment)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}

