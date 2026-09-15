import { getApiUrl } from '@/constants/api';
import type { User } from '@/types/user';

export async function createOrGetUser(username: string): Promise<User> {
  const response = await fetch(getApiUrl('/api/Users'), {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ username }),
  });

  if (!response.ok) {
    throw new Error(`Unable to create or restore the user (${response.status}).`);
  }

  return response.json() as Promise<User>;
}
