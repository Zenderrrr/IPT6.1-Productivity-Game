<script lang="ts" setup>
const props = defineProps<{
  title: string
  description: string
  fPoint: string
  sPoint: string
  tPoint: string
}>()
</script>

<template>
  <div
    class="group hover:shadow-xl hover:border-[var(--primary-color)] transition duration-100 relative overflow-hidden min-h-[350px] flex flex-col gap-2 px-8 py-7 border border-[var(--border-color)] rounded-2xl shadow-sm bg-[var(--surface-color)]"
  >
    <div
      class="group-hover:block transition duration-100 hidden absolute top-0 left-0 right-0 h-[3px] bg-[var(--primary-color)]"
    ></div>

    <div
      class="text-xl flex items-center justify-center w-[45px] h-[45px] rounded-xl bg-[var(--primary-color-light)] text-[var(--primary-color)]"
    >
      <i class="fa-regular fa-clipboard"></i>
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
        > {{ props.tPoint }}</span
      >
    </div>
  </div>
</template>
