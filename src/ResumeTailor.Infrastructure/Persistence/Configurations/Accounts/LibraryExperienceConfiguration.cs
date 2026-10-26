using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ResumeTailor.Domain.Profile;

namespace ResumeTailor.Infrastructure.Persistence.Configurations.Accounts;

internal sealed class LibraryExperienceConfiguration : IEntityTypeConfiguration<LibraryExperience>
{
    public void Configure(EntityTypeBuilder<LibraryExperience> builder)
    {
        builder.ToTable(nameof(LibraryExperience));
        builder.HasKey(ex => ex.Id);

        builder.Property(ex => ex.AccountId)
            .IsRequired();

        builder.Property(ex => ex.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(ex => ex.Started)
            .HasColumnType("date");

        builder.Property(ex => ex.Ended)
            .HasColumnType("date");

        builder.Property(ex => ex.LetAiChooseSummary)
            .IsRequired();

        builder.Property(ex => ex.LetAiChooseBullets)
            .IsRequired();


        builder.HasMany(ex => ex.Summaries)
            .WithOne()
            .HasForeignKey(s => s.ExperienceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(ex => ex.Bullets)
            .WithOne()
            .HasForeignKey(b => b.ExperienceId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Navigation(ex => ex.Summaries)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Navigation(ex => ex.Bullets)
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Ignore(ex => ex.PrimarySummary);
    }
}
