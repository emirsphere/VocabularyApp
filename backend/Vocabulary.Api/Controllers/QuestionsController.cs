using Microsoft.AspNetCore.Mvc;
using Vocabulary.Application.Services;

namespace Vocabulary.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private readonly IQuestionService _questionService;

    public QuestionsController(IQuestionService questionService)
    {
        _questionService = questionService;
    }

    [HttpGet("next")]
    public async Task<IActionResult> GetNextQuestion(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        var question = await _questionService.GetNextQuestionAsync(
            userId,
            cancellationToken);

        if (question is null)
        {
            return NotFound("No questions available.");
        }

        return Ok(question);
    }
}