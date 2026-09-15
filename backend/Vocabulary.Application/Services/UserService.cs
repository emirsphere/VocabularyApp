using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.DTOs;
using Vocabulary.Domain.Entities;

namespace Vocabulary.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserMeaningProgressRepository _meaningProgressRepository;

    public UserService(
        IUserRepository userRepository,
        IUserMeaningProgressRepository meaningProgressRepository)
    {
        _userRepository = userRepository;
        _meaningProgressRepository = meaningProgressRepository;
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

    public async Task<UserStatisticsDto?> GetStatisticsAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var meaningProgresses = await _meaningProgressRepository
            .GetMeaningProgressesAsync(userId, cancellationToken);

        return new UserStatisticsDto
        {
            TotalLearnedMeanings = meaningProgresses.Count(x => x.IsLearned),
            TotalCorrectAnswers = meaningProgresses.Sum(x => x.CorrectCount),
            TotalWrongAnswers = meaningProgresses.Sum(x => x.WrongCount)
        };
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
