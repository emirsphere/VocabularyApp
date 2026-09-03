using Vocabulary.Domain.Enums;

namespace Vocabulary.Infrastructure.Data.Models;

public class WordMeaningSeedModel
{
    public Level Level { get; set; }
    public string TurkishMeaning { get; set; } = null!;
    public int Order { get; set; }

    public string PartOfSpeech { get; set; } = null!;
}