using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabulary.Domain.Entities;

namespace Vocabulary.Infrastructure.Persistence.Configurations;

public class UserMeaningProgressConfiguration
    : IEntityTypeConfiguration<UserMeaningProgress>
{
    public void Configure(EntityTypeBuilder<UserMeaningProgress> builder)
    {
        builder.ToTable("UserMeaningProgress");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.Property(x => x.CorrectCount)
            .IsRequired();

        builder.Property(x => x.WrongCount)
            .IsRequired();

        builder.Property(x => x.IsLearned)
            .IsRequired();

        builder.Property(x => x.LastAnsweredAt)
            .IsRequired(false);

        builder.HasIndex(x => new
        {
            x.UserId,
            x.WordMeaningId
        }).IsUnique();

        builder.HasOne(x => x.User)
    .WithMany(x => x.MeaningProgresses)
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.WordMeaning)
            .WithMany(x => x.UserProgresses)
            .HasForeignKey(x => x.WordMeaningId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}