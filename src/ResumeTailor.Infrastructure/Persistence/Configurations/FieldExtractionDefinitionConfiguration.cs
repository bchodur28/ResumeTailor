
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Extraction;


namespace ResumeTailor.Infrastructure.Persistence.Configurations;

internal sealed class FieldExtractionDefinitionConfiguration : IEntityTypeConfiguration<FieldExtractionDefinition>
{
    public void Configure(EntityTypeBuilder<FieldExtractionDefinition> builder)
    {

    }
}
