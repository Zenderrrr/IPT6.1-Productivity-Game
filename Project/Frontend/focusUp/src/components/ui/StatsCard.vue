<script lang="ts" setup>
const props = defineProps<{
  svg: string
  primaryColor: string
  secondaryColor: string
  title: string
  cardValue: number
}>()
</script>

<template>
  <div class="relative base-element w-full overflow-hidden">
    <div :style="{backgroundColor: `${props.primaryColor}`}" class="absolute top-0 left-0 w-full h-1"></div>
    <div class="flex items-center justify-start">
      <div
        :style="{ color: `${props.primaryColor}`, backgroundColor: `${props.secondaryColor}` }"
        class="flex items-center justify-center w-[35px] h-[35px] rounded-lg"
      >
        <i :class="props.svg"></i>
      </div>
    </div>

    <div class="mt-3 flex flex-col items-start justify-center">
      <span class="text-[var(--text-color-light)] text-sm text-nowrap">{{ props.title }}</span>
      <span class="text-[30px] font-semibold">{{ props.cardValue }}</span>

      <slot></slot>
    </div>
  </div>
</template>
