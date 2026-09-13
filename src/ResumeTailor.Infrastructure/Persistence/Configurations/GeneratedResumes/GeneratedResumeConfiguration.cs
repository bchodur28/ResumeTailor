using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.GeneratedResumes.AI;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class GeneratedResumeConfiguration : IEntityTypeConfiguration<GeneratedResume>
{
    public void Configure(EntityTypeBuilder<GeneratedResume> builder)
    {
        builder.ToTable(nameof(GeneratedResume));
        builder.HasKey(resume => resume.Id);

        builder.Property(resume => resume.Name)
            .IsRequired();

        //Foreign key relationships

        builder.HasMany(resume => resume.CompanySelections)
            .WithOne()
            .HasForeignKey(company => company.GeneratedResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(resume => resume.EducationSelections)
            .WithOne()
            .HasForeignKey(education => education.GeneratedResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(resume => resume.ProjectSelections)
            .WithOne()
            .HasForeignKey(project => project.GeneratedResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.AiMetaData)
            .WithOne()
            .HasForeignKey<ResumeAiMetaData>(fileMetaData => fileMetaData.GeneratedResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.AiAnalysis)
            .WithOne()
            .HasForeignKey<ResumeAiAnalysis>(aiMetaData => aiMetaData.GeneratedResumeId)
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
