import { defineStore} from 'pinia'
import { dashboardApi, getProductivityApi, getStatsApi } from '@/api/stats.api.ts'
import { computed, ref } from 'vue'
import type { Dashboard } from '@/types/dashboard.ts'
import type { Stats } from '@/types/stats.ts'
import type { Productivity } from '@/types/productivity.ts'
import { useTaskStore } from '@/stores/taskStore.ts'

export const useStatsStore = defineStore('stats', () => {
  const loading = ref<boolean>(false)
  const error = ref<string | null>(null)
  const dashboardData = ref<Dashboard | null>(null);
  const statsData = ref<Stats | null>(null);
  const productivityData = ref<Productivity[] | null>(null);

  const taskStore = useTaskStore();

  async function dashboard(taskLimit: string) {
    loading.value = true
    error.value = null

    try{
      const data = await dashboardApi(taskLimit)

      dashboardData.value = {
        ...data,
        lastCompletedTasks: await Promise.all(
          data.lastCompletedTasks.map(async (t) => {
            const task = await taskStore.getTaskById(t.taskId)

            return{
              ...t,
              createdAt: new Date(t.createdAt),
              title: task?.title ?? '',
            }
          }),
        )
      }

    }catch(e){
      error.value = e ? e.message : 'Unable to fetch dashboard'
    } finally {
      loading.value = false
    }
  }

  async function stats(rangeInDay?: number, from?: Date, to?: Date){
    loading.value = true
    error.value = null

    try{
      statsData.value = await getStatsApi(rangeInDay, from, to)
    }catch(e){
      error.value = e ? e.message : 'Unable to fetch stats'
    } finally {
      loading.value = false
    }
  }

  async function productivity(rangeInDay?: number, from?: Date, to?: Date ){
    loading.value = true
    error.value = null

    try{
      const data = await getProductivityApi(rangeInDay, from, to)

      productivityData.value = data.map(t => ({
        ...t,
        date: new Date(t.date)
      }))

    }catch(e){
      error.value = e ? e.message : 'Unable to fetch stats'
    } finally {
      loading.value = false
    }
  }

  return {loading, error, dashboard, dashboardData, stats, productivity, statsData, productivityData}
})
