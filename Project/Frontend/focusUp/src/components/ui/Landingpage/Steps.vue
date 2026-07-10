<script lang="ts" setup>
const props = defineProps<{
  number: number
  title: string
  description: string
}>()
</script>

<template>
  <div class="flex flex-col items-center gap-4">
    <div
      class="select-none steps relative flex items-center justify-center w-[50px] h-[50px] border-2 border-[var(--primary-color)] bg-[var(--surface-color)] rounded-full font-bold text-[var(--primary-color)] text-2xl"
    >
      <span>{{ props.number }}</span>
    </div>

    <div class="flex flex-col items-center justify-center">
      <span class="font-bold w-full text-center text-lg">{{ props.title }}</span>
      <span class="text-center w-full text-[var(--text-color-light)] mt-2 max-w-[350px]"
        >{{ props.description }}</span
      >
    </div>

    <div
      class="select-none w-full max-w-[400px] h-[180px] bg-[var(--background-color)] border border-[var(--border-color)] shadow-sm rounded-xl px-4 py-4"
    >
      <slot></slot>
    </div>
  </div>
</template>
