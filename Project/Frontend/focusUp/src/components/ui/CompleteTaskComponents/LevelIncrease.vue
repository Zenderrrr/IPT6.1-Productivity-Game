<script lang="ts" setup>
import PopUpTaskCompletion from '@/components/ui/CompleteTaskComponents/PopUpTaskCompletion.vue'
import { computed, onMounted, ref } from 'vue'

const props = defineProps<{
  levelBeforeXP: number
  totalTaskXP: number
  currXP: number
  nextLevelXP: number
}>()

const emit = defineEmits<{
  (e: 'submit'): void
}>()

function onSubmit() {
  emit('submit')
}

function wait(ms: number) {
  return new Promise((resolve) => setTimeout(resolve, ms))
}

const displayedXP = ref<number>(props.currXP)
const displayedTaskXP = ref<number>(0)
const displayedNextLevelXP = ref<number>(props.nextLevelXP)
const displayedLevel = ref<number>(props.levelBeforeXP)
const showLevelUp = ref<boolean>(false)
async function animateLevelProgress(){
  let remainingXP = props.totalTaskXP

  while(remainingXP > 0){
    remainingXP--
    displayedXP.value += 1
    displayedTaskXP.value += 1

    if(displayedXP.value >= displayedNextLevelXP.value){
      displayedXP.value = 0
      displayedLevel.value += 1
      displayedNextLevelXP.value = getXpForNextLevel(displayedLevel.value)

      showLevelUp.value = true
      await wait(2000)
      showLevelUp.value = false
    }

    const progress =  1 - (remainingXP / props.totalTaskXP)
    await wait(5 + 70 * Math.pow(progress, 2))
  }
}

function getXpForNextLevel(level: number) : number {
  return 100 * Math.pow(level, 2)
}

onMounted(async() => {
  await wait(400)
  await animateLevelProgress()
})

const progress = computed(() => {
  return (100 / displayedNextLevelXP.value) * displayedXP.value
})
</script>

<template>
  <PopUpTaskCompletion>
    <!-- Level text-->
    <div class="base-element min-w-[500px]">
      <div class="w-full flex justify-center items-center gap-2 text-sm">
        <i class="fa-solid fa-web-awesome"></i>
        <span class="uppercase">Level Belohnung</span>
      </div>

      <div class="flex flex-col justify-center items-center mt-5">
        <span class="text-[75px] font-extrabold text-[var(--primary-color)] leading-20"
          >+ {{ displayedTaskXP }} XP</span
        >
        <span class="text-[var(--text-color-light)] text-sm">insgesamt verdient</span>
      </div>

      <div class="flex justify-between items-center gap-2 text-sm mt-3">
        <span class="font-semibold">Level {{ displayedLevel }}</span>
        <span class="text-[var(--text-color-light)]"
          >{{ displayedXP }} / {{ displayedNextLevelXP }} XP</span
        >
      </div>
      <div class="mt-2 w-full h-3 rounded-full bg-gray-200">
        <div
          :style="{ width: `${progress}%` }"
          class="h-full bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] rounded-full"
        ></div>
      </div>

      <Transition name="popUp">
        <div v-if="showLevelUp" class="mt-3 flex justify-start items-center text-sm">
          <div
            class="flex gap-1 px-4 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] rounded-full text-[var(--text-color-white)] text-sm font-semibold"
          >
            <div class="flex justify-center items-center">
              <i class="fa-solid fa-angle-up"></i>
            </div>
            <span class="uppercase">Level Up</span>
          </div>
        </div>
      </Transition>

      <!-- divider-->
      <div style="--delay: 4.5s" class="appear w-full h-0.5 bg-gray-100 my-4"></div>

      <!-- Continue button-->
      <div
        @click="onSubmit"
        class="appear scale-animation-sm cursor-pointer rounded-lg w-full px-5 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] flex justify-center items-center"
      >
        <button class="cursor-pointer">Weiter</button>
      </div>
    </div>
  </PopUpTaskCompletion>
</template>

<style scoped>
.level-increase {
  animation: levelIncrease 1s;
}

@keyframes levelIncrease {
}
</style>
