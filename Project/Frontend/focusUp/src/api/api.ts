import { useAuthStore } from '@/stores/authStore.ts'

const API_URL = "http://localhost:5165/api";

export async function apiFetch<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const authStore = useAuthStore()

  let res = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(authStore.token ? { Authorization: `Bearer ${authStore.token}` } : {}),
      ...(options?.headers || {})
    },
    credentials: 'include'
  });

  if(res.status === 401){
    try {
      await authStore.refresh()

      res = await fetch(`${API_URL}${endpoint}`, {
        ...options,
        headers: {
          "Content-Type": "application/json",
          ...(authStore.token ? { Authorization: `Bearer ${authStore.token}` } : {}),
          ...(options?.headers || {})
        },
        credentials: 'include'
      });
    }catch{
      throw new Error(`Session expired`)
    }
  }


  if (!res.ok) {
    const text = await res.text()

    console.log(`Error ${res.status}: ${text}`)

    throw new Error(`Error ${res.status}: ${text}`)
  }

  return res.json()
}
