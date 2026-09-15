using Vocabulary.Application.DTOs;

namespace Vocabulary.Application.Services;

public interface ILevelProgressService
{
    Task<List<LevelProgressDto>?> GetProgressAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
