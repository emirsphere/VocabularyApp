using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Vocabulary.Domain.Entities;
using Vocabulary.Infrastructure.Data.Models;
using Vocabulary.Infrastructure.Persistence.Context;

namespace Vocabulary.Infrastructure.Data.Seed;

public class VocabularySeedService
{
    private readonly VocabularyDbContext _context;

    public VocabularySeedService(VocabularyDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        var filePath = Path.Combine(
            AppContext.BaseDirectory,
            "Data",
            "Seed",
            "words.json");

        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException(
                "words.json bulunamadı.",
                filePath);
        }

        var json = await File.ReadAllTextAsync(filePath);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        options.Converters.Add(new JsonStringEnumConverter());

        var seedWords = JsonSerializer.Deserialize<List<WordSeedModel>>(
            json,
            options);

        if (seedWords is null || seedWords.Count == 0)
        {
            return;
        }

        // Get existing words to avoid duplicates
        var existingWords = await _context.Words
            .Include(w => w.Meanings)
            .ToDictionaryAsync(w => w.Text.ToLower());

        var wordsToAdd = new List<Word>();
        var changesDetected = false;

        foreach (var seedWord in seedWords)
        {
            var seedWordKeyLower = seedWord.Text.ToLower();

            if (existingWords.TryGetValue(seedWordKeyLower, out var existingWord))
            {
                // Word exists - check if all meanings exist
                var existingMeaningSet = existingWord.Meanings
                    .ToDictionary(m => (m.Level, m.PartOfSpeech, m.TurkishMeaning.ToLower()));

                foreach (var seedMeaning in seedWord.Meanings)
                {
                    var meaningKey = (seedMeaning.Level, seedMeaning.PartOfSpeech, seedMeaning.TurkishMeaning.ToLower());

                    // Only add if this exact meaning doesn't exist
                    if (!existingMeaningSet.ContainsKey(meaningKey))
                    {
                        existingWord.Meanings.Add(new WordMeaning
                        {
                            Id = Guid.NewGuid(),
                            Level = seedMeaning.Level,
                            Order = seedMeaning.Order,
                            PartOfSpeech = seedMeaning.PartOfSpeech,
                            TurkishMeaning = seedMeaning.TurkishMeaning
                        });
                        changesDetected = true;
                    }
                }
            }
            else
            {
                // Word doesn't exist - create new word with meanings
                var word = new Word
                {
                    Id = Guid.NewGuid(),
                    Text = seedWord.Text,
                    ImageUrl = seedWord.ImageUrl
                };

                foreach (var seedMeaning in seedWord.Meanings)
                {
                    word.Meanings.Add(new WordMeaning
                    {
                        Id = Guid.NewGuid(),
                        Level = seedMeaning.Level,
                        Order = seedMeaning.Order,
                        PartOfSpeech = seedMeaning.PartOfSpeech,
                        TurkishMeaning = seedMeaning.TurkishMeaning
                    });
                }

                wordsToAdd.Add(word);
                changesDetected = true;
            }
        }

        if (wordsToAdd.Count > 0 || changesDetected)
        {
            if (wordsToAdd.Count > 0)
            {
                _context.Words.AddRange(wordsToAdd);
            }

            await _context.SaveChangesAsync();
        }
    }
}