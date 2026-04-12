<script lang="ts" setup>
  const props = defineProps<{
    link: string
  }>()
</script>

<template>
  <div class="bg-[var(--surface-color)]">
  <RouterLink :to="props.link">
    <span class="font-bold text-[var(--text-color)]">
    Focus<span class="text-[var(--primary-color)]">Up</span>
    </span>
  </RouterLink>
  </div>
</template>
