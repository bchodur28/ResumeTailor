using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Resumes.Content;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeExperienceConfiguration : IEntityTypeConfiguration<ResumeExperience>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ResumeExperience> builder)
    {
        builder.ToTable(nameof(ResumeExperience));
        builder.HasKey(e => e.Id);

        builder.Property(e => e.ResumeId)
            .IsRequired();

        builder.Property(e => e.LibraryExperienceId)
            .IsRequired();


        builder.HasMany(e => e.Bullets)
            .WithOne()
            .HasForeignKey(b => b.ResumeExperienceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.ResumeSummary)
            .WithOne()
            .HasForeignKey<ResumeSummary>(s => s.ResumeExperienceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.LibraryExperience)
            .WithMany()
            .HasForeignKey(e => e.LibraryExperienceId)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Navigation(e => e.Bullets)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
