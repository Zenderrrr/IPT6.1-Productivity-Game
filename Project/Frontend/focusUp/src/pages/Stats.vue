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
import categories from '@/components/ui/Categories.vue'

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
  return prodInfoInd.value?.map((t) => t.completedTasks)
})

const prodTrend = computed(() => {
  const arr = values.value ?? []

  if(arr.length < 2) return 0

  const last = arr[arr.length - 1]
  const prev = arr[arr.length - 2]

  return last - prev
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

const totalTasksThisWeek = computed(() => {
  return prodInfoInd.value?.reduce((i,item) => i + item.completedTasks, 0)
})

const taskPerCategory = computed(() => {
  const map = new Map<string, number>()

  for (const task of lastCompletedTasks.value ?? []){
    const name = task.category?.name ?? 'Keine'
    map.set(name, (map.get(name) ?? 0) + 1)
  }

  return Array.from(map, ([name, value]) => ({
    name,
    value,
  }))
})
const optionDoughnut = computed(() => ({
  title: {
    text: totalTasksThisWeek.value ?? 0,
    subtext: 'Tasks',
    left: 'center',
    top: 'center',
    textStyle: {
      fontSize: '28px',
      fontWeight: 'bold',
    },
    subtextStyle: {
      fontSize: '12px',
      color: '#64748B',
    },
    itemGap: 3,
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
      data: taskPerCategory.value,
      radius: ['50%', '70%'],
    },
  ],
}))

const accumulatedXpIncrease = computed(() => {
  let sum = 0
  return (prodInfoInd.value ?? []).map(t => sum += t.xpGained)
})
const increaseXpPerDay = computed(() => {
  return (prodInfoInd.value ?? []).map(t => t.xpGained)
})
const optionLine = computed(() => ({
  xAxis: {
    data: labels.value ?? [],
  },
  yAxis: [
    {
      type: 'value',
      position: 'left',
      splitLine: {
        show: true,
      }
    },
    {
      type: 'value',
      position: 'right',
      splitLine: {
        show: false,
      }
    }
  ],
  grid: {
    left: 20,
    right: 20,
    bottom: 20,
    top: 30,
    containLabel: false,
  },
  series: [
    {
      data: accumulatedXpIncrease.value,
      type: 'line',
      smooth: true,
      lineStyle: {
        color: '#0EA5E9'
      },
      itemStyle: {
        color: '#0EA5E9'
      },
      yAxisIndex: 0,
    },
    {
      data: increaseXpPerDay.value,
      type: 'line',
      smooth: true,
      lineStyle: {
        color: '#14B8A6'
      },
      itemStyle: {
        color: '#14B8A6'
      },
      yAxisIndex: 1,
    },
  ],
}))

const prodInfo91Days = ref<Productivity[] | null>(null)
const week = Array.from({ length: 13 }, (_, i) => `W${i + 1}`)
const days = ['Sa', 'Fr', 'Do', 'Mi', 'Di', 'Mo', 'So']

const heatmapData = computed(() => {
    const data = prodInfo91Days.value ?? []

    if(data.length === 0) return []

    const sorted = [...data].sort(
      (a, b) => new Date(a.date).getTime() - new Date(b.date).getTime()
    )

    const start = new Date(sorted[0].date)

    return data.map(item => {
      const date = new Date(item.date)

      const diffDays = Math.floor(
        (date.getTime() - start.getTime()) / (1000 * 60 * 60 * 24)
      )

      const week = Math.floor(diffDays / 7)

      const day = date.getDay() === 0 ? 6 : date.getDay() - 1

      return [week, day, item.xpGained]
    })
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
    data: week,
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
        '#f3f4f6',
        '#ebf8f7',
        '#96f7e4',
        '#14B8A6',
        '#0f766e',
      ],
    },
  },
  series: [
    {
      name: 'Aktivitätsmap',
      type: 'heatmap',
      data: heatmapData.value,
      itemStyle: {
        borderRadius: 4,
        borderWidth: 2,
        borderColor: '#FFFFFF',
      },
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

  return Math.round((((last7 - previous7) / previous7) * 100))
})

