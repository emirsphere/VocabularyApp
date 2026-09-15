using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.DTOs;

public class LevelProgressDto
{
    public Level Level { get; set; }

    public int TotalMeanings { get; set; }

    public int LearnedMeanings { get; set; }

    public double ProgressPercentage { get; set; }

    public bool IsUnlocked { get; set; }
}
