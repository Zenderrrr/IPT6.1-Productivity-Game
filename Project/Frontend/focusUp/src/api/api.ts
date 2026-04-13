const API_URL = "http://localhost:5165/api";

export async function apiFetch<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const res = await fetch(`${API_URL}${endpoint}`, {
    headers: {
      "Content-Type": "application/json",
      ...(options?.headers || {})
    },
    ...options
  });

  if(!res.ok){
    throw new Error(`Error: ${res.status}`)
  }
  return res.json()
}
