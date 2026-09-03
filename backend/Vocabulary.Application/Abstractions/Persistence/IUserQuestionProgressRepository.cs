using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Abstractions.Persistence;

public interface IUserQuestionProgressRepository
{
    Task<UserQuestionProgress?> GetQuestionProgressAsync(
        Guid userId,
        Guid wordMeaningId,
        QuestionType questionType,
        CancellationToken cancellationToken = default);

    Task<List<UserQuestionProgress>> GetQuestionProgressesAsync(
        Guid userId,
        Guid wordMeaningId,
        CancellationToken cancellationToken = default);

    Task AddQuestionProgressAsync(
        UserQuestionProgress progress,
        CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
