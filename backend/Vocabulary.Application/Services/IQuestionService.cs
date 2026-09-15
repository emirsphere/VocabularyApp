using Vocabulary.Application.DTOs;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Services;

public interface IQuestionService
{
    Task<QuestionDto?> GetNextQuestionAsync(
        Guid userId,
        Level level,
        CancellationToken cancellationToken = default);

    Task<SubmitAnswerResponse> SubmitAnswerAsync(
        SubmitAnswerRequest request,
        CancellationToken cancellationToken = default);
}
