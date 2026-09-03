using Microsoft.AspNetCore.Mvc;
using Vocabulary.Application.Services;

namespace Vocabulary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WordsController : ControllerBase
{
    private readonly IWordService _wordService;

    public WordsController(IWordService wordService)
    {
        _wordService = wordService;
    }

    [HttpGet]
    public async Task<IActionResult> GetWords(
        CancellationToken cancellationToken)
    {
        var words = await _wordService.GetAllAsync(cancellationToken);

        return Ok(words);
    }
}