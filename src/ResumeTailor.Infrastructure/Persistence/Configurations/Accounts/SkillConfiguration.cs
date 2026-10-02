using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> builder)
    {
        builder.ToTable(nameof(Skill));
        builder.HasKey(title => title.Id);

        builder.Property(title => title.Value)
            .IsRequired()
            .HasMaxLength(200);
    }
}
