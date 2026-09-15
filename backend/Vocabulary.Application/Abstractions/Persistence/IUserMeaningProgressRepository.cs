using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Abstractions.Persistence;

public interface IUserMeaningProgressRepository
{
    Task<UserMeaningProgress?> GetMeaningProgressAsync(
        Guid userId,
        Guid wordMeaningId,
        CancellationToken cancellationToken = default);

    Task<List<UserMeaningProgress>> GetMeaningProgressesByLevelAsync(
        Guid userId,
        Level level,
        CancellationToken cancellationToken = default);

    Task<List<UserMeaningProgress>> GetMeaningProgressesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task AddMeaningProgressAsync(
        UserMeaningProgress progress,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
