<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import StatsCard from '@/components/ui/StatsCard.vue'
import InsightCard from '@/components/ui/InsightCard.vue'
import Tag from '@/components/ui/Tag.vue'
import type { Stats } from '@/types/stats.ts'
import VChart from 'vue-echarts'
import { computed, onMounted, ref, watch, watchEffect } from 'vue'
import { use } from 'echarts/core'
import { CanvasRenderer } from 'echarts/renderers'
import { LineChart, BarChart, PieChart, HeatmapChart } from 'echarts/charts'
import {
  GridComponent,
  TooltipComponent,
  LegendComponent,
  VisualMapComponent,
  TitleComponent,
} from 'echarts/components'
import { LegacyGridContainLabel } from 'echarts/features'
import { useTaskStore } from '@/stores/taskStore.ts'
import { useStatsStore } from '@/stores/statsStore.ts'
import { formatTime, GetTimeFromNow } from '@/utils/date.ts'
import { useCategoryStore } from '@/stores/categoryStore.ts'
import type { Productivity } from '@/types/productivity.ts'

use([
  CanvasRenderer,
  LineChart,
  GridComponent,
  TooltipComponent,
  LegendComponent,
  LegacyGridContainLabel,
  BarChart,
  PieChart,
  HeatmapChart,
  VisualMapComponent,
  TitleComponent,
])

// data
const statsStore = useStatsStore()
const tasksStore = useTaskStore()
const categoryStore = useCategoryStore()

const statsLast14Days = ref<Stats | null>(null)
const statsLast7Days = ref<Stats | null>(null)

const prodInfoInd = ref<Productivity[] | null>(null)
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

const optionProductivity = computed(() => ({
  xAxis: {
    data: labels.value ?? [],
  },
  grid: {
    left: 20,
    right: 20,
    bottom: 20,
    top: 30,
    containLabel: true,
  },
  yAxis: {},
  series: [
    {
      type: 'bar',
      data: values.value,
      itemStyle: {
        barBorderRadius: 10,
        borderWidth: 1,
        borderType: 'solid',
        color: '#14B8A6',
      },
    },
  ],
}))

const optionDoughnut = computed(() => ({
  title: {
    text: 'A Case of Doughnut Chart',
    left: 'center',
    top: 'center',
  },
  grid: {
    left: 20,
    right: 20,
    bottom: 20,
    top: 30,
    containLabel: true,
  },
  series: [
    {
      type: 'pie',
      data: [
        {
          value: 335,
          name: 'A',
        },
        {
          value: 234,
          name: 'B',
        },
        {
          value: 1548,
          name: 'C',
        },
      ],
      radius: ['40%', '70%'],
    },
  ],
}))

const optionLine = computed(() => ({
  xAxis: {
    data: ['A', 'B', 'C', 'D', 'E'],
  },
  yAxis: {},
  grid: {
    left: 20,
    right: 20,
    bottom: 20,
    top: 30,
    containLabel: false,
  },
  series: [
    {
      data: [10, 22, 28, 43, 49],
      type: 'line',
      stack: 'x',
    },
    {
      data: [5, 4, 3, 5, 10],
      type: 'line',
      stack: 'x',
    },
  ],
}))

