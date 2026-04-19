<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import StatsOverviewCard from '@/components/ui/StatsOverviewCard.vue'
import StatsCompletedTask from '@/components/ui/StatsCompletedTask.vue'
import CurrentWeekStats from '@/components/ui/CurrentWeekStats.vue'

import VChart from 'vue-echarts'
import { computed, onMounted, onUnmounted, ref, watchEffect } from 'vue'
import { use } from 'echarts/core'
import { CanvasRenderer } from 'echarts/renderers'
import { LineChart } from 'echarts/charts'
import { GridComponent, TooltipComponent, LegendComponent } from 'echarts/components'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import { useStatsStore } from '@/stores/statsStore.ts'
import type { Dashboard } from '@/types/dashboard.ts'
import type { Stats } from '@/types/stats.ts'
import type { Productivity } from '@/types/productivity.ts'
import { formatTime } from '@/utils/date.ts'
import type { User } from '@/types/user.ts'
import { useAuthStore } from '@/stores/authStore.ts'
import CreateTask from '@/components/ui/CreateTask.vue'
import type { CreateTaskType } from '@/types/createTaskType.ts'
import { useTaskStore } from '@/stores/taskStore.ts'

// today
const date = computed(() => {
  return new Date()
})
const today = date.value.getDay() === 0 ? 7 : date.value.getDay()

const weekLength = computed(() => {
  return date.value.getDay() === 0 ? 7 : date.value.getDay()
})

// username
const authStore = useAuthStore()
const authInfo = ref<User | null>(null)

// dashboard get data
const statsStore = useStatsStore()
const error = ref<string | null>(null)

const dashboardInfo = ref<Dashboard | null>(null)
const statsInfoWeek = ref<Stats | null>(null)
const prodInfoWeek = ref<Productivity[] | null>(null)
const prodInfoInd = ref<Productivity[] | null>(null)

use([CanvasRenderer, LineChart, GridComponent, TooltipComponent, LegendComponent])

const chartDayLength = ref<number>(14)

watchEffect(async () => {
  if (chartDayLength.value) {
    prodInfoInd.value = await getProductivity(chartDayLength.value)
  }
})

const labels = computed(() => {
  return prodInfoInd.value?.map(
    (t) =>
      `${formatTime(t.date.getDate())}. ${t.date.toLocaleDateString('de-CH', {
        month: 'short',
      })}`,
  )
})
const values = computed(() => {
  return prodInfoInd.value?.map((t) => t.xpGained)
})

const option = computed(() => ({
  tooltip: {
    trigger: 'axis',
  },
  grid: {
    left: 20,
    right: 20,
    bottom: 20,
    top: 30,
    containLabel: true,
  },
  xAxis: {
    type: 'category',
    data: labels.value,
  },
  yAxis: {
    type: 'value',
  },
  series: [
    {
      name: 'XP',
      type: 'line',
      data: values.value,
      smooth: true,
      lineStyle: {
        color: 'rgb(20, 184, 166)',
      },
      itemStyle: {
        color: 'rgb(20, 184, 166)',
      },
    },
  ],
}))

// chart logic
const active = ref<number>(0)
function setActive(value: number) {
  active.value = value
}

function isActive(i: number) {
  return active.value === i
}

// dashboard data
const totalXp = computed(() => dashboardInfo.value?.totalXp)

const currLvl = computed(() => dashboardInfo.value?.level)
const xpCurr = computed(() => dashboardInfo.value?.xpCurrent)
const xpNext = computed(() => dashboardInfo.value?.xpNext)
const progressToNextLevel = computed(() =>
  Math.floor((dashboardInfo.value?.progressToNextLevel ?? 0) * 100)
)

const streakCount = computed(() => dashboardInfo.value?.streakCount)
const tasksDone = computed(() => dashboardInfo.value?.tasksDone)
const tasksOpen = computed(() => dashboardInfo.value?.tasksOpen)

const lastCompletedTasks = computed(() => dashboardInfo.value?.lastCompletedTasks ?? [])

// stats data
const xpWeek = computed(() => statsInfoWeek.value?.totalXp)

const todayStatsDay = ref<Stats | null>(null)
const taskDoneToday = computed(() => todayStatsDay.value?.tasksDone)

// productivity data
const tasksDoneWeek = computed(() =>
  prodInfoWeek.value?.reduce((n, { completedTasks }) => n + completedTasks, 0),
)
const focusTimeWeek = computed(
  () =>
    Math.round(
      (prodInfoWeek.value?.reduce((n, { timeSpent }) => n + timeSpent, 0) ?? 0) / 60 * 10,
    ) / 10,
)

