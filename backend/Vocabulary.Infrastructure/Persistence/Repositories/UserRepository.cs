using Microsoft.EntityFrameworkCore;
using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;
using Vocabulary.Infrastructure.Persistence.Context;

namespace Vocabulary.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly VocabularyDbContext _context;

    public UserRepository(VocabularyDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        return await _context.Users
            .FirstOrDefaultAsync(
                x => x.Username == username,
                cancellationToken);
    }

    public async Task AddAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserLevelProgress?> GetLevelProgressAsync(
        Guid userId,
        Level level,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserLevelProgresses
            .FirstOrDefaultAsync(
                x => x.UserId == userId && x.Level == level,
                cancellationToken);
    }

    public async Task AddLevelProgressAsync(
        UserLevelProgress progress,
        CancellationToken cancellationToken = default)
    {
        await _context.UserLevelProgresses.AddAsync(progress, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

}
