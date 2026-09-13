using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts
{
    internal class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable(nameof(Project));
            builder.HasKey(project => project.Id);

            builder.Property(project => project.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(project => project.Description)
                .IsRequired();

            builder.Property(project => project.Started)
                .HasColumnType("date")
                .IsRequired();

            builder.Property(project => project.Ended)
                .HasColumnType("date");

            builder.Property(project => project.UseForResume)
                .IsRequired();
        }
    }
}
