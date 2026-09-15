const configuredApiBaseUrl = process.env.EXPO_PUBLIC_API_BASE_URL?.replace(/\/+$/, '');

export const API_BASE_URL = configuredApiBaseUrl ?? '';

export function getApiUrl(path: string): string {
  if (!API_BASE_URL) {
    throw new Error('API base URL is not configured. Set EXPO_PUBLIC_API_BASE_URL.');
  }

  return `${API_BASE_URL}${path.startsWith('/') ? path : `/${path}`}`;
}
