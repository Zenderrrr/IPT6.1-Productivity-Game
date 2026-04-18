<script lang="ts" setup>
import PopUpTaskCompletion from '@/components/ui/CompleteTaskComponents/PopUpTaskCompletion.vue'
import { computed, onMounted, ref } from 'vue'
import type { Task } from '@/types/task.ts'
import { useStatsStore } from '@/stores/statsStore.ts'
import { status } from '@/utils/status.ts'

const props = defineProps<{
  task: Task
}>()

const emit = defineEmits<{
  (e: 'submit'): void,
  (e: 'skip'): void,
}>()

function onSubmit() {
  emit('submit')
}

function onSkip() {
  emit('skip')
}

const statsStore = useStatsStore()

// xp calculation
const baseXp = Math.round(props.task.difficulty * 10)
const timeXp = Math.round(props.task.durationMin / 3)
const streakBonus = Math.round((baseXp + timeXp) * ((statsStore.dashboardData?.streakCount ?? 0) * 0.02))
const tempBonus = Math.round((baseXp + timeXp + streakBonus) * 0.05)

const totalXP = computed(() => {
  return Math.floor(baseXp + timeXp + streakBonus + tempBonus)
})

function wait(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms))
}

const animateXP = ref<number>(0)
async function xpAnimate() {
  await wait(4500)
  while (animateXP.value < totalXP.value) {
    animateXP.value += 1
    await wait(10)
  }
}

onMounted(async () => {
  await xpAnimate()
})
</script>

<template>
  <PopUpTaskCompletion>
    <div class="base-element min-w-[500px]">
      <!-- XP Text-->
      <div
        style="--delay: 0.1s"
        class="appear w-full flex justify-center items-center gap-2 text-sm"
      >
        <i class="fa-solid fa-bolt"></i>
        <span class="uppercase">XP Übersicht</span>
      </div>

      <!-- Task item-->
      <div
        style="--delay: 1s"
        class="appear w-full flex justify-start items-center gap-2 text-sm mt-5 bg-gray-50 px-4 py-2 rounded-lg border border-gray-100"
      >
        <div
          class="flex justify-center items-center w-[40px] h-[40px] border border-[var(--primary-color)] bg-[var(--primary-color-light)] text-[var(--primary-color)] rounded-lg"
        >
          <i class="fa-solid fa-bolt"></i>
        </div>
        <div class="flex flex-col items-start justify-center text-sm">
          <span class="font-semibold text-sm">{{ props.task.title }}</span>
          <span class="text-xs text-[var(--text-color-light)]"
            >{{ status[props.task.difficulty] }} - {{ props.task.durationMin }} Min.</span
          >
        </div>
      </div>

      <div style="--delay: 2s" class="mt-6">
        <div class="appear flex flex-col gap-3 mt-3">
          <div class="px-4 flex justify-between items-center gap-2 text-sm">
            <div class="flex justify-center items-center gap-2 text-[var(--text-color)]">
              <i class="fa-solid fa-bolt"></i>
              <span>Basis XP</span>
            </div>
            <div>
              <span class="text-[var(--primary-color)] font-semibold">+ {{ baseXp }} XP</span>
            </div>
          </div>

          <!-- divider-->
          <div class="w-full h-0.5 bg-gray-100"></div>
        </div>

        <div style="--delay: 2.5s" class="appear flex flex-col gap-3 mt-3">
          <div class="px-4 flex justify-between items-center gap-2 text-sm">
            <div class="flex justify-center items-center gap-2 text-[var(--text-color)]">
              <i class="fa-regular fa-alarm-clock"></i>
              <span>Zeit Bonus</span>
            </div>
            <div>
              <span class="text-[var(--primary-color)] font-semibold">+ {{ timeXp }} XP</span>
            </div>
          </div>

          <!-- divider-->
          <div class="w-full h-0.5 bg-gray-100"></div>
        </div>

        <div style="--delay: 3s" class="appear flex flex-col gap-3 mt-3">
          <div class="px-4 flex justify-between items-center gap-2 text-sm">
            <div class="flex justify-center items-center gap-2 text-[var(--text-color)]">
              <i class="fa-solid fa-fire"></i>
              <span>Streak Bonus</span>
            </div>
            <div>
              <span class="text-[var(--primary-color)] font-semibold"
                >+ {{ streakBonus }} XP</span
              >
            </div>
          </div>

          <!-- divider-->
          <div class="w-full h-0.5 bg-gray-100"></div>
        </div>

        <div style="--delay: 3.5s" class="appear flex flex-col gap-3 mt-3">
          <div class="px-4 flex justify-between items-center gap-2 text-sm">
            <div class="flex justify-center items-center gap-2 text-[var(--text-color)]">
              <i class="fa-solid fa-rocket"></i>
              <span>Spezial Bonus</span>
            </div>
            <div>
              <span class="text-[var(--accent-color)] font-semibold"
                >+ {{ tempBonus }} XP</span
              >
            </div>
          </div>

          <!-- divider-->
          <div class="w-full h-0.5 bg-gray-100"></div>
        </div>
      </div>

      <div
        style="--delay: 4s"
        class="appear my-4 flex justify-between items-center gap-2 px-4 py-2 rounded-lg border border-[var(--primary-color)] bg-[var(--primary-color-light)]"
      >
        <span class="font-semibold">Gesamt</span>
        <span class="text-[var(--primary-color)] font-semibold">{{ animateXP }} XP</span>
      </div>

      <!-- divider-->
      <div style="--delay: 4.5s" class="appear w-full h-0.5 bg-gray-100 my-4"></div>

      <!-- Continue button-->
      <div class="flex gap-2">
        <div
          style="--delay: 5s"
          @click="onSubmit"
          class="w-full appear scale-animation-sm cursor-pointer rounded-lg px-5 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] flex justify-center items-center"
        >
          <button class="cursor-pointer">Weiter</button>
        </div>
        <div
          style="--delay: 5s"
          @click="onSkip"
          class="appear scale-animation-sm cursor-pointer rounded-lg text-nowrap px-5 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] flex justify-center items-center"
        >
          <button class="cursor-pointer">Skip</button>
        </div>
      </div>
    </div>
  </PopUpTaskCompletion>
</template>

<style scoped>
.appear {
  animation: appear 0.5s ease-out forwards;
  animation-delay: var(--delay);
  opacity: 0;
}

@keyframes appear {
  0% {
    opacity: 0;
  }
  70% {
    opacity: 1;
  }
  100% {
    opacity: 1;
  }
}
</style>
