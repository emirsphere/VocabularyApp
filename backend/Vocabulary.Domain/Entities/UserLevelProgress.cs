using Vocabulary.Domain.Enums;

namespace Vocabulary.Domain.Entities;

public class UserLevelProgress
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Level Level { get; set; }

    public int NextMeaningOrder { get; set; }

    public User User { get; set; } = null!;
}