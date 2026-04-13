<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import StatsOverviewCard from '@/components/ui/StatsOverviewCard.vue'
import StatsCompletedTask from '@/components/ui/StatsCompletedTask.vue'
import CurrentWeekStats from '@/components/ui/CurrentWeekStats.vue'

import VChart from 'vue-echarts'
import { computed, onMounted, ref } from 'vue'
import { use } from 'echarts/core'
import { CanvasRenderer } from 'echarts/renderers'
import { LineChart } from 'echarts/charts'
import { GridComponent, TooltipComponent, LegendComponent } from 'echarts/components'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import { useStatsStore } from '@/stores/statsStore.ts'
import type { Dashboard } from '@/types/dashboard.ts'

use([CanvasRenderer, LineChart, GridComponent, TooltipComponent, LegendComponent])

const labels = ['Mo', 'Di', 'Mi', 'Do', 'Fr', 'Sa', 'So']
const values = [100, 80, 20, 150, 240, 20, 50]

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
    data: labels,
  },
  yAxis: {
    type: 'value',
  },
  series: [
    {
      name: 'XP',
      type: 'line',
      data: values,
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

// dashboard get data
const statsStore = useStatsStore()
const error = ref<string | null>(null)

const dashboardInfo = ref<Dashboard | null>(null)

const totalXp = computed(() => dashboardInfo.value?.totalXp)

const currLvl = computed(() => dashboardInfo.value?.level)
const xpCurr = computed(() => dashboardInfo.value?.xpCurrent)
const xpNext = computed(() => dashboardInfo.value?.xpNext)
const progressToNextLevel = computed(() =>
  Math.floor((dashboardInfo.value?.progressToNextLevel ?? 0) * 100),
)

const streakCount = computed(() => dashboardInfo.value?.streakCount)
const tasksDone = computed(() => dashboardInfo.value?.tasksDone)
const tasksOpen = computed(() => dashboardInfo.value?.tasksOpen)

const lastCompletedTasks = computed(() => dashboardInfo.value?.lastCompletedTasks)

onMounted(async () => {
  try {
    await statsStore.dashboard('20')

    dashboardInfo.value = statsStore.dashboardData

    console.log(dashboardInfo.value)
  } catch (e) {
    error.value = e ? e.message : 'Failed to fetch dashboard data'
  }
})

// today
const date = computed(() => { return new Date() })
</script>

<template>
  <NavAuth nameInitials="SS"></NavAuth>
  <main>
    <!-- Greeting Section-->
    <GreetingsSection
      title="Willkommen zurück,"
      user-name="Sanjivan"
      subtitle="Du bist auf einem guten Weg, bleib dran!"
    ></GreetingsSection>

    <!-- Card overview section-->
    <section>
      <div class="grid grid-cols-4 gap-4 auto-rows-[150px]">
        <StatsOverviewCard svg="fa-solid fa-star" stats-name="Gesamt XP" :statsValue="totalXp">
          <span><em class="text-[var(--accent-color)]">+340 XP</em> diese Woche</span>
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
              :class="i == date.getDay() ? 'streak-on' : 'streak-off' "
            ></div>
          </div>
        </StatsOverviewCard>
        <StatsOverviewCard svg="fa-solid fa-check" stats-name="Erledigte Tasks" :stats-value="tasksDone">
          <span><em class="text-[var(--accent-color)]">+8</em> heute - {{ tasksOpen }} offen</span>
        </StatsOverviewCard>
      </div>
    </section>

    <!-- Last completed tasks & quick actions-->
    <section class="grid grid-cols-6 gap-4">
      <!-- Title -->
      <div class="col-span-4 bg-[var(--surface-color)] gen-padding rounded-2xl shadow-lg">
        <div class="flex items-center justify-between">
          <h2 class="font-semibold text-lg">Letzte erledigte Tasks</h2>
          <div
            class="flex items-center justify-center gap-1 text-[var(--primary-color)] text-sm font-semibold"
          >
            <span>Alle ansehen</span>
            <div>
              <i class="fa-solid fa-arrow-right"></i>
            </div>
          </div>
        </div>

        <!-- Tasks -->
        <div class="grid grid-cols-1 grid-rows-5 gap-3 mt-4">
          <div v-for="(task, i) in lastCompletedTasks" :key="i">
            <StatsCompletedTask
              :title="task.action"
              :date="task.createdAt"
              :xp="task.xpAwarded"
            ></StatsCompletedTask>
          </div>
        </div>
      </div>

      <div class="col-span-2 bg-[var(--surface-color)] rounded-2xl gen-padding shadow-lg">
        <h2 class="font-bold">Schnelle Aktionen</h2>

        <!-- Quick Actions-->
        <div class="mb-4">
          <div
            class="flex items-center justify-start mt-4 gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] rounded-xl px-4 py-4 cursor-pointer"
          >
            <div
              class="w-[30px] h-[30px] flex items-center justify-center text-[var(--primary-color-white)] bg-white/20 backdrop-blur-2lg rounded-lg"
            >
              <i class="fa-solid fa-plus text-sm"></i>
            </div>
            <span class="font-semibold">Task erstellen</span>
          </div>

          <div
            class="flex items-center justify-start mt-4 gap-2 bg-[var(--background-color)] border border-gray-200 rounded-xl px-4 py-4 cursor-pointer"
          >
            <div
              class="w-[30px] h-[30px] flex items-center justify-center rounded-lg bg-[var(--text-color-white)]"
            >
              <i class="fa-solid fa-list-check text-sm"></i>
            </div>
            <span class="font-semibold">Tasks anzeigen</span>
          </div>
        </div>

        <!-- Stats Current Week-->
        <span class="uppercase text-[var(--text-color-light)] text-sm font-semibold"
          >Diese Woche</span
        >
        <div class="grid grid-cols-2 grid-rows-2 gap-3 mt-4">
          <CurrentWeekStats :stats-value="23" name="Tasks"></CurrentWeekStats>
          <CurrentWeekStats :stats-value="8.4" name="Fokus" digit="h"></CurrentWeekStats>
          <CurrentWeekStats
            :stats-value="340"
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
    <section class="bg-[var(--surface-color)] rounded-2xl gen-padding shadow-lg">
      <div class="flex items-center justify-between">
        <div>
          <h2 class="font-bold text-lg tracking-wide">Produktivität über Zeit</h2>
          <span class="font-semibold text-sm text-[var(--text-color-light)]"
            >Tasks pro Tag, letzte 14 Tage</span
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
            <span
              class="cursor-pointer px-2.5 py-1 rounded-lg"
              @click="setActive(0)"
              :class="{ chartActive: isActive(0) }"
              >14T</span
            >
            <span
              class="cursor-pointer px-2.5 py-1 rounded-lg"
              @click="setActive(1)"
              :class="{ chartActive: isActive(1) }"
              >1M</span
            >
            <span
              class="cursor-pointer px-2.5 py-1 rounded-lg"
              @click="setActive(2)"
              :class="{ chartActive: isActive(2) }"
              >3M</span
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

.streak-on{
  background-color: var(--primary-color);
}

.streak-off{
  background-color: var(--background-color);
}
</style>
