import { createOrGetUser } from '@/api/usersApi';
import { saveUser } from '@/storage/userStorage';
import type { User } from '@/types/user';

export async function createAndStoreUser(username: string): Promise<User> {
  const user = await createOrGetUser(username);
  const wasStored = await saveUser(user);

  if (!wasStored) {
    throw new Error('Your user could not be saved on this device. Please try again.');
  }

  return user;
}
