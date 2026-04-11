<script lang="ts" setup>
  import { formatNumber } from '@/utils/helpers.ts'

  const props = defineProps<{
    svg: string,
    statsName: string,
    statsValue: number
  }>();
</script>

<template>
  <div class="w-full h-full bg-[var(--surface-color)] rounded-2xl px-5 py-4">
    <div class="flex items-center justify-start gap-2">
      <div class="flex justify-center items-center w-[30px] h-[30px] bg-[var(--primary-color-light)] rounded-lg">
        <i :class="svg" class="rounded-full text-xs text-[var(--primary-color)]"></i>
      </div>
      <span class="text-[var(--text-color-light)] uppercase text-xs font-semibold">{{ props.statsName }}</span>
    </div>
    <div class="text-3xl font-bold mt-3 mb-2">
      {{ formatNumber(props.statsValue) }}
    </div>
    <div class="text-xs text-[var(--text-color-light)] font-semibold">
      <slot></slot>
    </div>
  </div>
</template>
