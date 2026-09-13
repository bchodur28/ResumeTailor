using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Accounts;


namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts
{
    internal sealed class TitleConfiguration : IEntityTypeConfiguration<Title>
    {
        public void Configure(EntityTypeBuilder<Title> builder)
        {
            builder.ToTable(nameof(Title));
            builder.HasKey(title => title.Id);

            builder.Property(title => title.Value)
                .IsRequired()
                .HasMaxLength(200);
        }
    }
}
