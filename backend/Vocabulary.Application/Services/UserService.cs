using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.DTOs;
using Vocabulary.Domain.Entities;

namespace Vocabulary.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> GetOrCreateAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        username = username.Trim();

        var existingUser = await _userRepository
            .GetByUsernameAsync(username, cancellationToken);

        if (existingUser is not null)
        {
            return MapToDto(existingUser);
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, cancellationToken);

        return MapToDto(user);
    }

    public async Task<UserDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository
            .GetByIdAsync(id, cancellationToken);

        return user is null ? null : MapToDto(user);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        };
    }
}
