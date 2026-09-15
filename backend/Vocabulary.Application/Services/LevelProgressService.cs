using Vocabulary.Application.Abstractions.Persistence;
using Vocabulary.Application.DTOs;
using Vocabulary.Domain.Entities;
using Vocabulary.Domain.Enums;

namespace Vocabulary.Application.Services;

public class LevelProgressService : ILevelProgressService
{
    private const double UnlockThreshold = 85.0;

    private readonly IUserRepository _userRepository;
    private readonly IWordRepository _wordRepository;
    private readonly IUserMeaningProgressRepository _meaningProgressRepository;

    public LevelProgressService(
        IUserRepository userRepository,
        IWordRepository wordRepository,
        IUserMeaningProgressRepository meaningProgressRepository)
    {
        _userRepository = userRepository;
        _wordRepository = wordRepository;
        _meaningProgressRepository = meaningProgressRepository;
    }

    public async Task<List<LevelProgressDto>?> GetProgressAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return null;
        }

        var levels = Enum.GetValues<Level>();
        var progressByLevel = new Dictionary<Level, LevelProgressDto>();
        var unlockedLevels = new HashSet<Level> { Level.A1 };

        foreach (var level in levels)
        {
            var meanings = await _wordRepository
                .GetMeaningsByLevelAsync(level, cancellationToken);
            var meaningProgresses = await _meaningProgressRepository
                .GetMeaningProgressesByLevelAsync(
                    userId,
                    level,
                    cancellationToken);
            var levelProgress = await _userRepository
                .GetLevelProgressAsync(userId, level, cancellationToken);

            var learnedMeanings = meaningProgresses.Count(x => x.IsLearned);
            var totalMeanings = meanings.Count;

            progressByLevel[level] = new LevelProgressDto
            {
                Level = level,
                TotalMeanings = totalMeanings,
                LearnedMeanings = learnedMeanings,
                ProgressPercentage = totalMeanings == 0
                    ? 0
                    : (double)learnedMeanings / totalMeanings * 100,
                IsUnlocked = level == Level.A1 || levelProgress is not null
            };

            if (levelProgress is not null)
            {
                unlockedLevels.Add(level);
            }
        }

        for (var index = 0; index < levels.Length - 1; index++)
        {
            var currentLevel = levels[index];
            var nextLevel = levels[index + 1];

            if (progressByLevel[currentLevel].ProgressPercentage >= UnlockThreshold &&
                !unlockedLevels.Contains(nextLevel))
            {
                await _userRepository.AddLevelProgressAsync(
                    new UserLevelProgress
                    {
                        UserId = userId,
                        Level = nextLevel,
                        NextMeaningOrder = 0
                    },
                    cancellationToken);

                unlockedLevels.Add(nextLevel);
            }
        }

        return levels
            .Select(level =>
            {
                var progress = progressByLevel[level];
                progress.IsUnlocked = unlockedLevels.Contains(level);
                return progress;
            })
            .ToList();
    }
}
