using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable(nameof(Company));
        builder.HasKey(company => company.Id);

        builder.Property(company => company.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(company => company.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(company => company.Location)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(company => company.Started)
            .IsRequired()
            .HasColumnType("date");

        builder.Property(company => company.Ended)
            .HasColumnType("date");

        builder.Property(company => company.GenerateBullets)
            .IsRequired();

        builder.Property(company => company.MaxGeneratedBulletCount)
            .IsRequired();

        builder.HasMany(company => company.Bullets)
            .WithOne()
            .HasForeignKey(bullet => bullet.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(company => company.Bullets)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
