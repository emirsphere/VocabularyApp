namespace Vocabulary.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public ICollection<UserLevelProgress> LevelProgresses { get; set; }
        = new List<UserLevelProgress>();

    public ICollection<UserMeaningProgress> MeaningProgresses { get; set; }
        = new List<UserMeaningProgress>();

    public ICollection<UserQuestionProgress> QuestionProgresses { get; set; }
        = new List<UserQuestionProgress>();
}