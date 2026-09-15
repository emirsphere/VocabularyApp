import { getApiUrl } from '@/constants/api';
import type { UserStatistics } from '@/types/userStatistics';

export async function fetchUserStatistics(userId: string): Promise<UserStatistics> {
  const response = await fetch(getApiUrl(`/api/Users/${userId}/stats`));

  if (!response.ok) {
    throw new Error(`Unable to load user statistics (${response.status}).`);
  }

  return response.json() as Promise<UserStatistics>;
}
