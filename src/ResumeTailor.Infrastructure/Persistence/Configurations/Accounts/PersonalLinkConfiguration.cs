
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Accounts;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class PersonalLinkConfiguration : IEntityTypeConfiguration<PersonalLink>
{
    public void Configure(EntityTypeBuilder<PersonalLink> builder)
    {
        builder.ToTable(nameof(PersonalLink));
        builder.HasKey(link => link.Id);

        builder.Property(link => link.DisplayName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(link => link.Url)
            .IsRequired();

    }
}
