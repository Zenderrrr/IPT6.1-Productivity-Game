const API_URL = "http://localhost:5165/api";

export async function apiFetch<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers: {
      "Content-Type": "application/json",
      ...(options?.headers || {})
    },
  });

  if (!res.ok) {
    const text = await res.text()

    console.log(`Error ${res.status}: ${text}`)

    throw new Error(`Error ${res.status}: ${text}`)
  }

  return res.json()
}
