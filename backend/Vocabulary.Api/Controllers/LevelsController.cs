using Microsoft.AspNetCore.Mvc;
using Vocabulary.Application.Services;

namespace Vocabulary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LevelsController : ControllerBase
{
    private readonly ILevelProgressService _levelProgressService;

    public LevelsController(ILevelProgressService levelProgressService)
    {
        _levelProgressService = levelProgressService;
    }

    [HttpGet("progress")]
    public async Task<IActionResult> GetProgress(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var progress = await _levelProgressService.GetProgressAsync(
            userId,
            cancellationToken);

        return progress is null ? NotFound("User not found.") : Ok(progress);
    }
}
