using Vocabulary.Application.DTOs;

namespace Vocabulary.Application.Services;

public interface IWordService
{
    Task<List<WordDto>> GetAllAsync(
        CancellationToken cancellationToken = default);
}