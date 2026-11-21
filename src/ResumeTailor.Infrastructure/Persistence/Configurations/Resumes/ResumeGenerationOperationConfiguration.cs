using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Resumes;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes;

public class ResumeGenerationOperationConfiguration
    : IEntityTypeConfiguration<ResumeGenerationOperation>
{
    public void Configure(
        EntityTypeBuilder<ResumeGenerationOperation> builder)
    {
        builder.ToTable("ResumeGenerationOperation");

        builder.HasKey(o => o.Id);

        builder.Property(o => o.RequestId)
            .IsRequired();

        builder.Property(o => o.RequestHash)
            .HasMaxLength(64)
            .IsRequired();

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .HasMaxLength(30)
            .IsRequired();

        builder.Property(o => o.AiResultJson)
            .HasColumnType("nvarchar(max)");

        builder.Property(o => o.LeaseToken)
            .IsRequired();

        builder.Property(o => o.LeaseExpiresAt)
            .IsRequired();

        builder.HasIndex(o => new { o.AccountId, o.RequestId })
            .IsUnique();

        builder.HasIndex(o => new { o.Status, o.LeaseExpiresAt });

        builder.HasIndex(o => o.ReservationId)
            .IsUnique();

        builder.Property<byte[]>("RowVersion")
            .IsRowVersion();
    }
}