const hours = ['12a', '1a', '2a', '3a', '4a', '5a', '6a', '7a', '8a', '9a', '10a', '11a', '12p']
const days = ['Sa', 'Fr', 'Do', 'Mi', 'Di', 'Mo', 'So']
const data = [
  [0, 0, 5],
  [0, 1, 1],
  [0, 2, 0],
  [0, 3, 0],
  [0, 4, 0],
  [0, 5, 0],
  [0, 6, 0],
  [0, 7, 0],
  [0, 8, 0],
  [0, 9, 0],
  [0, 10, 0],
  [0, 11, 2],
  [0, 12, 4],
  [0, 13, 1],
  [0, 14, 1],
  [0, 15, 3],
  [0, 16, 4],
  [0, 17, 6],
  [0, 18, 4],
  [0, 19, 4],
  [0, 20, 3],
  [0, 21, 3],
  [0, 22, 2],
  [0, 23, 5],
  [1, 0, 7],
  [1, 1, 0],
  [1, 2, 0],
  [1, 3, 0],
  [1, 4, 0],
  [1, 5, 0],
  [1, 6, 0],
  [1, 7, 0],
  [1, 8, 0],
  [1, 9, 0],
  [1, 10, 5],
  [1, 11, 2],
  [1, 12, 2],
  [1, 13, 6],
  [1, 14, 9],
  [1, 15, 11],
  [1, 16, 6],
  [1, 17, 7],
  [1, 18, 8],
  [1, 19, 12],
  [1, 20, 5],
  [1, 21, 5],
  [1, 22, 7],
  [1, 23, 2],
  [2, 0, 1],
  [2, 1, 1],
  [2, 2, 0],
  [2, 3, 0],
  [2, 4, 0],
  [2, 5, 0],
  [2, 6, 0],
  [2, 7, 0],
  [2, 8, 0],
  [2, 9, 0],
  [2, 10, 3],
  [2, 11, 2],
  [2, 12, 1],
  [2, 13, 9],
  [2, 14, 8],
  [2, 15, 10],
  [2, 16, 6],
  [2, 17, 5],
  [2, 18, 5],
  [2, 19, 5],
  [2, 20, 7],
  [2, 21, 4],
  [2, 22, 2],
  [2, 23, 4],
  [3, 0, 7],
  [3, 1, 3],
  [3, 2, 0],
  [3, 3, 0],
  [3, 4, 0],
  [3, 5, 0],
  [3, 6, 0],
  [3, 7, 0],
  [3, 8, 1],
  [3, 9, 0],
  [3, 10, 5],
  [3, 11, 4],
  [3, 12, 7],
  [3, 13, 14],
  [3, 14, 13],
  [3, 15, 12],
  [3, 16, 9],
  [3, 17, 5],
  [3, 18, 5],
  [3, 19, 10],
  [3, 20, 6],
  [3, 21, 4],
  [3, 22, 4],
  [3, 23, 1],
  [4, 0, 1],
  [4, 1, 3],
  [4, 2, 0],
  [4, 3, 0],
  [4, 4, 0],
  [4, 5, 1],
  [4, 6, 0],
  [4, 7, 0],
  [4, 8, 0],
  [4, 9, 2],
  [4, 10, 4],
  [4, 11, 4],
  [4, 12, 2],
  [4, 13, 4],
  [4, 14, 4],
  [4, 15, 14],
  [4, 16, 12],
  [4, 17, 1],
  [4, 18, 8],
  [4, 19, 5],
  [4, 20, 3],
  [4, 21, 7],
  [4, 22, 3],
  [4, 23, 0],
  [5, 0, 2],
  [5, 1, 1],
  [5, 2, 0],
  [5, 3, 3],
  [5, 4, 0],
  [5, 5, 0],
  [5, 6, 0],
  [5, 7, 0],
  [5, 8, 2],
  [5, 9, 0],
  [5, 10, 4],
  [5, 11, 1],
  [5, 12, 5],
  [5, 13, 10],
  [5, 14, 5],
  [5, 15, 7],
  [5, 16, 11],
  [5, 17, 6],
  [5, 18, 0],
  [5, 19, 5],
  [5, 20, 3],
  [5, 21, 4],
  [5, 22, 2],
  [5, 23, 0],
  [6, 0, 1],
  [6, 1, 0],
  [6, 2, 0],
  [6, 3, 0],
  [6, 4, 0],
  [6, 5, 0],
  [6, 6, 0],
  [6, 7, 0],
  [6, 8, 0],
  [6, 9, 0],
  [6, 10, 1],
  [6, 11, 0],
  [6, 12, 2],
  [6, 13, 1],
  [6, 14, 3],
  [6, 15, 4],
  [6, 16, 0],
  [6, 17, 0],
  [6, 18, 0],
  [6, 19, 0],
  [6, 20, 1],
  [6, 21, 2],
  [6, 22, 2],
  [6, 23, 6],
].map(function (item) {
  return [item[1], item[0], item[2] || '-']
})