let isMounted = true
onMounted(async () => {
  try {
    await statsStore.dashboard('20')
    dashboardInfo.value = statsStore.dashboardData

    await statsStore.stats(weekLength.value)
    statsInfoWeek.value = statsStore.statsData

    await statsStore.stats(1)
    todayStatsDay.value = statsStore.statsData

    await statsStore.productivity(weekLength.value)
    prodInfoWeek.value = statsStore.productivityData

    await statsStore.productivity(chartDayLength.value)
    prodInfoInd.value = statsStore.productivityData

    await authStore.me()
    authInfo.value = authStore.user

    if (!isMounted) {
      return
    }

  } catch (e) {
    error.value = e ? e.message : 'Failed to fetch dashboard data'
  }
})

onUnmounted(() => {
  isMounted = false
})

async function getProductivity(lengthDay: number) {
  await statsStore.productivity(lengthDay)
  return statsStore.productivityData
}

// show pop-up task
const taskStore = useTaskStore()
const showPopUpTask = ref<boolean>(false)

async function submitTask(task: CreateTaskType) {
  taskStore.error = null
  try{
    await taskStore.createTask(task)
  }catch(e){
    console.error(e)
  } finally {
    if(!taskStore.error)
      showPopUpTask.value = false
  }
}
</script>

