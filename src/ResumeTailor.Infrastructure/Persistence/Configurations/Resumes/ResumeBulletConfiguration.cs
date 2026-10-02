using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.GeneratedResumes.Content;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

internal sealed class ResumeBulletConfiguration : IEntityTypeConfiguration<ResumeCompanyBullet>
{
    public void Configure(EntityTypeBuilder<ResumeCompanyBullet> builder)
    {
        builder.ToTable(nameof(ResumeCompanyBullet));
        builder.HasKey(bullet => bullet.Id);

        builder.HasIndex(bullet => bullet.SourceBulletId);

        builder.Property(bullet => bullet.Value).IsRequired();

        builder
           .HasOne<Bullet>()
           .WithMany()
           .HasForeignKey(x => x.SourceBulletId)
           .OnDelete(DeleteBehavior.Restrict);

    }
}
