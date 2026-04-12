<script lang="ts" setup>
import { formatTime } from '@/utils/date.ts'

const props = defineProps<{
  title: string
  userName?: string
  subtitle: string
}>()

const date = new Date()
const day = new Intl.DateTimeFormat('de-CH', { weekday: 'long' }).format(date)
const month = new Intl.DateTimeFormat('de-CH', { month: 'long' }).format(date)
</script>

<template>
  <section>
    <header class="flex items-center justify-between">
      <div>
        <h1 class="font-bold text-3xl tracking-wide">
          {{ props.title}}
          <span class="text-[var(--primary-color)]"> {{ props.userName }}</span>
        </h1>
        <p class="text-[var(--text-color-light)] mt-1"> {{ props.subtitle }}</p>
      </div>
      <div
        class="text-[var(--text-color-light)] text-sm font-semibold px-2.5 py-1.5 bg-[var(--surface-color)] border border-gray-200 rounded-full"
      >
        {{ ` ${day}, ${formatTime(date.getDate())} ${month} ${date.getFullYear()}` }}
      </div>
    </header>
  </section>
</template>
