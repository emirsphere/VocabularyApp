import { getApiUrl } from '@/constants/api';
import type {
  Question,
  SubmitAnswerRequest,
  SubmitAnswerResponse,
} from '@/types/question';
import type { LearningLevel } from '@/types/levelProgress';

export async function fetchNextQuestion(
  userId: string,
  level: LearningLevel,
): Promise<Question> {
  const query = new URLSearchParams({ userId, level });
  const response = await fetch(getApiUrl(`/api/Questions/next?${query.toString()}`));

  if (!response.ok) {
    throw new Error(`Unable to load a question (${response.status}).`);
  }

  return response.json() as Promise<Question>;
}

export async function submitAnswer(
  request: SubmitAnswerRequest,
): Promise<SubmitAnswerResponse> {
  const response = await fetch(getApiUrl('/api/Questions/answer'), {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(request),
  });

  if (!response.ok) {
    throw new Error(`Unable to submit the answer (${response.status}).`);
  }

  return response.json() as Promise<SubmitAnswerResponse>;
}