const optionHeatmap = computed(() => ({
  tooltip: {
    position: 'top',
  },
  grid: {
    left: 20,
    right: 20,
    bottom: 20,
    top: 30,
    containLabel: false,
  },
  xAxis: {
    type: 'category',
    data: hours,
    splitArea: {
      show: true,
    },
    axisLabel: {
      show: false,
    },
  },
  yAxis: {
    type: 'category',
    data: days,
    splitArea: {
      show: true,
    },
  },
  visualMap: {
    min: 0,
    max: 10,
    show: false,
    calculable: true,
    orient: 'horizontal',
    left: 'center',
    bottom: '15%',
    inRange: {
      color: [
        '#f3f4f6', // wenig
        '#ebf8f7',
        '#96f7e4',
        '#14B8A6',
        '#0f766e', // viel
      ],
    },
  },
  series: [
    {
      name: 'Aktivitätsmap',
      type: 'heatmap',
      data: data,
      label: {
        show: false,
      },
      emphasis: {
        itemStyle: {
          shadowBlur: 10,
          shadowColor: 'rgba(0, 0, 0, 0.5)',
        },
      },
    },
  ],
}))

// stats overview
const taskDoneTrend = computed(() => {
  const last14 = statsLast14Days.value?.tasksDone ?? 0
  const last7 = statsLast7Days.value?.tasksDone ?? 0

  const previous7 = last14 - last7

  if (previous7 === 0) return 0

  return ((last7 - previous7) / previous7) * 100
})

const totalXpTrend = computed(() => {
  const last14 = statsLast14Days.value?.totalXp ?? 0
  const last7 = statsLast7Days.value?.totalXp ?? 0

  const previous7 = last14 - last7

  if (previous7 === 0) return 0

  return ((last7 - previous7) / previous7) * 100
})

const taskPerDayTrend = computed(() => {
  const last14 = (statsLast14Days.value?.tasksDone ?? 0) / 7
  const last7 = (statsLast7Days.value?.tasksDone ?? 0) / 7

  const previous7 = last14 - last7

  if (previous7 === 0) return 0
  return previous7
})

// last completed tasks
const lastCompletedTasks = ref<any[]>([])

