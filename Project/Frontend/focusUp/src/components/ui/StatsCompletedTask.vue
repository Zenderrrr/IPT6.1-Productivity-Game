<script lang="ts" setup>
import { GetTimeFromNow } from '@/utils/date.ts'

const props = defineProps<{
  title: string
  date: Date
  xp: Number
}>()
</script>

<template>
  <div
    class="flex gap-2 justify-between items-center bg-[var(--background-color)] px-4 py-3 rounded-2xl"
  >
    <div class="flex gap-4 items-center">
      <div
        class="w-[22px] h-[22px] flex justify-center items-center text-center bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] text-xs font-light rounded-2xl"
      >
        <i class="fa-solid fa-check"></i>
      </div>
      <div class="flex flex-col gap-2 font-light">
        <p class="font-bold text-sm">{{ props.title }}</p>
        <span class="text-[var(--text-color-light)] text-xs font-semibold">{{ GetTimeFromNow(props.date) }}</span>
      </div>
    </div>
    <div
      class="text-center text-[var(--primary-color)] bg-[var(--primary-color-light)] rounded-2xl px-2 py-1 text-xs font-semibold border border-[var(--primary-color)]"
    >
      + {{ props.xp }} XP
    </div>
  </div>
</template>
