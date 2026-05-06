<script lang="ts" setup>
const props = defineProps<{
  text: string,
  name: string,
  value: string,
  modelValue: string | null,
}>()

const emit = defineEmits<{
  (e: 'update:modelValue', value: string): void
}>()


</script>

<template>
  <label
    class="has-checked:border-[var(--primary-color)] hover:bg-gray-100 duration-75 transition flex items-center justify-center gap-2 border border-gray-200 rounded-full px-4 py-2 text-xs cursor-pointer"
  >
    <input type="radio" :name="props.name" :value="props.value" :checked="props.modelValue === props.value" @change="emit('update:modelValue', props.value)" class="peer hidden appearance-none w-[5px] h-[5px] cursor-pointer checked:text-9xl" />
    <span class="peer-checked:text-[var(--primary-color)]">{{ props.text}}</span>
  </label>
</template>
