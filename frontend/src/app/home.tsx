import {
  ActivityIndicator,
  Button,
  Pressable,
  RefreshControl,
  ScrollView,
  StyleSheet,
  Text,
  View,
} from 'react-native';
import { Redirect, useRouter } from 'expo-router';

import { useDashboardData } from '@/hooks/use-dashboard-data';

export default function HomeRoute() {
  const { error, isLoading, levels, refresh, statistics, user } = useDashboardData();
  const router = useRouter();

  if (isLoading && !statistics) {
    return (
      <View style={styles.container}>
        <ActivityIndicator />
      </View>
    );
  }

  if (!user) {
    return <Redirect href="/" />;
  }

  return (
    <ScrollView
      contentContainerStyle={styles.content}
      refreshControl={
        <RefreshControl
          refreshing={isLoading && statistics !== null}
          onRefresh={() => void refresh()}
        />
      }>
      <Text style={styles.username}>Hello, {user.username}</Text>

      {error && (
        <View style={styles.errorContainer}>
          <Text style={styles.error}>{error}</Text>
          <Button title="Try again" onPress={() => void refresh()} />
        </View>
      )}

      {statistics && (
        <View style={styles.section}>
          <Text>Total learned meanings: {statistics.totalLearnedMeanings}</Text>
          <Text>Total correct answers: {statistics.totalCorrectAnswers}</Text>
          <Text>Total wrong answers: {statistics.totalWrongAnswers}</Text>
        </View>
      )}

      <Pressable
        style={styles.startButton}
        onPress={() => router.push({ pathname: '/question', params: { level: 'A1' } } as any)}>
        <Text style={styles.startButtonText}>Start learning</Text>
      </Pressable>

      <View style={styles.section}>
        <Text style={styles.sectionTitle}>Levels</Text>
        {levels.map((level) => (
          <View key={level.level} style={styles.levelRow}>
            <View>
              <Text>{level.level}</Text>
              <Text>
                {level.learnedMeanings}/{level.totalMeanings} learned
              </Text>
            </View>
            <View style={level.isUnlocked ? styles.unlocked : styles.locked}>
              <Text>{level.progressPercentage.toFixed(0)}%</Text>
              <Text>{level.isUnlocked ? 'Unlocked' : 'Locked'}</Text>
            </View>
          </View>
        ))}
      </View>
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
  },
  content: {
    padding: 24,
    gap: 16,
  },
  username: {
    fontSize: 22,
    fontWeight: '600',
  },
  section: {
    gap: 8,
  },
  sectionTitle: {
    fontSize: 18,
    fontWeight: '600',
  },
  levelRow: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    alignItems: 'center',
    borderWidth: 1,
    borderColor: '#d0d0d0',
    borderRadius: 8,
    padding: 12,
  },
  unlocked: {
    alignItems: 'flex-end',
  },
  locked: {
    alignItems: 'flex-end',
    opacity: 0.45,
  },
  errorContainer: {
    gap: 8,
  },
  error: {
    color: '#b00020',
  },
  startButton: {
    backgroundColor: '#2563eb',
    borderRadius: 10,
    padding: 14,
  },
  startButtonText: {
    color: '#ffffff',
    fontSize: 16,
    fontWeight: '600',
    textAlign: 'center',
  },
});
