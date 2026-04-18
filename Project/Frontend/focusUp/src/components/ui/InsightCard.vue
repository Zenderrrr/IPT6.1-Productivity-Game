<script lang="ts" setup>
  const props = defineProps<{
    title: string,
    cardValue: string,
    description: string,
    svg: string,
  }>()
</script>

<template>
  <div class="base-element w-full">
    <div class="flex items-center justify-start gap-3">

      <!-- svg-->
      <div class="flex items-center justify-center rounded-lg w-[35px] h-[35px] bg-[var(--primary-color-light)] border border-[var(--primary-color)] text-[var(--primary-color)]">
        <i :class="props.svg"></i>
      </div>

      <!-- title and value-->
      <div class="flex flex-col items-start justify-center">
        <span class="uppercase text-[var(--text-color-light)] text-xs">{{ props.title }}</span>
        <span class="font-semibold text-lg">{{ props.cardValue}}</span>
      </div>
    </div>

    <!-- description-->
    <div class="mt-3">
      <p class="text-sm text-[var(--text-color-light)]">{{ props.description }}</p>
    </div>

    <div>
      <slot></slot>
    </div>
  </div>
</template>
