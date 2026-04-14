import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Badge } from '@/types/badge.ts'
import {
  getAllBadgesApi,
  getBadgeByIdApi,
  getLockedBadgesApi,
  getUnlockedBadgesApi,
} from '@/api/badge.api.ts'

export const useBadgeStore = defineStore('badge', () => {
  const loading = ref<boolean>(false)
  const error = ref<string | null>(null)
  const badgeData = ref<Badge[] | null>(null)
  const badgeUnlockedData = ref<Badge[] | null>(null)
  const badgeLockedData = ref<Badge[] | null>(null)

  async function allBadge(){
    loading.value = true
    error.value = null

    try{
      const data = await getAllBadgesApi()

      badgeData.value = data.map((t) => ({
        ...t,
        createdAt: new Date(t.createdAt),
      }))
    }
    catch(e){
      error.value = e ? e.message : 'Unable to fetch badges'
    } finally {
      loading.value = false
    }
  }

  async function badgesById(id:number){
    loading.value = true
    error.value = null

    try{
      return await getBadgeByIdApi(id)

    }catch{
      error.value = e ? e.message : 'Unable to fetch badge by id'
    } finally {
      loading.value = false
    }
  }

  async function badgeUnlocked(){
    loading.value = true
    error.value = null

    try{
      badgeUnlockedData.value = await getUnlockedBadgesApi()
    }catch(e){
      error.value = e ? e.message : 'Unable to fetch unlocked badges'
    } finally {
      loading.value = false
    }
  }

  async function badgeLocked(){
    loading.value = true
    error.value = null

    try{
      badgeLockedData.value = await getLockedBadgesApi()
    }catch(e){
      error.value = e ? e.message : 'Unable to fetch locked badges'
    } finally {
      loading.value = false
    }
  }

  return {allBadge,loading,error,badgeData, badgeUnlocked, badgeUnlockedData, badgeLocked, badgeLockedData}
})
