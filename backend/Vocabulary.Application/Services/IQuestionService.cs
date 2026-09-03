using Vocabulary.Application.DTOs;

namespace Vocabulary.Application.Services;

public interface IQuestionService
{
    Task<QuestionDto?> GetNextQuestionAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}