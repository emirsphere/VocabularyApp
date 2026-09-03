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
        if (await _context.Words.AnyAsync())
        {
            return;
        }

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

        foreach (var seedWord in seedWords)
        {
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

            _context.Words.Add(word);
        }

        await _context.SaveChangesAsync();
    }
}