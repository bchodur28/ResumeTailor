using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal class BulletConfiguration : IEntityTypeConfiguration<Bullet>
{
    public void Configure(EntityTypeBuilder<Bullet> builder)
    {
        builder.ToTable(nameof(Bullet));
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Value)
            .IsRequired();
    }
}
