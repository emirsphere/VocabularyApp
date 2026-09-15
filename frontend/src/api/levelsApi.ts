import { getApiUrl } from '@/constants/api';
import type { LevelProgress } from '@/types/levelProgress';

export async function fetchLevelProgress(userId: string): Promise<LevelProgress[]> {
  const query = new URLSearchParams({ userId });
  const response = await fetch(getApiUrl(`/api/levels/progress?${query.toString()}`));

  if (!response.ok) {
    throw new Error(`Unable to load level progress (${response.status}).`);
  }

  return response.json() as Promise<LevelProgress[]>;
}
