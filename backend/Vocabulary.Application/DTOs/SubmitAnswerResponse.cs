namespace Vocabulary.Application.DTOs;

public class SubmitAnswerResponse
{
    public bool IsCorrect { get; set; }

    public string CorrectAnswer { get; set; } = null!;
}
