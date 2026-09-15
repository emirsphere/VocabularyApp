using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.DTOs;

public class QuestionDto
{
    public Guid QuestionId { get; set; }

    public Guid WordMeaningId { get; set; }

    public string Word { get; set; } = null!;

    public Level Level { get; set; }

    public string PartOfSpeech { get; set; } = null!;

    public QuestionType QuestionType { get; set; }

    public string Prompt { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public List<string> Options { get; set; } = [];
}
