import { useEffect, useState } from 'react';

import { getStoredUser } from '@/storage/userStorage';
import type { User } from '@/types/user';

type UserSession = {
  isLoading: boolean;
  user: User | null;
};

export function useUserSession(): UserSession {
  const [session, setSession] = useState<UserSession>({
    isLoading: true,
    user: null,
  });

  useEffect(() => {
    let isMounted = true;

    async function restoreUser() {
      const user = await getStoredUser();

      if (isMounted) {
        setSession({ isLoading: false, user });
      }
    }

    void restoreUser();

    return () => {
      isMounted = false;
    };
  }, []);

  return session;
}
