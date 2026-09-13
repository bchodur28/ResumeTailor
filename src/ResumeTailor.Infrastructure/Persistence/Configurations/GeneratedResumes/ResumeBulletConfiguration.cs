using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.Content;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeBulletConfiguration : IEntityTypeConfiguration<ResumeBullet>
{
    public void Configure(EntityTypeBuilder<ResumeBullet> builder)
    {
        builder.ToTable(nameof(ResumeBullet));
        builder.HasKey(bullet => bullet.Id);

        builder.Property(bullet => bullet.Value).IsRequired();

    }
}
