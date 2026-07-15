<script lang="ts" setup>
import { onMounted, onUnmounted, ref } from 'vue'

const props = defineProps<{
  number: number
  title: string
  description: string
}>()

const elementRef = ref<HTMLElement | null>(null)
const isVisible = ref(false)

let observer: IntersectionObserver

onMounted(() => {
  observer = new IntersectionObserver(
    ([entry]) => {
      if (entry.isIntersecting) {
        isVisible.value = true
        observer.unobserve(entry.target)
      }
    },
    {
      threshold: 0.25,
    },
  )

  if (elementRef.value) {
    observer.observe(elementRef.value)
  }
})

onUnmounted(() => {
  observer?.disconnect()
})
</script>

<template>
  <div ref="elementRef" class="flex flex-col items-center gap-4">
    <div
      :class="!isVisible ? 'scale-0' : 'scale-100' "
      class="transition-all duration-700 select-none steps relative flex items-center justify-center w-[50px] h-[50px] border-2 border-[var(--primary-color)] bg-[var(--surface-color)] rounded-full font-bold text-[var(--primary-color)] text-2xl"
    >
      <span>{{ props.number }}</span>
    </div>

    <div class="flex flex-col items-center justify-center">
      <span :class="!isVisible ? 'show-up-off-md' : 'show-up-on' " class="transition-all duration-1000 font-bold w-full text-center text-lg">{{ props.title }}</span>
      <span :class="!isVisible ? 'show-up-off-md' : 'show-up-on' " class="delay-400 transition-all duration-700 text-center w-full text-[var(--text-color-light)] mt-2 max-w-[350px]">{{
        props.description
      }}</span>
    </div>

    <div
      :class="!isVisible ? 'show-up-off-xl' : 'show-up-on' "
      class="transition-all duration-1000 delay-800 select-none w-full max-w-[400px] h-[180px] bg-[var(--background-color)] border border-[var(--border-color)] shadow-sm rounded-xl px-4 py-4"
    >
      <slot></slot>
    </div>
  </div>
</template>
