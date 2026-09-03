using Vocabulary.Domain.Enums;

namespace Vocabulary.Infrastructure.Data.Models;

public class WordSeedModel
{
    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public List<WordMeaningSeedModel> Meanings { get; set; } = [];
}