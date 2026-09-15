import { useCallback, useEffect, useState } from 'react';

import { loadDashboardData } from '@/services/dashboardService';
import type { LevelProgress } from '@/types/levelProgress';
import type { UserStatistics } from '@/types/userStatistics';
import type { User } from '@/types/user';

import { useUserSession } from './use-user-session';

type DashboardData = {
  user: User | null;
  statistics: UserStatistics | null;
  levels: LevelProgress[];
  isLoading: boolean;
  error: string | null;
  refresh: () => Promise<void>;
};

export function useDashboardData(): DashboardData {
  const { isLoading: isSessionLoading, user } = useUserSession();
  const [statistics, setStatistics] = useState<UserStatistics | null>(null);
  const [levels, setLevels] = useState<LevelProgress[]>([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const refresh = useCallback(async () => {
    if (!user) {
      return;
    }

    setIsLoading(true);
    setError(null);

    try {
      const dashboardData = await loadDashboardData(user.id);
      setStatistics(dashboardData.statistics);
      setLevels(dashboardData.levels);
    } catch (requestError) {
      setError(
        requestError instanceof Error
          ? requestError.message
          : 'Unable to load dashboard data. Please try again.');
    } finally {
      setIsLoading(false);
    }
  }, [user]);

  useEffect(() => {
    if (!isSessionLoading && user) {
      void refresh();
    }
  }, [isSessionLoading, refresh, user]);

  return {
    user,
    statistics,
    levels,
    isLoading: isSessionLoading || isLoading,
    error,
    refresh,
  };
}
