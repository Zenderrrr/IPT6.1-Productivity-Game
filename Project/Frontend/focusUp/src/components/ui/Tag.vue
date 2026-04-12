<script lang="ts" setup>
const props = defineProps<{
  name: string
  colorHex: string
  textColorHex: string
}>()
</script>

<template>
  <div class="tag rounded-lg px-2 py-1 text-xs font-semibold" :style="{ background: props.colorHex, color: props.textColorHex }">
    <span>{{ props.name }}</span>
  </div>
</template>
