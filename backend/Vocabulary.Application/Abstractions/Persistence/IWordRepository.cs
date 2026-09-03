using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Abstractions.Persistence;

public interface IWordRepository
{
    Task<List<Word>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<List<WordMeaning>> GetMeaningsByLevelAsync(
        Level level,
        CancellationToken cancellationToken = default);
}
