using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Abstractions.Persistence;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<User?> GetByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        User user,
        CancellationToken cancellationToken = default);

    Task<UserLevelProgress?> GetLevelProgressAsync(
        Guid userId,
        Level level,
        CancellationToken cancellationToken = default);

    Task AddLevelProgressAsync(
        UserLevelProgress progress,
        CancellationToken cancellationToken = default);
}