watchEffect(async () => {
  if (!statsStore.dashboardData) return
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

onMounted(async () => {
  try {
    await statsStore.dashboard('20')
    await getLastCompletedTasks()

    await statsStore.stats(14)
    statsLast14Days.value = statsStore.statsData

    await statsStore.stats(7)
    statsLast7Days.value = statsStore.statsData

    await statsStore.productivity(7)
    prodInfoInd.value = statsStore.productivityData
  } catch {}
})
</script>

<template>
  <NavAuth></NavAuth>
  <main>
    <GreetingsSection
      title="Deine Produktivität"
      subtitle="Sehe deine Statistiken ein und untersuche deine Stärken"
    ></GreetingsSection>

    <!-- Choose time scaling-->
    <section class="flex items-center justify-start h-full gap-3">
      <div
        class="h-full flex items-center justify-center px-3 py-2 gap-4 bg-[var(--surface-color)] border border-gray-200 rounded-lg text-[var(--text-color-light)]"
      >
        <button class="active px-2 py-1 rounded-lg cursor-pointer">Heute</button>
        <button class="px-2 py-1 rounded-lg cursor-pointer">Woche</button>
        <button class="px-2 py-1 rounded-lg cursor-pointer">Monat</button>
        <button class="px-2 py-1 rounded-lg cursor-pointer">Benutzerdefiniert</button>
        <div class="h-[25px] w-0.5 rounded-full bg-gray-100"></div>
        <button class="px-2 py-1 rounded-lg cursor-pointer">Alle Kategorien</button>
      </div>
      <div
        class="cursor-pointer text-md flex justify-center items-center gap-2 text-[var(--text-color-light)] border border-gray-300 rounded-lg px-2 py-3"
      >
        <div class="flex justify-center items-center">
          <i class="fa-solid fa-arrow-up-from-bracket"></i>
        </div>
        <span>Export</span>
      </div>
    </section>

    <section>
      <span class="subtitle">Übersicht</span>

      <div class="flex items-center justify-center gap-4 w-full mt-3">
        <StatsCard
          title="Erledigte Task"
          svg="fa-solid fa-check"
          :card-value="statsStore.dashboardData?.tasksDone"
          primary-color="#14B8A6"
          secondary-color="#ebf8f7"
        >
          <div
            class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-up"></i>
            </div>
            <span>{{ taskDoneTrend }}% vs. letzer Woche</span>
          </div>
        </StatsCard>

        <StatsCard
          title="Gesamt XP"
          svg="fa-solid fa-bolt"
          :card-value="statsStore.dashboardData?.totalXp"
          primary-color="#0EA5E9"
          secondary-color="rgba(189, 234, 255, 0.63)"
        >
          <div
            class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-up"></i>
            </div>
            <span>{{ totalXpTrend }}% vs. letzer Woche</span>
          </div>
        </StatsCard>

        <StatsCard
          title="⌀ Tasks pro Tag (letzte 7 Tage)"
          svg="fa-solid fa-chart-line"
          :card-value="Math.round(((statsStore.statsData?.tasksDone ?? 0) / 7) * 10) / 10"
          primary-color="oklch(79.2% 0.209 151.711)"
          secondary-color="oklch(96.2% 0.044 156.743)"
        >
          <div
            class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-up"></i>
            </div>
            <span>{{ taskPerDayTrend }} vs. letzer Woche</span>
          </div>
        </StatsCard>

        <StatsCard
          title="Aktuelle Streak"
          svg="fa-solid fa-meteor"
          :card-value="statsStore.dashboardData?.streakCount"
          primary-color="oklch(83.7% 0.128 66.29)"
          secondary-color="oklch(95.4% 0.038 75.164)"
        >
          <div
            v-if="
              (statsStore.dashboardData?.streakCount ?? 0) >=
              (statsStore.dashboardData?.bestStreak ?? 0)
            "
            class="flex justify-center items-center gap-1 text-xs mt-1 text-orange-400 bg-orange-50 rounded-full px-2 py-1 border border-orange-400"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-up"></i>
            </div>
            <span>Persönlicher Rekord!</span>
          </div>
        </StatsCard>
      </div>
    </section>

    <section>
      <span class="subtitle">Verlauf</span>

      <div class="grid grid-cols-6 gap-4 mt-3 max-h-[500px]">
        <div class="col-span-4 base-element bg-[var(--surface-color)] h-[500px] flex flex-col">
          <div class="flex items-start justify-between w-full">
            <div class="flex flex-col">
              <span class="text-lg font-semibold">Produktivität über Zeit</span>
              <span class="text-sm text-[var(--text-color-light)]">Erledigte Tasks pro Tag</span>
            </div>
            <div
              class="flex items-center justify-center gap-2 text-sm text-[var(--primary-color)] bg-[var(--primary-color-light)] border border-[var(--primary-color)] rounded-full px-3 py-1.5"
            >
              <div class="flex justify-center items-center">
                <i class="fa-solid fa-arrow-up"></i>
              </div>
              <span>Steigend</span>
            </div>
          </div>

          <div class="flex gap-5 mt-3">
            <div
              class="flex items-center justify-center gap-2 text-sm text-[var(--text-color-light)]"
            >
              <div class="w-[12px] h-[12px] bg-[var(--primary-color)] rounded-sm"></div>
              <span>Tasks</span>
            </div>

            <div
              class="flex items-center justify-center gap-2 text-sm text-[var(--text-color-light)]"
            >
              <div class="w-[12px] h-[12px] bg-gray-200 rounded-sm"></div>
              <span class="text-nowrap">Ziel (7/Tag)</span>
            </div>
          </div>

          <div class="flex-1 min-h-0 w-full">
            <VChart class="w-full h-full" :option="optionProductivity" autoresize></VChart>
          </div>
        </div>
        <div class="col-span-2 base-element bg-[var(--surface-color)] flex flex-col">
          <div class="flex items-start justify-between w-full">
            <div class="flex flex-col">
              <span class="text-lg font-semibold">Kategorien</span>
              <span class="text-sm text-[var(--text-color-light)]"
                >Task Verteilung diese Woche</span
              >
            </div>
          </div>

          <div class="flex-1 min-h-0 w-full">
            <VChart class="w-full h-full" :option="optionDoughnut" autoresize></VChart>
          </div>
        </div>
      </div>
    </section>

    <section>
      <div class="base-element h-[350px] flex flex-col min-h-0">
        <div class="flex items-start justify-between w-full">
          <div class="flex flex-col">
            <span class="text-lg font-semibold">XP Verlauf</span>
            <span class="text-sm text-[var(--text-color-light)]"
              >Kumuliertes Wachstum und tägliche XP</span
            >
          </div>
          <div
            class="flex items-center justify-center gap-2 text-sm text-[var(--primary-color)] rounded-full px-3 py-1.5"
          >
            <div class="flex gap-5 mt-3">
              <div
                class="flex items-center justify-center gap-2 text-sm text-[var(--text-color-light)]"
              >
                <div class="w-[12px] h-[12px] bg-[var(--secondary-color)] rounded-sm"></div>
                <span>Kumuliert</span>
              </div>

              <div
                class="flex items-center justify-center gap-2 text-sm text-[var(--text-color-light)]"
              >
                <div class="w-[12px] h-[12px] bg-[var(--primary-color)] rounded-sm"></div>
                <span>Täglich</span>
              </div>
            </div>
          </div>
        </div>

        <div class="flex-1 min-h-0 w-full">
          <VChart class="w-full h-full" :option="optionLine" autoresize></VChart>
        </div>
      </div>
    </section>

    <section>
      <div class="base-element min-h-0 flex flex-col">
        <div class="flex items-start justify-between w-full">
          <div class="flex flex-col">
            <span class="text-lg font-semibold">Aktivitäts Heatmap</span>
            <span class="text-sm text-[var(--text-color-light)]">Letzte 91 Tage</span>
          </div>
          <div
            class="flex items-center justify-center gap-2 text-sm text-[var(--primary-color)] rounded-full px-3 py-1.5"
          >
            <div class="flex gap-5 mt-3">
              <div
                class="flex items-center justify-center gap-2 text-sm text-[var(--text-color-light)]"
              >
                <span>Weniger</span>
                <div class="flex gap-2 items-center justify-center">
                  <div class="w-[12px] h-[12px] bg-gray-100 rounded-sm"></div>
                  <div class="w-[12px] h-[12px] bg-[var(--primary-color-light)] rounded-sm"></div>
                  <div class="w-[12px] h-[12px] bg-teal-200 rounded-sm"></div>
                  <div class="w-[12px] h-[12px] bg-[var(--primary-color)] rounded-sm"></div>
                  <div class="w-[12px] h-[12px] bg-teal-700 rounded-sm"></div>
                </div>
                <span>Mehr</span>
              </div>
            </div>
          </div>
        </div>

        <div class="w-full" :style="{ aspectRatio: `${hours.length} / ${days.length}` }">
          <VChart class="w-full h-full" :option="optionHeatmap" autoresize></VChart>
        </div>
      </div>
    </section>

    <section>
      <span class="subtitle">Insights</span>

      <div class="mt-3 flex items-center justify-center gap-4 w-full">
        <InsightCard
          title="Beste Tage"
          card-value="Mo, Di, Do"
          description="Am Montag erledigst du die meisten Tags in der Woche. Das ist mit Abstand dein stärkster Wochentag"
          svg="fa-regular fa-calendar"
        >
          <div class="flex items-center justify-start">
            <div
              class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
            >
              <div class="flex items-center justify-center">
                <i class="fa-solid fa-arrow-up"></i>
              </div>
              <span>+12% vs. letzer Woche</span>
            </div>
          </div>
        </InsightCard>
        <InsightCard
          title="Beste Tage"
          card-value="Mo, Di, Do"
          description="Am Montag erledigst du die meisten Tags in der Woche. Das ist mit Abstand dein stärkster Wochentag"
          svg="fa-regular fa-calendar"
        >
          <div class="flex items-center justify-start">
            <div
              class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
            >
              <div class="flex items-center justify-center">
                <i class="fa-solid fa-arrow-up"></i>
              </div>
              <span>+12% vs. letzer Woche</span>
            </div>
          </div>
        </InsightCard>
        <InsightCard
          title="Beste Tage"
          card-value="Mo, Di, Do"
          description="Am Montag erledigst du die meisten Tags in der Woche. Das ist mit Abstand dein stärkster Wochentag"
          svg="fa-regular fa-calendar"
        >
          <div class="flex items-center justify-start">
            <div
              class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
            >
              <div class="flex items-center justify-center">
                <i class="fa-solid fa-arrow-up"></i>
              </div>
              <span>+12% vs. letzer Woche</span>
            </div>
          </div>
        </InsightCard>
      </div>
    </section>

    <section>
      <span class="subtitle">Task-Log</span>

      <div class="base-element min-h-[350px] mt-3">
        <div class="flex items-center justify-between w-full">
          <div class="flex flex-col items-start justify-center gap-1">
            <span class="font-semibold">Letzte erledigte Tasks</span>
            <span class="text-[var(--text-color-light)] text-sm"
              >{{ statsStore.dashboardData?.lastCompletedTasks.length }} Tasks diese Woche</span
            >
          </div>
          <button
            class="cursor-pointer flex items-center justify-center gap-1 border text-sm border-gray-300 rounded-lg px-2 py-1 text-gray-500"
          >
            <span>Alle anzeigen</span>
            <i class="fa-solid fa-angle-right"></i>
          </button>
        </div>

        <table class="w-full mt-3 border-collapse">
          <tr>
            <th>Datum</th>
            <th>Task</th>
            <th>Kategorie</th>
            <th>Dauer</th>
            <th>XP</th>
          </tr>
          <tr v-for="lastTask in lastCompletedTasks" :key="lastTask.id">
            <td class="text-[var(--text-color-light)] text-sm">
              {{ GetTimeFromNow(lastTask.createdAt) }}
            </td>
            <td class="font-medium text-sm">{{ lastTask.task.title }}</td>
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
            <td class="text-[var(--text-color-light)] text-sm">
              {{ lastTask.task.durationMin }} min
            </td>
            <td class="font-semibold text-[var(--primary-color)] text-sm">
              + {{ lastTask.xpAwarded }} XP
            </td>
          </tr>
        </table>
      </div>
    </section>
  </main>
</template>

<style scoped>
.active {
  background: var(--primary-color);
  border: 1px solid var(--color-emerald-200);
  color: var(--text-color-white);
}

.subtitle {
  text-transform: uppercase;
  color: var(--text-color-light);
  font-size: var(--text-sm);
  line-height: calc(1.25 / 0.875);
}

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
  background-color: var(--color-gray-100);
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
