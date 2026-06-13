<script lang="ts" setup>
  import { onMounted } from 'vue'
  import { applyUIMode } from '@/utils/modeUI.ts'

  const props = defineProps<{
    checked: boolean,
    name: String,
    svg: String,
    colorHex: String,
    svgColorHex: String,
  }>();
</script>

<template>
  <div :class="checked ? 'checked lift-hover' : 'unchecked' " class="badge text-center flex flex-col gap-2 justify-center items-center rounded-ls border-2 border-[var(--border-color)]">
    <div class="flex justify-center items-center w-[45px] h-[45px] p-2 bg-[var(--primary-color-light)] text-[var(--primary-color)] rounded-lg">
      <div v-html="svg" class="w-full h-full"></div>
    </div>
    <span class="font-semibold text-sm">{{ props.name }}</span>
  </div>
</template>

<style scoped>
  .badge{
    border-radius: var(--radius-xl);
    padding: calc(var(--spacing) * 5) calc(var(--spacing) * 5);
    box-shadow: var(--shadow-lg);
    background: var(--surface-color);
  }

  .checked{
    cursor: pointer;
  }

  .unchecked{
    background-color: oklch(96.7% 0.003 264.542);
    box-shadow: none;
  }

  html.dark .unchecked{
    background-color: var(--surface-color);
  }

  .unchecked div{
    background-color: oklch(92.8% 0.006 264.531);
  }

  html.dark .unchecked div{
    background-color: var(--background-color);
  }

  .unchecked span, .unchecked div{
    color: var(--text-color-light);
  }

  html.dark .unchecked span, html.dark .unchecked div{
    color: var(--primary-color);
  }
</style>
