using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class CreditTransactionConfiguration : IEntityTypeConfiguration<CreditTransaction>
{
    public void Configure(EntityTypeBuilder<CreditTransaction> builder)
    {
        builder.ToTable(nameof(CreditTransaction));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AccountId)
            .IsRequired();

        builder.Property(x => x.Amount)
            .IsRequired();

        builder.Property(x => x.Type)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.ResumeId)
            .IsRequired(false);

        builder.Property(x => x.ReferenceId)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(x => x.CreatedDate)
            .IsRequired();

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_CreditTransaction_Amount",
                "[Amount] <> 0"));

        builder.HasIndex(x => new { x.AccountId, x.CreatedDate });

        // Prevent modifications after the transactions is created.
        foreach (var property in builder.Metadata.GetProperties())
        {
            if (property.IsPrimaryKey())
            {
                continue;
            }

            property.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
        }
    }
}
