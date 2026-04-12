<script lang="ts" setup>
  const props = defineProps<{
    checked: boolean,
    name: String,
    svg: String,
    colorHex: String,
    svgColorHex: String,
  }>();
</script>

<template>
  <div :class="checked ? 'checked' : 'unchecked' " class="badge flex flex-col gap-2 justify-center items-center rounded-ls border-2 border-gray-200">
    <div class="flex justify-center items-center w-[45px] h-[45px] bg-[var(--primary-color-light)] text-[var(--primary-color)] rounded-lg">
      <i class="fa-solid fa-trophy"></i>
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

  .unchecked{
    background-color: oklch(96.7% 0.003 264.542);
    box-shadow: none;
  }

  .unchecked div{
    background-color: oklch(92.8% 0.006 264.531);
  }

  .unchecked span, .unchecked div{
    color: var(--text-color-light);
  }
</style>
