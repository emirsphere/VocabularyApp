using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabulary.Domain.Entities;

namespace Vocabulary.Infrastructure.Persistence.Configurations;

public class UserLevelProgressConfiguration
    : IEntityTypeConfiguration<UserLevelProgress>
{
    public void Configure(EntityTypeBuilder<UserLevelProgress> builder)
    {
        builder.ToTable("UserLevelProgress");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Level)
            .IsRequired();

        builder.Property(x => x.NextMeaningOrder)
            .IsRequired();

        builder.HasIndex(x => new { x.UserId, x.Level })
            .IsUnique();

        builder.HasOne(x => x.User)
    .WithMany(x => x.LevelProgresses)
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Cascade);
    }
}