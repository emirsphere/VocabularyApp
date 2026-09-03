namespace Vocabulary.Domain.Entities;

public class Word
{
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public ICollection<WordMeaning> Meanings { get; set; }
        = new List<WordMeaning>();
}