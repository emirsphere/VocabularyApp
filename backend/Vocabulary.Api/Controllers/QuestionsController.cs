using Microsoft.AspNetCore.Mvc;
using Vocabulary.Application.DTOs;
using Vocabulary.Application.Services;
using Vocabulary.Domain.Enums;

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
        [FromQuery] Level level,
        CancellationToken cancellationToken)
    {
        try
        {
            var question = await _questionService.GetNextQuestionAsync(
                userId,
                level,
                cancellationToken);

            if (question is null)
            {
                return NotFound("No questions available.");
            }

            return Ok(question);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
        catch (InvalidOperationException exception)
        {
            return StatusCode(
                StatusCodes.Status423Locked,
                new { message = exception.Message });
        }
    }

    [HttpPost("answer")]
    public async Task<IActionResult> SubmitAnswer(
        [FromBody] SubmitAnswerRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _questionService.SubmitAnswerAsync(
                request,
                cancellationToken);

            return Ok(response);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(exception.Message);
        }
        catch (KeyNotFoundException exception)
        {
            return NotFound(exception.Message);
        }
    }
}
