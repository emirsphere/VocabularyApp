using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.DTOs;

namespace Vocabulary.Application.Services;

public class WordService : IWordService
{
    private readonly IWordRepository _wordRepository;

    public WordService(IWordRepository wordRepository)
    {
        _wordRepository = wordRepository;
    }

    public async Task<List<WordDto>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        var words = await _wordRepository
            .GetAllAsync(cancellationToken);

        return words.Select(word => new WordDto
        {
            Id = word.Id,
            Text = word.Text,
            ImageUrl = word.ImageUrl,

            Meanings = word.Meanings
                .Select(meaning => new WordMeaningDto
                {
                    Id = meaning.Id,
                    Level = meaning.Level,
                    Order = meaning.Order,
                    PartOfSpeech = meaning.PartOfSpeech,
                    TurkishMeaning = meaning.TurkishMeaning,
                    ExampleSentence = meaning.ExampleSentence
                })
                .ToList()
        }).ToList();
    }
}