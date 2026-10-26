using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Resumes.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes
{
    internal class ResumeBulletConfiguration : IEntityTypeConfiguration<ResumeBullet>
    {
        public void Configure(EntityTypeBuilder<ResumeBullet> builder)
        {
            builder.ToTable(nameof(ResumeBullet));
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .ValueGeneratedOnAdd();

            builder.Property(b => b.ResumeExperienceId)
                .IsRequired();

            builder.Property(b => b.LibraryBulletId)
                .IsRequired();

            builder.Property(b => b.Alternative);
            builder.Property(b => b.Reasoning);

            builder.Property(b => b.SortOrder)
                .IsRequired();
        }
    }
}
