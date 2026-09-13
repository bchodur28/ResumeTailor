using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Accounts;


namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts
{
    internal sealed class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable(nameof(Account));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Auth0UserId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.DisplayName)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.City)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.State)
                .HasMaxLength(100);

            builder.Property(x => x.Country)
                .HasMaxLength(100)
                .IsRequired();

            //Foreign key relationships
            builder.HasMany(x => x.PersonalLinks)
                .WithOne()
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Titles)
                .WithOne()
                .HasForeignKey(x => x.AccountId)
                .OnDelete(DeleteBehavior.Cascade);


            //Navigation properties
            builder.Navigation(x => x.PersonalLinks)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.Navigation(x => x.Titles)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
