import { fetchNextQuestion, submitAnswer } from '@/api/questionsApi';
import type { LearningLevel } from '@/types/levelProgress';
import type {
  Question,
  SubmitAnswerResponse,
} from '@/types/question';

export function getNextQuestion(
  userId: string,
  level: LearningLevel,
): Promise<Question> {
  return fetchNextQuestion(userId, level);
}

export function submitQuestionAnswer(
  userId: string,
  question: Question,
  answer: string,
): Promise<SubmitAnswerResponse> {
  return submitAnswer({
    userId,
    wordMeaningId: question.wordMeaningId,
    questionType: question.questionType,
    answer,
  });
}
