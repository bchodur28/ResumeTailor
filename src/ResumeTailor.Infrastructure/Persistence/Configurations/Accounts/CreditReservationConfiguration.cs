using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

public class CreditReservationConfiguration : IEntityTypeConfiguration<CreditReservation>
{
    public void Configure(EntityTypeBuilder<CreditReservation> builder)
    {
        builder.ToTable(nameof(CreditReservation));

        builder.HasKey(cr => cr.Id);

        builder.Property(cr => cr.Amount)
            .IsRequired();

        builder.Property(cr => cr.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(cr => cr.RequestId)
            .IsRequired();

        builder.HasIndex(cr => cr.RequestId)
            .IsUnique();

        builder.Property(cr => cr.LeaseExpiresAt)
            .IsRequired();

        builder.Property(cr => cr.ResumeId)
            .IsRequired(false);

        builder.Property(cr => cr.CompletedAt)
            .IsRequired(false);

        builder.Property(cr => cr.ReleasedAt)
            .IsRequired(false);
    }
}
