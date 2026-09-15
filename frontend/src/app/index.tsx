import { ActivityIndicator, StyleSheet, View } from 'react-native';
import { useEffect } from 'react';
import { useRouter } from 'expo-router';

import { useUserSession } from '@/hooks/use-user-session';

export default function IndexRoute() {
  const router = useRouter();
  const { isLoading, user } = useUserSession();

  useEffect(() => {
    if (!isLoading) {
      router.replace(user ? '/home' : '/onboarding');
    }
  }, [isLoading, router, user]);

  return (
    <View style={styles.container}>
      <ActivityIndicator />
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
});
