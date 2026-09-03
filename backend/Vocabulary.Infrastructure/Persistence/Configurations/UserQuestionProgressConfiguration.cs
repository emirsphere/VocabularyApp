using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vocabulary.Domain.Entities;

namespace Vocabulary.Infrastructure.Persistence.Configurations;

public class UserQuestionProgressConfiguration
    : IEntityTypeConfiguration<UserQuestionProgress>
{
    public void Configure(EntityTypeBuilder<UserQuestionProgress> builder)
    {
        builder.ToTable("UserQuestionProgress");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.QuestionType)
            .IsRequired();

        builder.Property(x => x.Weight)
            .IsRequired();

        builder.Property(x => x.CorrectCount)
            .IsRequired();

        builder.Property(x => x.WrongCount)
            .IsRequired();

        builder.Property(x => x.IsMastered)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.UserId,
            x.WordMeaningId,
            x.QuestionType
        }).IsUnique();

        builder.HasOne(x => x.User)
    .WithMany(x => x.QuestionProgresses)
    .HasForeignKey(x => x.UserId)
    .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.WordMeaning)
            .WithMany(x => x.QuestionProgresses)
            .HasForeignKey(x => x.WordMeaningId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}