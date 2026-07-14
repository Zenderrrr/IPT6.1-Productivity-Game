<script lang="ts" setup>
import { onMounted, onUnmounted, ref } from 'vue'

const props = defineProps<{
  kicker: string
  title: string
  subtitle: string
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
  <section ref="sectionRef" class="px-8 py-30">
    <div class="max-w-[1400px] mx-auto">
      <div class="text-center">
        <span
          :class="!isVisible ? 'opacity-0 -translate-y-[35px]' : 'opacity-100 translate-0'"
          class="transition-all duration-700 uppercase text-[var(--primary-color)] text-sm font-semibold"
          >{{ props.kicker }}</span
        >
        <h2 :class="!isVisible ? 'opacity-0 -translate-y-[35px]' : 'opacity-100 translate-0'" class="delay-100 transition-all duration-700 text-[40px] font-extrabold">{{ props.title }}</h2>
        <div class="w-full flex justify-center">
          <p :class="!isVisible ? 'opacity-0 -translate-y-[35px]' : 'opacity-100 translate-0'" class="delay-200 transition-all duration-700 text-[var(--text-color-light)] text-md max-w-[600px]">
            {{ props.subtitle }}
          </p>
        </div>
      </div>

      <slot></slot>
    </div>
  </section>
</template>
