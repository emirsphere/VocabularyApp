using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.DTOs;
using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Services;

public class QuestionService : IQuestionService
{
    private static readonly QuestionType[] ActiveQuestionTypes =
    [
        QuestionType.EnglishToTurkish,
        QuestionType.TurkishToEnglish
    ];

    private readonly IWordRepository _wordRepository;
    private readonly IUserMeaningProgressRepository _meaningProgressRepository;
    private readonly IUserQuestionProgressRepository _questionProgressRepository;
    private readonly IUserRepository _userRepository;

    public QuestionService(
        IWordRepository wordRepository,
        IUserMeaningProgressRepository meaningProgressRepository,
        IUserQuestionProgressRepository questionProgressRepository,
        IUserRepository userRepository)
    {
        _wordRepository = wordRepository;
        _meaningProgressRepository = meaningProgressRepository;
        _questionProgressRepository = questionProgressRepository;
        _userRepository = userRepository;
    }

    public async Task<QuestionDto?> GetNextQuestionAsync(
        Guid userId,
        Level level,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        if (level != Level.A1)
        {
            var levelProgress = await _userRepository.GetLevelProgressAsync(
                userId,
                level,
                cancellationToken);

            if (levelProgress is null)
            {
                throw new InvalidOperationException(
                    "The requested level is locked.");
            }
        }

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

        var questionProgresses = await _questionProgressRepository
            .GetQuestionProgressesAsync(
                userId,
                meaning.Id,
                cancellationToken);

        var progressByQuestionType = questionProgresses
            .ToDictionary(x => x.QuestionType);
        var questionType = SelectWeightedQuestionType(progressByQuestionType);
        var options = await BuildOptionsAsync(
            meaning,
            meanings,
            questionType,
            cancellationToken);

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
            ImageUrl = meaning.Word.ImageUrl,
            Options = options
        };
    }

    public async Task<SubmitAnswerResponse> SubmitAnswerAsync(
        SubmitAnswerRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!ActiveQuestionTypes.Contains(request.QuestionType))
        {
            throw new ArgumentException("The question type is not currently active.");
        }

        var user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
        {
            throw new KeyNotFoundException("User not found.");
        }

        var meaning = await _wordRepository.GetMeaningByIdAsync(
            request.WordMeaningId,
            cancellationToken);

        if (meaning is null)
        {
            throw new KeyNotFoundException("Word meaning not found.");
        }

        var correctAnswer = request.QuestionType switch
        {
            QuestionType.EnglishToTurkish => meaning.TurkishMeaning,
            QuestionType.TurkishToEnglish => meaning.Word.Text,
            _ => throw new ArgumentException("The question type is not currently active.")
        };

        var isCorrect = string.Equals(
            NormalizeAnswer(request.Answer),
            NormalizeAnswer(correctAnswer),
            StringComparison.Ordinal);

        var meaningProgress = await _meaningProgressRepository
            .GetMeaningProgressAsync(
                request.UserId,
                meaning.Id,
                cancellationToken)
            ?? new UserMeaningProgress
            {
                UserId = request.UserId,
                WordMeaningId = meaning.Id,
                Weight = 1.0,
                CorrectCount = 0,
                WrongCount = 0
            };

        var questionProgress = await _questionProgressRepository
            .GetQuestionProgressAsync(
                request.UserId,
                meaning.Id,
                request.QuestionType,
                cancellationToken)
            ?? new UserQuestionProgress
            {
                UserId = request.UserId,
                WordMeaningId = meaning.Id,
                QuestionType = request.QuestionType,
                Weight = 1.0,
                CorrectCount = 0,
                WrongCount = 0
            };

        if (isCorrect)
        {
            meaningProgress.CorrectCount += 1;
            meaningProgress.Weight = Math.Max(0.05, meaningProgress.Weight / 2);
            questionProgress.CorrectCount += 1;
            questionProgress.Weight = Math.Max(0.05, questionProgress.Weight / 2);
        }
        else
        {
            meaningProgress.WrongCount += 1;
            meaningProgress.Weight = Math.Min(5.0, meaningProgress.Weight * 2);
            questionProgress.WrongCount += 1;
            questionProgress.Weight = Math.Min(5.0, questionProgress.Weight * 2);
        }

        meaningProgress.IsLearned = meaningProgress.CorrectCount >= 3 &&
                                    meaningProgress.CorrectCount >
                                    meaningProgress.WrongCount;
        meaningProgress.LastAnsweredAt = DateTime.UtcNow;

        if (meaningProgress.Id == Guid.Empty)
        {
            await _meaningProgressRepository.AddMeaningProgressAsync(
                meaningProgress,
                cancellationToken);
        }

        if (questionProgress.Id == Guid.Empty)
        {
            await _questionProgressRepository.AddQuestionProgressAsync(
                questionProgress,
                cancellationToken);
        }

        await _meaningProgressRepository.SaveChangesAsync(cancellationToken);

        return new SubmitAnswerResponse
        {
            IsCorrect = isCorrect,
            CorrectAnswer = correctAnswer
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

    private static QuestionType SelectWeightedQuestionType(
        IReadOnlyDictionary<QuestionType, UserQuestionProgress>
            progressByQuestionType)
    {
        var weightedQuestionTypes = ActiveQuestionTypes
            .Select(questionType => new
            {
                QuestionType = questionType,
                Weight = progressByQuestionType.TryGetValue(questionType, out var progress)
                    ? progress.Weight
                    : 1.0
            })
            .Where(x => double.IsFinite(x.Weight) && x.Weight > 0)
            .ToList();

        var totalWeight = weightedQuestionTypes.Sum(x => x.Weight);

        if (weightedQuestionTypes.Count == 0 ||
            !double.IsFinite(totalWeight) ||
            totalWeight <= 0)
        {
            return ActiveQuestionTypes[
                Random.Shared.Next(ActiveQuestionTypes.Length)];
        }

        var selectedWeight = Random.Shared.NextDouble() * totalWeight;
        var cumulativeWeight = 0.0;

        foreach (var weightedQuestionType in weightedQuestionTypes)
        {
            cumulativeWeight += weightedQuestionType.Weight;

            if (selectedWeight < cumulativeWeight)
            {
                return weightedQuestionType.QuestionType;
            }
        }

        return weightedQuestionTypes[^1].QuestionType;
    }

    private async Task<List<string>> BuildOptionsAsync(
        WordMeaning meaning,
        List<WordMeaning> levelMeanings,
        QuestionType questionType,
        CancellationToken cancellationToken)
    {
        var correctAnswer = GetAnswerText(meaning, questionType);
        var options = new List<string> { correctAnswer };
        var usedMeaningIds = new HashSet<Guid> { meaning.Id };
        var usedAnswers = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            correctAnswer
        };

        var levelCandidates = levelMeanings
            .Select(ToOptionCandidate)
            .ToList();

        AddDistinctOptions(
            levelCandidates.Where(x => x.PartOfSpeech == meaning.PartOfSpeech),
            questionType,
            options,
            usedMeaningIds,
            usedAnswers);

        AddDistinctOptions(
            levelCandidates.Where(x => x.PartOfSpeech != meaning.PartOfSpeech),
            questionType,
            options,
            usedMeaningIds,
            usedAnswers);

        if (options.Count < 3)
        {
            var allWords = await _wordRepository.GetAllAsync(cancellationToken);
            var fallbackCandidates = allWords
                .SelectMany(word => word.Meanings.Select(meaning =>
                    new OptionCandidate(
                        meaning.Id,
                        meaning.PartOfSpeech,
                        word.Text,
                        meaning.TurkishMeaning)))
                .ToList();

            AddDistinctOptions(
                fallbackCandidates,
                questionType,
                options,
                usedMeaningIds,
                usedAnswers);
        }

        Shuffle(options);
        return options;
    }

    private static OptionCandidate ToOptionCandidate(WordMeaning meaning)
    {
        return new OptionCandidate(
            meaning.Id,
            meaning.PartOfSpeech,
            meaning.Word.Text,
            meaning.TurkishMeaning);
    }

    private static void AddDistinctOptions(
        IEnumerable<OptionCandidate> candidates,
        QuestionType questionType,
        List<string> options,
        HashSet<Guid> usedMeaningIds,
        HashSet<string> usedAnswers)
    {
        var shuffledCandidates = candidates.ToList();
        Shuffle(shuffledCandidates);

        foreach (var candidate in shuffledCandidates)
        {
            if (options.Count == 3)
            {
                return;
            }

            var answer = GetAnswerText(candidate, questionType);

            if (!usedMeaningIds.Add(candidate.Id) || !usedAnswers.Add(answer))
            {
                continue;
            }

            options.Add(answer);
        }
    }

    private static string GetAnswerText(
        WordMeaning meaning,
        QuestionType questionType)
    {
        return questionType == QuestionType.EnglishToTurkish
            ? meaning.TurkishMeaning
            : meaning.Word.Text;
    }

    private static string GetAnswerText(
        OptionCandidate candidate,
        QuestionType questionType)
    {
        return questionType == QuestionType.EnglishToTurkish
            ? candidate.TurkishMeaning
            : candidate.Word;
    }

    private static void Shuffle<T>(IList<T> items)
    {
        for (var index = items.Count - 1; index > 0; index--)
        {
            var swapIndex = Random.Shared.Next(index + 1);
            (items[index], items[swapIndex]) = (items[swapIndex], items[index]);
        }
    }

    private sealed record OptionCandidate(
        Guid Id,
        string PartOfSpeech,
        string Word,
        string TurkishMeaning);

    private static string NormalizeAnswer(string value)
    {
        return string.Join(
                " ",
                value.Trim().Split(
                    (char[]?)null,
                    StringSplitOptions.RemoveEmptyEntries))
            .ToLowerInvariant();
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
