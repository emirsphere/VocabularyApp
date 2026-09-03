namespace Vocabulary.Application.DTOs;

public class UserDto
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}