<template>
  <CreateTask :is-shown="showPopUpTask" @cancel="showPopUpTask = false" @submit="submitTask"></CreateTask>

  <NavAuth></NavAuth>
  <main>
    <!-- Greeting Section-->
    <GreetingsSection
      title="Willkommen zurück,"
      :user-name="authInfo?.username"
      subtitle="Du bist auf einem guten Weg, bleib dran!"
    ></GreetingsSection>

    <!-- Card overview section-->
    <section>
      <div class="grid grid-cols-4 gap-4 auto-rows-[150px]">
        <StatsOverviewCard svg="fa-solid fa-star" stats-name="Gesamt XP" :statsValue="totalXp">
          <span
            ><em class="text-[var(--accent-color)]">+{{ xpWeek }} XP</em> diese Woche</span
          >
        </StatsOverviewCard>
        <StatsOverviewCard svg="fa-solid fa-chart-line" stats-name="Level" :stats-value="currLvl">
          <div>
            <div
              class="w-full bg-[var(--background-color)] h-2 rounded-full overflow-hidden mb-2.5"
            >
              <div
                :style="{ width: `${progressToNextLevel}%` }"
                class="h-full bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] rounded-full"
              ></div>
            </div>
            <span>{{ xpCurr }} / {{ xpNext }} XP zum nächsten Level</span>
          </div>
        </StatsOverviewCard>
        <StatsOverviewCard svg="fa-solid fa-fire" stats-name="Streak" :stats-value="streakCount">
          <span>Tage in Folge</span>
          <div class="grid grid-cols-7 gap-2 mt-2">
            <div
              v-for="i in 7"
              :key="i"
              class="h-1.5 bg-[var(--background-color)] rounded-full"
              :class="((i <= today && i > today - (streakCount ?? 0))) ? 'streak-on' : 'streak-off' "
            ></div>
          </div>
        </StatsOverviewCard>
        <StatsOverviewCard
          svg="fa-solid fa-check"
          stats-name="Erledigte Tasks"
          :stats-value="tasksDone"
        >
          <span
            ><em class="text-[var(--accent-color)]">+{{ taskDoneToday }}</em> heute -
            {{ tasksOpen }} offen</span
          >
        </StatsOverviewCard>
      </div>
    </section>

    <!-- Last completed tasks & quick actions-->
    <section class="grid grid-cols-6 gap-4">
      <!-- Title -->
      <div class="box-hover-animation col-span-4 bg-[var(--surface-color)] gen-padding rounded-2xl shadow-lg">
        <div class="flex items-center justify-between">
          <h2 class="font-semibold text-lg">Letzte erledigte Tasks</h2>
          <div
            class="color-change-secondary-animation cursor-pointer flex items-center justify-center gap-1 text-[var(--primary-color)] text-sm font-semibold"
          >
            <span>Alle ansehen</span>
            <div>
              <i class="fa-solid fa-arrow-right"></i>
            </div>
          </div>
        </div>

        <!-- Tasks -->
        <div class="grid grid-cols-1 grid-rows-5 gap-3 mt-4">
          <div v-for="task in lastCompletedTasks.slice(0, 5)" :key="task.id">
            <StatsCompletedTask
              :title="task.title"
              :date="task.createdAt"
              :xp="task.xpAwarded"
            ></StatsCompletedTask>
          </div>
        </div>
      </div>

      <div class="box-hover-animation col-span-2 bg-[var(--surface-color)] rounded-2xl gen-padding shadow-lg">
        <h2 class="font-bold">Schnelle Aktionen</h2>

        <!-- Quick Actions-->
        <div class="mb-4">
          <div
            @click="showPopUpTask = true"
            class="scale-animation-sm flex items-center justify-start mt-4 gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] rounded-xl px-4 py-4 cursor-pointer"
          >
            <div
              class="w-[30px] h-[30px] flex items-center justify-center text-[var(--primary-color-white)] bg-white/20 backdrop-blur-2lg rounded-lg"
            >
              <i class="fa-solid fa-plus text-sm"></i>
            </div>
            <span class="font-semibold">Task erstellen</span>
          </div>

          <div
            class="hover:border-[var(--secondary-color)] hover:text-[var(--secondary-color)] transition duration-200 flex items-center justify-start mt-4 gap-2 bg-[var(--background-color)] border border-gray-200 rounded-xl px-4 py-4 cursor-pointer"
          >
            <div
              class="w-[30px] h-[30px] flex items-center justify-center rounded-lg bg-[var(--text-color-white)]"
            >
              <i class="fa-solid fa-list-check text-sm"></i>
            </div>
            <RouterLink to="/tasks" class="font-semibold">Tasks anzeigen</RouterLink>
          </div>
        </div>

        <!-- Stats Current Week-->
        <span class="uppercase text-[var(--text-color-light)] text-sm font-semibold"
          >Diese Woche</span
        >
        <div class="grid grid-cols-2 grid-rows-2 gap-3 mt-4">
          <CurrentWeekStats :stats-value="tasksDoneWeek" name="Tasks"></CurrentWeekStats>
          <CurrentWeekStats :stats-value="focusTimeWeek" name="Fokus" digit="h"></CurrentWeekStats>
          <CurrentWeekStats
            :stats-value="xpWeek"
            name="XP Gewonnen"
            digit="+"
            color-tailwind="text-[var(--primary-color)]"
          ></CurrentWeekStats>
          <CurrentWeekStats
            :stats-value="79"
            name="Ziel erreicht"
            digit="%"
            color-tailwind="text-[var(--accent-color)]"
          ></CurrentWeekStats>
        </div>
      </div>
    </section>

    <!-- Productivity over time -->
    <section class="box-hover-animation bg-[var(--surface-color)] rounded-2xl gen-padding shadow-lg">
      <div class="flex items-center justify-between">
        <div>
          <h2 class="font-bold text-lg tracking-wide">Produktivität über Zeit</h2>
          <span class="font-semibold text-sm text-[var(--text-color-light)]"
            >XP pro Tag</span
          >
        </div>
        <div
          class="flex items-center justify-end gap-4 text-[var(--text-color-light)] font-semibold text-sm"
        >
          <div class="flex items-center justify-start gap-2">
            <div class="w-[10px] h-[10px] rounded-full bg-[var(--primary-color)]"></div>
            <span>Tasks erledigt</span>
          </div>

          <div class="flex items-center justify-start gap-2">
            <div class="w-[10px] h-[10px] rounded-full bg-[var(--primary-color-light)]"></div>
            <span>Ziel</span>
          </div>

          <div class="flex items-center justify-end gap-1.5">
            <button
              class="hover:dark:bg-gray-100 transition duration-200 cursor-pointer px-2.5 py-1 rounded-lg"
              @click="setActive(0); chartDayLength = 14"
              :class="{ chartActive: isActive(0) }"
              >14T</button
            >
            <button
              class="hover:dark:bg-gray-100 transition duration-200 cursor-pointer px-2.5 py-1 rounded-lg"
              @click="setActive(1); chartDayLength = 30"
              :class="{ chartActive: isActive(1) }"
              >1M</button
            >
            <button
              class="hover:dark:bg-gray-100 transition duration-200 cursor-pointer px-2.5 py-1 rounded-lg"
              @click="setActive(2); chartDayLength = 90"
              :class="{ chartActive: isActive(2) }"
              >3M</button
            >
          </div>
        </div>
      </div>

      <div class="w-full h-[200px]">
        <VChart class="h-full w-full" :option="option" autoresize />
      </div>
    </section>
  </main>
</template>

<style scoped>
.chartActive {
  background-color: var(--primary-color-light);
  color: var(--primary-color);
}

.streak-on {
  background-color: var(--primary-color);
}

.streak-off {
  background-color: var(--background-color);
}
</style>
