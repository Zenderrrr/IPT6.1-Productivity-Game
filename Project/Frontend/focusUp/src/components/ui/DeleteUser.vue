<script lang="ts" setup>
import PopUpWindow from '@/components/ui/PopUpWindow.vue'

const props = defineProps<{
  isShown: boolean,
}>()

const emit = defineEmits<{
  (e: 'cancel'): void
  (e: 'confirm'): void
}>()

function cancel() {
  emit('cancel')
}

function confirm() {
  emit('confirm')
}
</script>

<template>
  <PopUpWindow :style="props.isShown ? '' : 'display: none' " @cancel="cancel" title="Willst du deinen Account wirklich löschen?">
    <div class="w-full flex justify-center items-center my-6">
      <button @click="confirm" class="scale-animation-sm hover:bg-transparent transition duration-100 cursor-pointer px-4 py-2 bg-red-100 border border-red-500 text-red-500 rounded-lg">Endgültig Löschen</button>
    </div>
  </PopUpWindow>
</template>
