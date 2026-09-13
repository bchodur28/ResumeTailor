using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeCompanySelectionConfiguration : IEntityTypeConfiguration<ResumeCompanySelection>
{
    public void Configure(EntityTypeBuilder<ResumeCompanySelection> builder)
    {
        builder.ToTable(nameof(ResumeCompanySelection));
        builder.HasKey(company => company.Id);

        builder.Property(company => company.GeneratedResumeId)
            .IsRequired();

        builder.Property(company => company.CompanyId)
            .IsRequired();

        builder.Property(company => company.SortOrder)
            .IsRequired();

        builder.HasMany(company => company.Bullets)
            .WithOne()
            .HasForeignKey(company => company.ResumeCompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(company => company.Bullets)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
