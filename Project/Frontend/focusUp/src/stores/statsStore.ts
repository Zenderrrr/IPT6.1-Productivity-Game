import { defineStore} from 'pinia'
import { dashboardApi } from '@/api/stats.api.ts'
import { computed, ref } from 'vue'
import type { Dashboard } from '@/types/dashboard.ts'

export const useStatsStore = defineStore('stats', () => {
  const loading = ref<boolean>(false)
  const error = ref<string | null>(null)
  const dashboardData = ref<Dashboard | null>(null);

  async function dashboard() {
    loading.value = true
    error.value = null

    try{
      const data = await dashboardApi(10)
      dashboardData.value = data
    }catch(e){
      error.value = e ? e.message : 'Unable to fetch stats'
    }
  }

  return {loading, error, dashboard, dashboardData}
})
