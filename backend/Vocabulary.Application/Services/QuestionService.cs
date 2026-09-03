using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.DTOs;
using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Services;

public class QuestionService : IQuestionService
{
    private readonly IWordRepository _wordRepository;
    private readonly IUserMeaningProgressRepository _meaningProgressRepository;

    public QuestionService(
        IWordRepository wordRepository,
        IUserMeaningProgressRepository meaningProgressRepository)
    {
        _wordRepository = wordRepository;
        _meaningProgressRepository = meaningProgressRepository;
    }

    public async Task<QuestionDto?> GetNextQuestionAsync(
        Guid userId,
        Level level,
        CancellationToken cancellationToken = default)
    {
        var meanings = await _wordRepository
            .GetMeaningsByLevelAsync(level, cancellationToken);

        if (meanings.Count == 0)
        {
            return null;
        }

        var progresses = await _meaningProgressRepository
            .GetMeaningProgressesByLevelAsync(
                userId,
                level,
                cancellationToken);

        var progressByMeaningId = progresses.ToDictionary(x => x.WordMeaningId);
        var meaning = SelectWeightedMeaning(meanings, progressByMeaningId);

        var questionType = GetRandomQuestionType();

        return new QuestionDto
        {
            QuestionId = Guid.NewGuid(),
            WordMeaningId = meaning.Id,
            Word = meaning.Word.Text,
            Level = meaning.Level,
            PartOfSpeech = meaning.PartOfSpeech,
            QuestionType = questionType,
            Prompt = BuildPrompt(
                meaning.Word.Text,
                meaning.PartOfSpeech,
                questionType,
                meaning.TurkishMeaning),
            ImageUrl = meaning.Word.ImageUrl
        };
    }

    private static WordMeaning SelectWeightedMeaning(
        List<WordMeaning> meanings,
        IReadOnlyDictionary<Guid, UserMeaningProgress> progressByMeaningId)
    {
        var weightedMeanings = meanings
            .Select(meaning => new
            {
                Meaning = meaning,
                Weight = progressByMeaningId.TryGetValue(meaning.Id, out var progress)
                    ? progress.Weight
                    : 1.0
            })
            .Where(x => double.IsFinite(x.Weight) && x.Weight > 0)
            .ToList();

        var totalWeight = weightedMeanings.Sum(x => x.Weight);

        if (weightedMeanings.Count == 0 ||
            !double.IsFinite(totalWeight) ||
            totalWeight <= 0)
        {
            return meanings[Random.Shared.Next(meanings.Count)];
        }

        var selectedWeight = Random.Shared.NextDouble() * totalWeight;
        var cumulativeWeight = 0.0;

        foreach (var weightedMeaning in weightedMeanings)
        {
            cumulativeWeight += weightedMeaning.Weight;

            if (selectedWeight < cumulativeWeight)
            {
                return weightedMeaning.Meaning;
            }
        }

        return weightedMeanings[^1].Meaning;
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
