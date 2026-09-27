using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Resumes.JobPositing;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal class ResumeJobPostingConfiguration : IEntityTypeConfiguration<ResumeJobPosting>
{
    public void Configure(EntityTypeBuilder<ResumeJobPosting> builder)
    {
        builder.ToTable(nameof(ResumeJobPosting));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.ResumeId)
                .IsRequired();

        builder.Property(x => x.CompanyName)
            .HasMaxLength(200);

        builder.Property(x => x.JobTitle)
            .HasMaxLength(200);

        builder.Property(x => x.Location)
            .HasMaxLength(200);

        builder.Property(x => x.WorkStyle)
            .HasMaxLength(50);

        builder.Property(x => x.SalaryMin)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.SalaryMax)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Salary)
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.SalaryPeriod)
            .HasMaxLength(50);

        builder.Property(x => x.SalaryCurrency)
            .HasMaxLength(10);
    }
}
