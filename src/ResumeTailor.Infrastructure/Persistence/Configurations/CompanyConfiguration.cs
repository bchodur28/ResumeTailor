using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Resumes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Configurations
{
    internal sealed class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(nameof(Company));

            builder.HasKey(company => company.Id);

            builder.Property(company => company.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(company => company.Position)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(company => company.WorkingStatus)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(company => company.Location)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(company => company.GenerateBullets)
                .IsRequired();

            builder.Property(company => company.MaxGeneratedBulletCount)
                .IsRequired();

            builder.HasMany(company => company.Bullets)
                .WithOne(bullet => bullet.Company)
                .HasForeignKey(bullet => bullet.CompanyId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(company => company.Bullets)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
