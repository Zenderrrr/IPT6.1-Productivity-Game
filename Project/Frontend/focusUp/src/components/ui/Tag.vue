<script lang="ts" setup>
const props = defineProps<{
  name: string,
  colorHex: string,
  textColorHex: string,
  borderColorHex?: string
}>()
</script>

<template>
  <div class="tag rounded-lg px-2 py-1 text-xs font-semibold border border-[var(--primary-color)]" :style="{ background: props.colorHex, color: props.textColorHex, ...(props.borderColorHex ? { borderColor: props.borderColorHex } : { borderColor: 'transparent' })}">
    <span>{{ props.name }}</span>
  </div>
</template>
