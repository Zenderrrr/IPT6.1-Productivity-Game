<script lang="ts" setup>
import { onMounted, onUnmounted, ref } from 'vue'

const props = defineProps<{
  svg: string
  pColor: string
  sColor: string
  title: string
  description: string
  fPoint: string
  sPoint: string
  tPoint: string
}>()

const sectionRef = ref<HTMLElement | null>(null)
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

  if (sectionRef.value) {
    observer.observe(sectionRef.value)
  }
})

onUnmounted(() => {
  observer?.disconnect()
})
</script>

<template>
  <div ref="sectionRef" :class="!isVisible ? 'opacity-0 translate-y-[35px]' : 'opacity-100 translate-0' " class="transition-all duration-1000" >
    <div
      class="group hover:shadow-xl hover:border-[var(--primary-color)] transition duration-100 relative overflow-hidden min-h-[350px] flex flex-col gap-2 px-8 py-7 border border-[var(--border-color)] rounded-2xl shadow-sm bg-[var(--surface-color)]"
    >
      <div
        class="group-hover:block transition duration-100 hidden absolute top-0 left-0 right-0 h-[3px] bg-[var(--primary-color)]"
      ></div>

      <div
        class="text-xl flex items-center justify-center w-[45px] h-[45px] rounded-xl bg-[var(--primary-color-light)] text-[var(--primary-color)]"
      >
        <i :class="props.svg"></i>
      </div>

      <span class="font-bold text-[var(--text-color)] text-lg">{{ props.title }}</span>
      <p class="text-[var(--text-color-light)] text-md">{{ props.description }}</p>
      <div class="mt-3">
        <span
          class="mr-2 text-xs px-2 py-1 bg-[var(--background-color)] border border-[var(--border-color)] rounded-full font-bold text-[var(--text-color-light)]"
          >{{ props.fPoint }}</span
        >
        <span
          class="mr-2 text-xs px-2 py-1 bg-[var(--background-color)] border border-[var(--border-color)] rounded-full font-bold text-[var(--text-color-light)]"
          >{{ props.sPoint }}</span
        >
        <span
          class="mr-2 text-xs px-2 py-1 bg-[var(--background-color)] border border-[var(--border-color)] rounded-full font-bold text-[var(--text-color-light)]"
        >
          {{ props.tPoint }}</span
        >
      </div>
    </div>
  </div>
</template>
