using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.Resumes.ApplicationTracking;
using ResumeTailor.Domain.Resumes.JobPositing;
using ResumeTailor.Domain.Resumes.ResumeAppearance;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.ToTable(nameof(Resume));
        builder.HasKey(resume => resume.Id);

        builder.Property(resume => resume.Name)
            .IsRequired();

        builder.Property(resume => resume.AiScoreStaleness)
            .IsRequired();

        //Foreign key relationships

        builder.HasMany(resume => resume.CompanySelections)
            .WithOne()
            .HasForeignKey(company => company.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(resume => resume.EducationSelections)
            .WithOne()
            .HasForeignKey(education => education.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(resume => resume.ProjectSelections)
            .WithOne()
            .HasForeignKey(project => project.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.AiMetaData)
            .WithOne()
            .HasForeignKey<ResumeAiMetaData>(fileMetaData => fileMetaData.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.AiAnalysis)
            .WithOne()
            .HasForeignKey<ResumeAiAnalysis>(aiMetaData => aiMetaData.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.JobPosting)
            .WithOne()
            .HasForeignKey<ResumeJobPosting>(jobPosting => jobPosting.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.ApplicationTracking)
            .WithOne()
            .HasForeignKey<ResumeApplicationTracking>(tracking => tracking.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.Appearance)
            .WithOne()
            .HasForeignKey<ResumeAppearance>(appearance => appearance.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);


        // Configure navigation properties to use field access mode
        builder.Navigation(resume => resume.CompanySelections)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(resume => resume.EducationSelections)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(resume => resume.ProjectSelections)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
