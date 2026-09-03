using Microsoft.EntityFrameworkCore;
using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;
using Vocabulary.Infrastructure.Persistence.Context;

namespace Vocabulary.Infrastructure.Persistence.Repositories;

public class UserQuestionProgressRepository : IUserQuestionProgressRepository
{
    private readonly VocabularyDbContext _context;

    public UserQuestionProgressRepository(VocabularyDbContext context)
    {
        _context = context;
    }

    public async Task<UserQuestionProgress?> GetQuestionProgressAsync(
        Guid userId,
        Guid wordMeaningId,
        QuestionType questionType,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserQuestionProgresses
            .FirstOrDefaultAsync(
                x => x.UserId == userId &&
                     x.WordMeaningId == wordMeaningId &&
                     x.QuestionType == questionType,
                cancellationToken);
    }

    public Task<List<UserQuestionProgress>> GetQuestionProgressesAsync(
        Guid userId,
        Guid wordMeaningId,
        CancellationToken cancellationToken = default)
    {
        return _context.UserQuestionProgresses
            .Where(x => x.UserId == userId && x.WordMeaningId == wordMeaningId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddQuestionProgressAsync(
        UserQuestionProgress progress,
        CancellationToken cancellationToken = default)
    {
        await _context.UserQuestionProgresses.AddAsync(progress, cancellationToken);
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }
}
