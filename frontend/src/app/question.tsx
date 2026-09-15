import { useLocalSearchParams, useRouter } from 'expo-router';
import {
  ActivityIndicator,
  Pressable,
  StyleSheet,
  Text,
  View,
} from 'react-native';

import { useQuestionData } from '@/hooks/use-question-data';
import type { LearningLevel } from '@/types/levelProgress';

const supportedLevels: LearningLevel[] = ['A1', 'A2', 'B1', 'B2', 'C1', 'C2'];

function getRequestedLevel(value: string | string[] | undefined): LearningLevel {
  const level = Array.isArray(value) ? value[0] : value;
  return supportedLevels.includes(level as LearningLevel) ? (level as LearningLevel) : 'A1';
}

export default function QuestionRoute() {
  const { level: levelParam } = useLocalSearchParams<{ level?: string }>();
  const level = getRequestedLevel(levelParam);
  const router = useRouter();
  const {
    answer,
    answerResult,
    error,
    isAnswering,
    isLoading,
    loadNextQuestion,
    question,
    selectedAnswer,
  } = useQuestionData(level);

  if (isLoading && !question) {
    return (
      <View style={styles.centered}>
        <ActivityIndicator />
        <Text>Loading question…</Text>
      </View>
    );
  }

  if (!question) {
    return (
      <View style={styles.centered}>
        <Text style={styles.error}>{error ?? 'No question is available.'}</Text>
        <Pressable style={styles.primaryButton} onPress={() => void loadNextQuestion()}>
          <Text style={styles.primaryButtonText}>Try again</Text>
        </Pressable>
        <Pressable onPress={() => router.back()}>
          <Text style={styles.link}>Back to home</Text>
        </Pressable>
      </View>
    );
  }

  const hasAnswered = answerResult !== null;

  return (
    <View style={styles.container}>
      <View style={styles.header}>
        <Text style={styles.level}>{question.level}</Text>
        <Text style={styles.questionType}>
          {question.questionType === 'EnglishToTurkish'
            ? 'English → Turkish'
            : 'Turkish → English'}
        </Text>
      </View>

      <View style={styles.questionCard}>
        <Text style={styles.prompt}>{question.prompt}</Text>
        <Text style={styles.partOfSpeech}>({question.partOfSpeech})</Text>
      </View>

      <View style={styles.options}>
        {question.options.map((option) => {
          const isSelected = option === selectedAnswer;
          const isCorrectOption = hasAnswered && option === answerResult.correctAnswer;
          const isWrongSelection = hasAnswered && isSelected && !answerResult.isCorrect;

          return (
            <Pressable
              key={option}
              disabled={hasAnswered || isAnswering}
              onPress={() => void answer(option)}
              style={[
                styles.option,
                isSelected && !hasAnswered && styles.optionSelected,
                isCorrectOption && styles.optionCorrect,
                isWrongSelection && styles.optionWrong,
                (hasAnswered || isAnswering) && !isSelected && !isCorrectOption && styles.optionDisabled,
              ]}>
              <Text style={styles.optionText}>{option}</Text>
            </Pressable>
          );
        })}
      </View>

      {isAnswering && <Text>Checking answer…</Text>}

      {error && <Text style={styles.error}>{error}</Text>}

      {hasAnswered && (
        <View style={styles.feedback}>
          <Text style={answerResult.isCorrect ? styles.correctText : styles.wrongText}>
            {answerResult.isCorrect ? 'Correct!' : 'Not quite.'}
          </Text>
          {!answerResult.isCorrect && (
            <Text>Correct answer: {answerResult.correctAnswer}</Text>
          )}
          <Pressable style={styles.primaryButton} onPress={() => void loadNextQuestion()}>
            <Text style={styles.primaryButtonText}>Next question</Text>
          </Pressable>
        </View>
      )}
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    padding: 24,
    gap: 20,
    justifyContent: 'center',
  },
  centered: {
    flex: 1,
    alignItems: 'center',
    justifyContent: 'center',
    gap: 16,
    padding: 24,
  },
  header: {
    flexDirection: 'row',
    justifyContent: 'space-between',
  },
  level: {
    fontSize: 18,
    fontWeight: '700',
  },
  questionType: {
    color: '#4b5563',
  },
  questionCard: {
    alignItems: 'center',
    gap: 8,
    paddingVertical: 28,
  },
  prompt: {
    fontSize: 32,
    fontWeight: '700',
    textAlign: 'center',
  },
  partOfSpeech: {
    color: '#4b5563',
    fontSize: 16,
  },
  options: {
    gap: 12,
  },
  option: {
    borderWidth: 1,
    borderColor: '#cbd5e1',
    borderRadius: 12,
    minHeight: 56,
    justifyContent: 'center',
    paddingHorizontal: 16,
  },
  optionSelected: {
    borderColor: '#2563eb',
    backgroundColor: '#dbeafe',
  },
  optionCorrect: {
    borderColor: '#15803d',
    backgroundColor: '#dcfce7',
  },
  optionWrong: {
    borderColor: '#b91c1c',
    backgroundColor: '#fee2e2',
  },
  optionDisabled: {
    opacity: 0.45,
  },
  optionText: {
    fontSize: 17,
  },
  feedback: {
    gap: 10,
    alignItems: 'center',
  },
  correctText: {
    color: '#15803d',
    fontSize: 18,
    fontWeight: '700',
  },
  wrongText: {
    color: '#b91c1c',
    fontSize: 18,
    fontWeight: '700',
  },
  error: {
    color: '#b91c1c',
    textAlign: 'center',
  },
  primaryButton: {
    backgroundColor: '#2563eb',
    borderRadius: 10,
    paddingHorizontal: 20,
    paddingVertical: 14,
  },
  primaryButtonText: {
    color: '#ffffff',
    fontSize: 16,
    fontWeight: '600',
    textAlign: 'center',
  },
  link: {
    color: '#2563eb',
  },
});