const totalXpTrend = computed(() => {
  const last14 = statsLast14Days.value?.totalXp ?? 0
  const last7 = statsLast7Days.value?.totalXp ?? 0

  const previous7 = last14 - last7

  if (previous7 === 0) return 0

  return Math.round(((last7 - previous7) / previous7) * 100)
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

    await statsStore.productivity(91)
    prodInfo91Days.value = statsStore.productivityData
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
<!--      <div-->
<!--        class="h-full flex items-center justify-center px-3 py-2 gap-4 bg-[var(&#45;&#45;surface-color)] border border-gray-200 rounded-lg text-[var(&#45;&#45;text-color-light)]"-->
<!--      >-->
<!--        <button class="active px-2 py-1 rounded-lg cursor-pointer hover:dark:bg-gray-100 transition duration-200">Heute</button>-->
<!--        <button class="px-2 py-1 rounded-lg cursor-pointer hover:dark:bg-gray-100 transition duration-200">Woche</button>-->
<!--        <button class="px-2 py-1 rounded-lg cursor-pointer hover:dark:bg-gray-100 transition duration-200">Monat</button>-->
<!--        <button class="px-2 py-1 rounded-lg cursor-pointer hover:dark:bg-gray-100 transition duration-200">Benutzerdefiniert</button>-->
<!--        <div class="h-[25px] w-0.5 rounded-full bg-gray-100"></div>-->
<!--        <button class="px-2 py-1 rounded-lg cursor-pointer hover:dark:bg-gray-100 transition duration-200">Alle Kategorien</button>-->
<!--      </div>-->
      <div
        class="hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] transition duration-200 cursor-pointer text-md flex justify-center items-center gap-2 text-[var(--text-color-light)] border border-gray-300 rounded-lg px-2 py-3"
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
            v-if="taskDoneTrend > 0"
            class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-up"></i>
            </div>
            <span>{{ taskDoneTrend }}% vs. letzer Woche</span>
          </div>
          <div
            v-if="taskDoneTrend < 0"
            class="flex justify-center items-center gap-1 text-xs mt-1 text-red-500 bg-red-50 rounded-full px-2 py-1 border border-red-500"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-down"></i>
            </div>
            <span>{{ taskDoneTrend }}% vs. letzer Woche</span>
          </div>
          <div v-else class="h-[30px]"></div>
        </StatsCard>

        <StatsCard
          title="Gesamt XP"
          svg="fa-solid fa-bolt"
          :card-value="statsStore.dashboardData?.totalXp"
          primary-color="#0EA5E9"
          secondary-color="rgba(189, 234, 255, 0.63)"
        >
          <div
            v-if="totalXpTrend > 0"
            class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-up"></i>
            </div>
            <span>{{ totalXpTrend }}% vs. letzer Woche</span>
          </div>
          <div
            v-if="totalXpTrend < 0"
            class="flex justify-center items-center gap-1 text-xs mt-1 text-red-500 bg-red-50 rounded-full px-2 py-1 border border-red-500"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-down"></i>
            </div>
            <span>{{ totalXpTrend }}% vs. letzer Woche</span>
          </div>
          <div v-else class="h-[30px]"></div>
        </StatsCard>

        <StatsCard
          title="⌀ Tasks pro Tag (letzte 7 Tage)"
          svg="fa-solid fa-chart-line"
          :card-value="Math.round(((statsStore.statsData?.tasksDone ?? 0) / 7) * 10) / 10"
          primary-color="oklch(79.2% 0.209 151.711)"
          secondary-color="oklch(96.2% 0.044 156.743)"
        >
          <div
            v-if="taskPerDayTrend !== 0"
            class="flex justify-center items-center gap-1 text-xs mt-1 text-[var(--accent-color)] bg-green-50 rounded-full px-2 py-1 border border-[var(--accent-color)]"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-up"></i>
            </div>
            <span>{{ taskPerDayTrend }}% vs. letzer Woche</span>
          </div>
          <div
            v-if="taskPerDayTrend < 0"
            class="flex justify-center items-center gap-1 text-xs mt-1 text-red-500 bg-red-50 rounded-full px-2 py-1 border border-red-500"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-down"></i>
            </div>
            <span>{{ taskPerDayTrend }}% vs. letzer Woche</span>
          </div>
          <div v-else class="h-[30px]"></div>
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
          <div v-else class="h-[30px]"></div>
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
              v-if="prodTrend > 0"
              class="flex items-center justify-center gap-2 text-sm text-[var(--primary-color)] bg-[var(--primary-color-light)] border border-[var(--primary-color)] rounded-full px-3 py-1.5"
            >
              <div class="flex justify-center items-center">
                <i class="fa-solid fa-arrow-up"></i>
              </div>
              <span>Steigend</span>
            </div>

            <div
              v-if="prodTrend < 0"
              class="flex items-center justify-center gap-2 text-sm text-red-500 bg-red-50 border border-red-500 rounded-full px-3 py-1.5"
            >
              <div class="flex justify-center items-center">
                <i class="fa-solid fa-arrow-down"></i>
              </div>
              <span>Sinkend</span>
            </div>

            <div
              v-if="prodTrend === 0"
              class="flex items-center justify-center gap-2 text-sm text-[var(--primary-color)] bg-[var(--primary-color-light)] border border-[var(--primary-color)] rounded-full px-3 py-1.5"
            >
              <div class="flex justify-center items-center">
                <i class="fa-solid fa-grip-lines"></i>
              </div>
              <span>Stabil</span>
            </div>
          </div>

          <div class="flex gap-5 mt-3">
            <div
              class="flex items-center justify-center gap-2 text-sm text-[var(--text-color-light)]"
            >
              <div class="w-[12px] h-[12px] bg-[var(--primary-color)] rounded-sm"></div>
              <span>Tasks</span>
            </div>

<!--            <div-->
<!--              class="flex items-center justify-center gap-2 text-sm text-[var(&#45;&#45;text-color-light)]"-->
<!--            >-->
<!--              <div class="w-[12px] h-[12px] bg-gray-200 rounded-sm"></div>-->
<!--              <span class="text-nowrap">Ziel (7/Tag)</span>-->
<!--            </div>-->
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

        <div class="w-full" :style="{ aspectRatio: `${week.length} / ${days.length}` }">
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
            <span v-if="statsStore.dashboardData?.lastCompletedTasks.length === 20" class="text-[var(--text-color-light)] text-sm"
              >{{ statsStore.dashboardData?.lastCompletedTasks.length }} max. Tasks diese Woche</span
            >
            <span v-if="(statsStore.dashboardData?.lastCompletedTasks.length ?? 0) < 20" class="text-[var(--text-color-light)] text-sm"
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
