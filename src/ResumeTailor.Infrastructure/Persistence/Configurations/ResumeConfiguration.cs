using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Infrastructure.Persistence.Configurations;

internal sealed class ResumeConfiguration : IEntityTypeConfiguration<Resume>
{
    public void Configure(EntityTypeBuilder<Resume> builder)
    {
        builder.ToTable(nameof(Resume));

        builder.HasKey(resume => resume.Id);

        builder.Property(resume => resume.PersonName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(resume => resume.Profession)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(resume => resume.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(resume => resume.PhoneNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(resume => resume.College)
            .HasMaxLength(100);

        builder.Property(resume => resume.Degree)
            .HasMaxLength(100);

        builder.Property(resume => resume.Major)
            .HasMaxLength(100);

        builder.Property(resume => resume.CollegeStatus)
            .HasMaxLength(100);

        builder.HasMany(resume => resume.Companies)
            .WithOne(company => company.Resume)
            .HasForeignKey(company => company.ResumeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(resume => resume.Companies)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
            
    }
}
