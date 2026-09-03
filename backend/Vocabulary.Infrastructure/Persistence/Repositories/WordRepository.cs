using Microsoft.EntityFrameworkCore;
using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Domain.Entities;
using Vocabulary.Infrastructure.Persistence.Context;

namespace Vocabulary.Infrastructure.Persistence.Repositories;

public class WordRepository : IWordRepository
{
    private readonly VocabularyDbContext _context;

    public WordRepository(VocabularyDbContext context)
    {
        _context = context;
    }

    public async Task<List<Word>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Words
            .AsNoTracking()
            .Include(x => x.Meanings)
            .OrderBy(x => x.Text)
            .ToListAsync(cancellationToken);
    }
}