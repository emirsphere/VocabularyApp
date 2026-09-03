using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.DTOs;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IWordRepository _wordRepository;

    public QuestionService(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public async Task<QuestionDto?> GetNextQuestionAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var words = await _wordRepository.GetAllAsync(cancellationToken);

        var a1Meanings = words
            .SelectMany(x => x.Meanings)
            .Where(x => x.Level == Level.A1)
            .ToList();

        if (a1Meanings.Count == 0)
        {
            return null;
        }

        var meaning = a1Meanings[
            Random.Shared.Next(a1Meanings.Count)
        ];

        var questionType = GetRandomQuestionType();

        var word = words.First(x =>
            x.Meanings.Any(m => m.Id == meaning.Id));

        return new QuestionDto
        {
            QuestionId = Guid.NewGuid(),
            WordMeaningId = meaning.Id,
            Word = word.Text,
            Level = meaning.Level,
            PartOfSpeech = meaning.PartOfSpeech,
            QuestionType = questionType,
            Prompt = BuildPrompt(
                word.Text,
                meaning.PartOfSpeech,
                questionType,
                meaning.TurkishMeaning),
            ImageUrl = word.ImageUrl
        };
    }

    private static QuestionType GetRandomQuestionType()
{
    return (QuestionType)Random.Shared.Next(1, 4);
}

    private static string BuildPrompt(
        string word,
        string partOfSpeech,
        QuestionType questionType,
        string turkishMeaning)
    {
        return questionType switch
        {
            QuestionType.EnglishToTurkish =>
                $"{word} ({partOfSpeech})",

            QuestionType.TurkishToEnglish =>
                turkishMeaning,

            QuestionType.ImageToEnglish =>
                "Bu görseldeki kelime nedir?",

            _ => word
        };
    }
}