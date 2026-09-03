namespace Vocabulary.Application.DTOs;

public class WordDto
{
    public Guid Id { get; set; }

    public string Text { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public List<WordMeaningDto> Meanings { get; set; } = [];
}