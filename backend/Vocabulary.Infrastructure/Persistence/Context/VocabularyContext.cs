using Microsoft.EntityFrameworkCore;
using Vocabulary.Domain.Entities;

namespace Vocabulary.Infrastructure.Persistence.Context;

public class VocabularyDbContext : DbContext
{
    public VocabularyDbContext(
        DbContextOptions<VocabularyDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Word> Words => Set<Word>();

    public DbSet<WordMeaning> WordMeanings => Set<WordMeaning>();

    public DbSet<UserLevelProgress> UserLevelProgresses
        => Set<UserLevelProgress>();

    public DbSet<UserMeaningProgress> UserMeaningProgresses
        => Set<UserMeaningProgress>();

    public DbSet<UserQuestionProgress> UserQuestionProgresses
        => Set<UserQuestionProgress>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(VocabularyDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}