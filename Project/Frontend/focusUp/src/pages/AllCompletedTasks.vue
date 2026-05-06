<script lang="ts" setup>
import { onMounted, ref } from 'vue'
import { useTaskStore } from '@/stores/taskStore.ts'
import { GetTimeFromNow } from '@/utils/date.ts'
import Tag from '@/components/ui/Tag.vue'
import { useStatsStore } from '@/stores/statsStore.ts'
import { useCategoryStore } from '@/stores/categoryStore.ts'
import NavAuth from '@/components/layout/NavAuth.vue'

const statsStore = useStatsStore()
const tasksStore = useTaskStore()
const categoryStore = useCategoryStore()

const lastCompletedTasks = ref<any | null>(null)

onMounted(async () => {
  await statsStore.dashboard('200')
  lastCompletedTasks.value = await getLastCompletedTasks()
})

async function getLastCompletedTasks() {
  if (statsStore.dashboardData == null) {
    return []
  }
  return await Promise.all(
    statsStore.dashboardData.lastCompletedTasks.map(async (t) => {
      const task = await tasksStore.getTaskById(t.taskId)

      return {
        ...t,
        task,
        category: task?.categoryId ? await categoryStore.getCategoryById(task.categoryId) : null,
      }
    }),
  )
}
</script>

<template>
  <NavAuth></NavAuth>

  <main>
    <table class="w-full mt-3 border-collapse">
      <tr>
        <th>Datum</th>
        <th>Task</th>
        <th>Kategorie</th>
        <th>Dauer</th>
        <th>XP</th>
      </tr>
      <tr v-for="lastTask in lastCompletedTasks ?? []" :key="lastTask.id">
        <td class="text-[var(--text-color-light)] text-sm">
          {{ GetTimeFromNow(lastTask.createdAt) }}
        </td>
        <td class="font-medium text-sm">{{ lastTask.title }}</td>
        <td>
          <Tag
            v-if="lastTask.category !== null && lastTask.category !== undefined"
            class="inline"
            :name="lastTask.category.name"
            color-hex="#ebf8f7"
            text-color-hex="#14B8A6"
          ></Tag>

          <Tag
            v-if="lastTask.category === null || lastTask.category === undefined"
            class="inline"
            name="Keine"
            color-hex="#f9fafb"
            text-color-hex="#99a1af"
          ></Tag>
        </td>
        <td class="text-[var(--text-color-light)] text-sm">{{ lastTask.task.durationMin }} min</td>
        <td class="font-semibold text-[var(--primary-color)] text-sm">
          + {{ lastTask.xpAwarded }} XP
        </td>
      </tr>
    </table>
  </main>
</template>

<style scoped>
table {
  width: 100%;
  border-collapse: collapse;
}

th {
  text-transform: uppercase;
  color: var(--text-color-light);
  font-size: var(--text-sm);
  font-weight: normal;
  border-top: 1px solid var(--color-gray-200);
  background-color: var(--text-color-white);
}

th:last-child,
td:last-child {
  text-align: right;
  padding-right: 20px;
}

th:first-child,
td:first-child {
  text-align: left;
  padding-left: 20px;
}

td,
th {
  border-bottom: 1px solid var(--color-gray-200);
  text-align: left;
  padding: 8px;
}
</style>
