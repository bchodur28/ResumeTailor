using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Extraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Configurations
{
    internal sealed class FieldPatternConfiguration : IEntityTypeConfiguration<FieldPattern>
    {
        public void Configure(EntityTypeBuilder<FieldPattern> builder)
        {
            builder.ToTable(nameof(FieldPattern));
            builder.HasKey(pattern => pattern.Id);

            builder.Property(pattern => pattern.MatchPattern)
                .HasMaxLength(1_00)
                .IsRequired();

            builder.Property(pattern => pattern.Priority).IsRequired();

            builder.HasIndex(pattern => new
            {
                pattern.FieldExtractionDefinitionId,
                pattern.Priority
            }).IsUnique();

            builder.HasIndex(pattern => new
            {
                pattern.FieldExtractionDefinitionId,
                pattern.MatchPattern
            }).IsUnique();
            
        }
    }
}
