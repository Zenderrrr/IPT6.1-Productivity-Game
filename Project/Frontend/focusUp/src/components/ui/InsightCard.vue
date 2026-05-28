<script lang="ts" setup>
  const props = defineProps<{
    title: string,
    cardValue: string,
    description: string,
    svg: string,
    type: string,
  }>()
</script>

<template>
  <div :class="type" class="select-none base-element w-full">
    <div class="flex items-center justify-start gap-3">

      <!-- svg-->
      <div class="svgEl flex items-center justify-center rounded-lg w-[35px] h-[35px] bg-[var(--primary-color-light)] border border-[var(--primary-color)] text-[var(--primary-color)]">
        <i :class="props.svg"></i>
      </div>

      <!-- title and value-->
      <div class="flex flex-col items-start justify-center">
        <span class="title uppercase text-[var(--text-color-light)] text-xs font-semibold">{{ props.title }}</span>
        <span class="font-semibold text-lg">{{ props.cardValue}}</span>
      </div>
    </div>

    <!-- description-->
    <div class="mt-3">
      <p class="description text-sm text-[var(--text-color-light)]">{{ props.description }}</p>
    </div>

    <div>
      <slot></slot>
    </div>
  </div>
</template>

<style scoped>
  .success, .success .svgEl, .success .description, .success .title{
    border-color: var(--color-green-500);
    background-color: var(--color-green-50);
    color: var(--color-green-500);
  }

  .success:hover{
    border-color: var(--color-green-600);
  }

  .warning, .warning .svgEl, .warning .description, .warning .title{
    border-color: var(--color-yellow-500);
    background-color: var(--color-yellow-50);
  }

  .warning:hover{
    border-color: var(--color-yellow-600);
  }

  .danger, .danger .svgEl, .danger .description, .danger .title{
    border-color: var(--color-red-500);
    background-color: var(--color-red-50);
  }

  .danger:hover{
    border-color: var(--color-red-600);
  }

  .info, .info .svgEl, .info .description, .info .title{
    border-color: var(--color-gray-500);
    background-color: var(--color-gray-50);
  }

  .info:hover{
    border-color: var(--color-green-600);
  }
</style>
