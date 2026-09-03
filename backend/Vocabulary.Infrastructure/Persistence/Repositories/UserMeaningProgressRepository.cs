using Microsoft.EntityFrameworkCore;
using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;
using Vocabulary.Infrastructure.Persistence.Context;

namespace Vocabulary.Infrastructure.Persistence.Repositories;

public class UserMeaningProgressRepository : IUserMeaningProgressRepository
{
    private readonly VocabularyDbContext _context;

    public UserMeaningProgressRepository(VocabularyDbContext context)
    {
        _context = context;
    }

    public async Task<UserMeaningProgress?> GetMeaningProgressAsync(
        Guid userId,
        Guid wordMeaningId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserMeaningProgresses
            .FirstOrDefaultAsync(
                x => x.UserId == userId && x.WordMeaningId == wordMeaningId,
                cancellationToken);
    }

    public Task<List<UserMeaningProgress>> GetMeaningProgressesByLevelAsync(
        Guid userId,
        Level level,
        CancellationToken cancellationToken = default)
    {
        return _context.UserMeaningProgresses
            .Where(x => x.UserId == userId && x.WordMeaning.Level == level)
            .ToListAsync(cancellationToken);
    }

    public async Task AddMeaningProgressAsync(
        UserMeaningProgress progress,
        CancellationToken cancellationToken = default)
    {
        await _context.UserMeaningProgresses.AddAsync(progress, cancellationToken);
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
