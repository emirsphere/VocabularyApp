import { fetchLevelProgress } from '@/api/levelsApi';
import { fetchUserStatistics } from '@/api/statisticsApi';
import type { LevelProgress } from '@/types/levelProgress';
import type { UserStatistics } from '@/types/userStatistics';

type DashboardData = {
  statistics: UserStatistics;
  levels: LevelProgress[];
};

export async function loadDashboardData(userId: string): Promise<DashboardData> {
  const [statistics, levels] = await Promise.all([
    fetchUserStatistics(userId),
    fetchLevelProgress(userId),
  ]);

  return { statistics, levels };
}
