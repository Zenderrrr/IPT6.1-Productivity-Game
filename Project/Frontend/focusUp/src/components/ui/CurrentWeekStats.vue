<script lang="ts" setup>
  import { formatNumber } from '@/utils/helpers.ts'

  const props = defineProps<{
    statsValue: number,
    name: string,
    colorTailwind?: string,
    digit?: string,
  }>();
</script>

<template>
  <div :class="colorTailwind" class="flex flex-col items-center justify-center bg-[var(--background-color)] rounded-lg px-3 py-2">
    <span class="font-bold text-xl">{{ formatNumber(props.statsValue) }} {{ props.digit }}</span>
    <span class="font-normal text-xs text-[var(--text-color-light)]">{{ props.name }}</span>
  </div>
</template>
