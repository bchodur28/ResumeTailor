using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Extraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace ResumeTailor.Infrastructure.Persistence.Configurations
{
    internal sealed class FieldSelectorConfiguration : IEntityTypeConfiguration<FieldSelector>
    {
        public void Configure(EntityTypeBuilder<FieldSelector> builder)
        {
            builder.ToTable(nameof(FieldSelector));
            builder.HasKey(selector => selector.Id);

            builder.Property(selector => selector.Selector)
                .HasMaxLength(1_00)
                .IsRequired();

            builder.Property(selector => selector.Priority).IsRequired();

            builder.HasIndex(selector => new
            {
                selector.FieldExtractionDefinitionId,
                selector.Priority
            }).IsUnique();

            builder.HasIndex(selector => new
            {
                selector.FieldExtractionDefinitionId,
                selector.Selector
            }).IsUnique();
            
        }
    }
}
