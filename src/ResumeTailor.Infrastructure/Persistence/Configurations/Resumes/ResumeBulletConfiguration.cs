using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeBulletConfiguration : IEntityTypeConfiguration<ResumeCompanyBullet>
{
    public void Configure(EntityTypeBuilder<ResumeCompanyBullet> builder)
    {
        builder.ToTable(nameof(ResumeCompanyBullet));
        builder.HasKey(bullet => bullet.Id);

        builder.Property(bullet => bullet.Value).IsRequired();

    }
}
