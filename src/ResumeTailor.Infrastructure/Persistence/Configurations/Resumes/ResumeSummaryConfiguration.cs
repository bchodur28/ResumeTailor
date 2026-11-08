using Microsoft.EntityFrameworkCore;
using ResumeTailor.Domain.Resumes.Content;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes
{
    internal sealed class ResumeSummaryConfiguration : IEntityTypeConfiguration<ResumeSummary>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ResumeSummary> builder)
        {
            builder.ToTable(nameof(ResumeSummary));
            builder.HasKey(rs => rs.Id);

            builder.Property(rs => rs.ResumeExperienceId)
                .IsRequired();

            builder.Property(rs => rs.LibrarySummaryId)
                .IsRequired();

            builder.Property(rs => rs.WasManuallyChanged)
                .IsRequired();

            builder.Property(rs => rs.SortOrder)
                .IsRequired();

            builder.Property(rs => rs.Reasoning)
                .HasMaxLength(1000);

            builder.HasOne(rs => rs.LibrarySummary)
                .WithMany()
                .HasForeignKey(rs => rs.LibrarySummaryId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
