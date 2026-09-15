import { useCallback, useEffect, useState } from 'react';

import { getNextQuestion, submitQuestionAnswer } from '@/services/questionService';
import type { LearningLevel } from '@/types/levelProgress';
import type { Question, SubmitAnswerResponse } from '@/types/question';

import { useUserSession } from './use-user-session';

type QuestionData = {
  question: Question | null;
  selectedAnswer: string | null;
  answerResult: SubmitAnswerResponse | null;
  isLoading: boolean;
  isAnswering: boolean;
  error: string | null;
  answer: (option: string) => Promise<void>;
  loadNextQuestion: () => Promise<void>;
};

export function useQuestionData(level: LearningLevel): QuestionData {
  const { isLoading: isSessionLoading, user } = useUserSession();
  const [question, setQuestion] = useState<Question | null>(null);
  const [selectedAnswer, setSelectedAnswer] = useState<string | null>(null);
  const [answerResult, setAnswerResult] = useState<SubmitAnswerResponse | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [isAnswering, setIsAnswering] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const loadNextQuestion = useCallback(async () => {
    if (!user) {
      return;
    }

    setIsLoading(true);
    setError(null);
    setQuestion(null);
    setSelectedAnswer(null);
    setAnswerResult(null);

    try {
      setQuestion(await getNextQuestion(user.id, level));
    } catch (requestError) {
      setError(
        requestError instanceof Error
          ? requestError.message
          : 'Unable to load a question. Please try again.',
      );
    } finally {
      setIsLoading(false);
    }
  }, [level, user]);

  const answer = useCallback(async (option: string) => {
    if (!user || !question || selectedAnswer !== null || isAnswering || answerResult) {
      return;
    }

    setSelectedAnswer(option);
    setIsAnswering(true);
    setError(null);

    try {
      setAnswerResult(await submitQuestionAnswer(user.id, question, option));
    } catch (requestError) {
      setSelectedAnswer(null);
      setError(
        requestError instanceof Error
          ? requestError.message
          : 'Unable to submit the answer. Please try again.',
      );
    } finally {
      setIsAnswering(false);
    }
  }, [answerResult, isAnswering, question, selectedAnswer, user]);

  useEffect(() => {
    if (!isSessionLoading && user) {
      void loadNextQuestion();
    }
  }, [isSessionLoading, loadNextQuestion, user]);

  return {
    question,
    selectedAnswer,
    answerResult,
    isLoading: isSessionLoading || isLoading,
    isAnswering,
    error,
    answer,
    loadNextQuestion,
  };
}
