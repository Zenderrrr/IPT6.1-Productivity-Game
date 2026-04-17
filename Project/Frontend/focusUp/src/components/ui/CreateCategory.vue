<script lang="ts" setup>
import PopUpWindow from '@/components/ui/PopUpWindow.vue'
import { ref } from 'vue'
import type { CreateCategory} from '@/types/createCategory.ts'

const props = defineProps<{
  isShown: boolean,
}>()

const emit = defineEmits<{
  (e: 'submit', category: CreateCategory): void
  (e: 'cancel'): void
}>()

function cancel() {
  emit('cancel')
}

const chosenColor = ref<string>()
const name = ref<string>()

const ownChosenColor = ref<string>()
function submit() {
  if (chosenColor.value == null) {
    return
  }

  if (name.value == null) {
    return
  }

  const category: CreateCategory = {
    name: name.value,
    color: chosenColor.value,
  }

  emit('submit', category)
}

const colors = [
  '#efb7b0',
  '#f1d39f',
  '#f0e17f',
  '#e9eb66',
  '#bff07a',
  '#9fe5be',
  '#8fdcc4',
  '#a7d8f0',
  '#96cbf0',
  '#8dbef0',
  '#b3b0f3',
  '#c4b0f0',
  '#d5acef',
  '#e6a3ea',
  '#e8a7c8',
  '#edb3b8',
]
</script>

<template>
  <PopUpWindow title="Erstelle eine neue Kategorie" @cancel="cancel" :style="props.isShown ? '' : 'display: none' ">

    <!-- Inputs-->
    <form @submit.prevent="submit" class="w-full flex flex-col gap-5 mt-6">
      <div class="flex items-start justify-center flex-col gap-1.5">
        <label class="text-sm font-semibold ml-0.5" for="title">Name *</label>
        <input
          required
          v-model="name"
          class="input-hover w-full px-4 py-3 border border-gray-200 rounded-lg outline-[var(--primary-color)]"
          id="title"
          type="text"
          placeholder="zb. Hobby, Arbeit, Fitness, Lernen ..."
        />
      </div>

      <div class="flex items-start justify-center flex-col gap-1.5">
        <label class="text-sm font-semibold ml-0.5" for="title">Farbe *</label>

        <div class="grid grid-cols-10 gap-1.5">
          <div
            @click="chosenColor = color"
            :class="chosenColor === color ? 'category-active' : ''"
            v-for="color in colors"
            :style="{ backgroundColor: color }"
            class="scale-animation-lg cursor-pointer border-2 border-transparent bg-[var(--primary-color)] rounded-lg w-[40px] h-[40px]"
          >
            <div
              v-if="chosenColor === color"
              class="w-full h-full flex justify-center items-center text-[var(--text-color-white)] text-sm"
            >
              <i class="fa-solid fa-check"></i>
            </div>
          </div>
        </div>
      </div>

      <div class="flex items-start justify-center flex-col gap-1.5">
        <label class="text-sm font-semibold ml-0.5" for="title">oder eigene Farbe</label>
        <input
          v-model="ownChosenColor"
          @change="chosenColor = ownChosenColor"
          class="input-hover h-[50px] cursor-pointer w-full px-4 py-3 border border-gray-200 rounded-lg outline-[var(--primary-color)]"
          id="title"
          type="color"
          placeholder="zb. Hobby, Arbeit, Fitness, Lernen ..."
        />
      </div>

      <div class="h-0.5 bg-gray-100 w-full"></div>

      <!-- submit & cancel-->
      <div class="w-full flex justify-end items-center gap-3">
        <button
          @click="cancel"
          class="hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] transition duration-200 cursor-pointer px-4 py-2 bg-transparent border border-gray-200 rounded-lg text-[var(--text-color-light)] font-semibold"
        >
          Cancel
        </button>
        <button
          class="scale-animation-sm cursor-pointer px-4 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] border rounded-lg text-[var(--text-color-white)] font-semibold"
        >
          Kategorie erstellen
        </button>
      </div>
    </form>
  </PopUpWindow>
</template>

<style scoped>
.category-active {
  border-color: oklch(55.1% 0.027 264.364);
  width: 105%;
  height: 105%;
}
</style>
