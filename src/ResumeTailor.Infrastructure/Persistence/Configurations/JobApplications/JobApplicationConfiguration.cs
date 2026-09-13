using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes;
using ResumeTailor.Domain.JobApplications;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.JobApplications;

internal sealed class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable(nameof(JobApplication));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.CompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.JobName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Location)
            .HasMaxLength(200);

        builder.Property(x => x.WorkStyle)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(x => x.AppliedDate)
            .HasColumnType("date");

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        
        builder.HasOne(x => x.GeneratedResume)
            .WithOne()
            .HasForeignKey<GeneratedResume>(x => x.JobApplicationId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
