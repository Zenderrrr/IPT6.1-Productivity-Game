import { apiFetch } from '@/api/api.ts'
import type { Dashboard } from '@/types/dashboard.ts'
import type { Stats } from '@/types/stats.ts'
import type { Productivity } from '@/types/productivity.ts'

export async function dashboardApi(taskLogLimit: string): Promise<Dashboard> {
  const params = new URLSearchParams({ taskLogLimit: taskLogLimit })

  return apiFetch(`/dashboard?${params}`, {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}

export async function getStatsApi(rangeInDay?: number, from?: Date, to?: Date ): Promise<Stats> {
  const params = new URLSearchParams({ rangeInDays: rangeInDay?.toString() ?? '', from: from?.toISOString() ?? '', to: to?.toISOString() ?? ''})

  return apiFetch(`/stats?${params}`, {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}

export async function getProductivityApi(rangeInDay?: number, from?: Date, to?: Date ): Promise<Productivity[]> {
  const params = new URLSearchParams({ rangeInDays: rangeInDay?.toString() ?? '', from: from?.toString() ?? '', to: to?.toString() ?? ''})

  return apiFetch(`/stats/productivity?${params}`, {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}
