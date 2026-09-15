import AsyncStorage from '@react-native-async-storage/async-storage';

import type { User } from '@/types/user';

const USER_STORAGE_KEY = '@vocabulary-app/user';

function isStoredUser(value: unknown): value is User {
  if (!value || typeof value !== 'object') {
    return false;
  }

  const user = value as Partial<User>;
  return typeof user.id === 'string' &&
         user.id.length > 0 &&
         typeof user.username === 'string' &&
         user.username.length > 0 &&
         typeof user.createdAt === 'string';
}

export async function getStoredUser(): Promise<User | null> {
  try {
    const serializedUser = await AsyncStorage.getItem(USER_STORAGE_KEY);

    if (!serializedUser) {
      return null;
    }

    const user: unknown = JSON.parse(serializedUser);
    return isStoredUser(user) ? user : null;
  } catch (error) {
    console.warn('Unable to load the stored user.', error);
    return null;
  }
}

export async function saveUser(user: User): Promise<boolean> {
  try {
    await AsyncStorage.setItem(USER_STORAGE_KEY, JSON.stringify(user));
    return true;
  } catch (error) {
    console.warn('Unable to save the user.', error);
    return false;
  }
}

export async function clearStoredUser(): Promise<boolean> {
  try {
    await AsyncStorage.removeItem(USER_STORAGE_KEY);
    return true;
  } catch (error) {
    console.warn('Unable to clear the stored user.', error);
    return false;
  }
}
