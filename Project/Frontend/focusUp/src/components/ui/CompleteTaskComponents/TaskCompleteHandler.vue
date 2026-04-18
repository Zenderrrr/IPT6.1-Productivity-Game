<script lang="ts" setup>
import StreakUpdate from '@/components/ui/CompleteTaskComponents/StreakUpdate.vue'
import XpUpdate from '@/components/ui/CompleteTaskComponents/XpUpdate.vue'
import { computed, onMounted, ref } from 'vue'
import LevelIncrease from '@/components/ui/CompleteTaskComponents/LevelIncrease.vue'
import BadgeUpdate from '@/components/ui/CompleteTaskComponents/BadgeUpdate.vue'
import { useStatsStore } from '@/stores/statsStore.ts'
import type { TaskCompleteType } from '@/types/taskComplete.ts'

const props = defineProps<{
  tasksCompleteArr: TaskCompleteType[]
}>()


const emit = defineEmits<{
  (e: 'close'): void
}>()

function onStreakSubmit() {
  step.value = 2
}

const tasks = computed(() => {
  return props.tasksCompleteArr.map((t: TaskCompleteType) => t.task)
})
const newBadges = props.tasksCompleteArr.flatMap((t: TaskCompleteType) => t.badges)
const isStreakCountIncreased = props.tasksCompleteArr.some(t => t.isStreakIncreased)

const step = ref<number>(isStreakCountIncreased ? 1 : 2)

const xpUpdateCount = ref<number>(0)
const currentTask = computed(() => tasks.value[xpUpdateCount.value])

function onXpSubmit() {
  if(xpUpdateCount.value < tasks.value.length - 1) {
    xpUpdateCount.value += 1
    return
  }
  step.value = 3
}

function onLevelSubmit() {
  step.value = 4
}

const badgeUpdateCount = ref<number>(0)
function onClose() {
  if(badgeUpdateCount.value < (newBadges.length ?? 0)) {
    badgeUpdateCount.value += 1
    return
  }
  emit('close')
}

// stats logic
const statsStore = useStatsStore()
const level = computed(() => statsStore.dashboardData?.level ?? 0)
const totalTaskXP = props.tasksCompleteArr.reduce((sum, item) => sum + item.xp, 0)
const currXP = computed(() => statsStore.dashboardData?.xpCurrent ?? 0)
const nextLevelXP = computed(() => statsStore.dashboardData?.xpNext ?? 0)
</script>

<template>
  <StreakUpdate v-if="step === 1 && isStreakCountIncreased" @submit="onStreakSubmit" :streak-count-after="props.tasksCompleteArr[0]?.streakCountAfter ?? 0" :streak-count-before="props.tasksCompleteArr[0]?.streakCountBefore ?? 0"></StreakUpdate>

  <div v-if="step === 2">
    <XpUpdate
      v-if="currentTask !== null && currentTask !== undefined"
      :key="currentTask.id"
      @submit="onXpSubmit"
      @skip="step = 3"
      :task="currentTask"
    ></XpUpdate>
  </div>

  <LevelIncrease
    v-if="step === 3"
    @submit="onLevelSubmit"
    :level-before-x-p="level"
    :total-task-x-p="totalTaskXP"
    :curr-x-p="currXP"
    :next-level-x-p="nextLevelXP"
  ></LevelIncrease>

  <div v-if="step === 4">
    <BadgeUpdate
      v-if="newBadges[badgeUpdateCount] !== null && newBadges[badgeUpdateCount] !== undefined"
      @submit="onClose"
      :key="newBadges[badgeUpdateCount]!.id"
      :badge-name="newBadges[badgeUpdateCount]!.name"
      :badge-description="newBadges[badgeUpdateCount]!.description"
      svg-color=""
      badge-icon=""
      background-badge-color=""
    ></BadgeUpdate>
  </div>
</template>
