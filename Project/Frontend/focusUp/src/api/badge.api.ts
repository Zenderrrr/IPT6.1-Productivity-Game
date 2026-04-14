import type { Badge } from '@/types/badge.ts'
import { apiFetch } from '@/api/api.ts'

export async function getAllBadgesApi(): Promise<Badge[]> {
  return await apiFetch('/badges', {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}

export async function getBadgeByIdApi(id: number): Promise<Badge> {
  return await apiFetch(`/badges/${id}`, {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}

export async function getUnlockedBadgesApi(): Promise<Badge[]> {
  return await apiFetch('/badges/unlocked', {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}

export async function getLockedBadgesApi(): Promise<Badge[]> {
  return await apiFetch('/badges/locked', {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`
    }
  })
}
