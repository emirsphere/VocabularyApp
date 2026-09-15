using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.DTOs;

public class SubmitAnswerRequest
{
    public Guid UserId { get; set; }

    public Guid WordMeaningId { get; set; }

    public QuestionType QuestionType { get; set; }

    public string Answer { get; set; } = string.Empty;
}
