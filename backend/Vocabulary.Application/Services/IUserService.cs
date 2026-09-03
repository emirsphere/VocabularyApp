using Vocabulary.Application.DTOs;

namespace Vocabulary.Application.Services;

public interface IUserService
{
    Task<UserDto> GetOrCreateAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<UserDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}