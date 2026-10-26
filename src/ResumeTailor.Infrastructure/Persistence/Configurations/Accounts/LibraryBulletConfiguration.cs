using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class LibraryBulletConfiguration : IEntityTypeConfiguration<LibraryBullet>
{
    public void Configure(EntityTypeBuilder<LibraryBullet> builder)
    {
        builder.ToTable(nameof(LibraryBullet));
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Value)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(b => b.AiRewrite)
            .HasMaxLength(500);

        builder.Property(b => b.AiOpinion)
            .HasMaxLength(1000);

    }
}
