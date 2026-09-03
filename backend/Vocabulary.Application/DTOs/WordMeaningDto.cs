using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.DTOs;

public class WordMeaningDto
{
    public Guid Id { get; set; }

    public Level Level { get; set; }

    public int Order { get; set; }

    public string PartOfSpeech { get; set; } = null!;

    public string TurkishMeaning { get; set; } = null!;

    public string? ExampleSentence { get; set; }
}