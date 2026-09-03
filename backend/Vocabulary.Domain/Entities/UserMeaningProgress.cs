namespace Vocabulary.Domain.Entities;

public class UserMeaningProgress
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid WordMeaningId { get; set; }

    public double Weight { get; set; } = 1.0;

    public int CorrectCount { get; set; }

    public int WrongCount { get; set; }

    public bool IsLearned { get; set; }

    public DateTime? LastAnsweredAt { get; set; }

    public User User { get; set; } = null!;

    public WordMeaning WordMeaning { get; set; } = null!;
}