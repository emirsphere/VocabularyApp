using Microsoft.AspNetCore.Mvc;
using Vocabulary.Application.DTOs;
using Vocabulary.Application.Services;

namespace Vocabulary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrGetUser(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
        {
            return BadRequest("Username cannot be empty.");
        }

        var user = await _userService.GetOrCreateAsync(
            request.Username,
            cancellationToken);

        return Ok(user);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUser(
        Guid id,
        CancellationToken cancellationToken)
    {
        var user = await _userService.GetByIdAsync(
            id,
            cancellationToken);

        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpGet("{userId:guid}/stats")]
    public async Task<IActionResult> GetStatistics(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var statistics = await _userService.GetStatisticsAsync(
            userId,
            cancellationToken);

        return statistics is null ? NotFound() : Ok(statistics);
    }
}
