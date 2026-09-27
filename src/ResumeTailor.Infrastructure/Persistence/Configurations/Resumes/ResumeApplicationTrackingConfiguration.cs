using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Resumes.ApplicationTracking;


namespace ResumeTailor.Infrastructure.Persistence.Configurations.Resumes
{
    internal class ResumeApplicationTrackingConfiguration : IEntityTypeConfiguration<ResumeApplicationTracking>
    {
        public void Configure(EntityTypeBuilder<ResumeApplicationTracking> builder)
        {
            builder.ToTable(nameof(ResumeApplicationTracking));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.ResumeId)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.Interviewed)
                .HasColumnType("date");

            builder.Property(x => x.OfferReceived)
                .HasColumnType("date");

            builder.Property(x => x.OfferAccepted)
                .HasColumnType("date");

            builder.Property(x => x.Rejected)
                .HasColumnType("date");

            builder.Property(x => x.Status)
                .HasColumnType("date");
        }
    }
}
