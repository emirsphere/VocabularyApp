export type LearningLevel = 'A1' | 'A2' | 'B1' | 'B2' | 'C1' | 'C2';

export interface LevelProgress {
  level: LearningLevel;
  totalMeanings: number;
  learnedMeanings: number;
  progressPercentage: number;
  isUnlocked: boolean;
}
