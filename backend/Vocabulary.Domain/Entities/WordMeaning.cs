using Vocabulary.Domain.Enums;

namespace Vocabulary.Domain.Entities;

public class WordMeaning
{
    public Guid Id { get; set; }

    public Guid WordId { get; set; }

    public Level Level { get; set; }

    public int Order { get; set; }

    public string PartOfSpeech { get; set; } = null!;

    public string TurkishMeaning { get; set; } = null!;

    public string? ExampleSentence { get; set; }

    public Word Word { get; set; } = null!;

    public ICollection<UserMeaningProgress> UserProgresses { get; set; }
        = new List<UserMeaningProgress>();

    public ICollection<UserQuestionProgress> QuestionProgresses { get; set; }
        = new List<UserQuestionProgress>();
}