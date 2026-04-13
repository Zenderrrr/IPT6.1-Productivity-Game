import { apiFetch } from '@/api/api.ts'
import type { Dashboard } from '@/types/dashboard.ts'

export async function dashboardApi(taskLogLimit: string): Promise<Dashboard> {
  const params = new URLSearchParams({ taskLimit: taskLogLimit })

  return apiFetch(`/dashboard?${params}`, {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}
