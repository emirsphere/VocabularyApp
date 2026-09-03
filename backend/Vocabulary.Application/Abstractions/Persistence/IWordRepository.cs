using Vocabulary.Domain.Entities;

namespace Vocabulary.Application.Abstractions.Persistence;

public interface IWordRepository
{
    Task<List<Word>> GetAllAsync(CancellationToken cancellationToken = default);
}