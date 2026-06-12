import { useAuthStore } from '@/stores/authStore.ts'

const API_URL = `${import.meta.env.VITE_API_URL}/api`

const AUTH_ENDPOINTS = [
  '/Auth/login',
  '/Auth/register',
  '/Auth/refresh',
  '/Auth/logout',
]

export async function apiFetch<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const authStore = useAuthStore()

  const isAuthEndpoint = AUTH_ENDPOINTS.includes(endpoint)

  let res = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    credentials: 'include',
    headers: {
      'Content-Type': 'application/json',
      ...(options?.headers || {}),
      ...(authStore.token && !isAuthEndpoint
        ? { Authorization: `Bearer ${authStore.token}` } : {}),
    },
  })

  if ((res.status === 401 || res.status === 403) && !isAuthEndpoint) {
    try {
      await authStore.refresh()

      res = await fetch(`${API_URL}${endpoint}`, {
        ...options,
        credentials: 'include',
        headers: {
          'Content-Type': 'application/json',
          ...(options?.headers || {}),
          ...(authStore.token
            ? { Authorization: `Bearer ${authStore.token}` }
            : {}),
        },
      })
    } catch {
      await authStore.logout()
      throw new Error('Session abgelaufen. Bitte erneut einloggen.')
    }
  }

  if (!res.ok) {
    const text = await res.text()
    throw new Error(`ERROR: ${res.status}: ${text}`)
  }

  return res.json();
}
