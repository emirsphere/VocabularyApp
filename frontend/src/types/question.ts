import type { LearningLevel } from './levelProgress';

export type ActiveQuestionType = 'EnglishToTurkish' | 'TurkishToEnglish';

export interface Question {
  questionId: string;
  wordMeaningId: string;
  word: string;
  level: LearningLevel;
  partOfSpeech: string;
  questionType: ActiveQuestionType;
  prompt: string;
  imageUrl: string | null;
  options: string[];
}

export interface SubmitAnswerRequest {
  userId: string;
  wordMeaningId: string;
  questionType: ActiveQuestionType;
  answer: string;
}

export interface SubmitAnswerResponse {
  isCorrect: boolean;
  correctAnswer: string;
}
