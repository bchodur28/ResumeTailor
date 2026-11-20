using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;

internal sealed class CreditWalletConfiguration : IEntityTypeConfiguration<CreditWallet>
{
    public void Configure(EntityTypeBuilder<CreditWallet> builder)
    {
        builder.ToTable(nameof(CreditWallet));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.Property(x => x.Balance)
            .IsRequired();

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_CreditWallet_Balance",
                "[Balance] >= 0"));

        builder.HasAlternateKey(x => x.AccountId);

        builder.HasMany(cw => cw.Transactions)
            .WithOne(t => t.CreditWallet)
            .HasForeignKey(t => t.AccountId)
            .HasPrincipalKey(cw => cw.AccountId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Transactions)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
