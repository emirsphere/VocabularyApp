using Vocabulary.Domain.Enums;

namespace Vocabulary.Domain.Entities;

public class UserQuestionProgress
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid WordMeaningId { get; set; }

    public QuestionType QuestionType { get; set; }

    public double Weight { get; set; } = 1.0;

    public int CorrectCount { get; set; }

    public int WrongCount { get; set; }

    public bool IsMastered { get; set; }

    public User User { get; set; } = null!;

    public WordMeaning WordMeaning { get; set; } = null!;
}