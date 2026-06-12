<script lang="ts" setup>
import type { CreateTaskType } from '@/types/createTaskType.ts'
import { computed, onMounted, ref } from 'vue'
import { useCategoryStore } from '@/stores/categoryStore.ts'
import type { Category } from '@/types/category.ts'
import PopUpWindow from '@/components/ui/PopUpWindow.vue'
import createTask from '@/components/ui/CreateTask.vue'

const props = defineProps<{
  isShown: boolean
  onSubmit: (task: CreateTaskType) => Promise<boolean>
}>()

const emit = defineEmits<{
  (e: 'cancel') : void
}>()

const title = ref<string>('')
const description = ref<string>('')
const difficulty = ref<number | null>(null)
const duration = ref<number | null>(null)
const categoryId = ref<number | null>(null)
const dueDate = ref<string | null>(null)

const error = ref<string | null>(null)
async function submit(){
  if(difficulty.value === null )
    return error.value = 'Schwierigkeit nicht festgelegt.'

  if(duration.value === null)
    return error.value = 'Dauer ist leer.'

  const diffRecord: Record<number, string> = {
    1: 'Easy',
    2: 'Medium',
    3: 'Hard',
  }

  const diff = diffRecord[difficulty.value]
  if(diff === null || diff === undefined)
    return

  const task : CreateTaskType = {
    title: title.value,
    description: description.value,
    difficulty: diff,
    durationMin: duration.value,
    categoryId: categoryId.value ?? undefined,
    dueDate: dueDate.value ?? undefined,
  }

  const success = await props.onSubmit(task)
  if(success){
    title.value = ''
    description.value = ''
    difficulty.value = null
    duration.value = null
    categoryId.value = null
    dueDate.value = undefined
  }
}

function cancel(){
  emit('cancel')
}

function setDifficulty(diff: number){
  difficulty.value = diff
}

const categoryStore = useCategoryStore()
const categoryData = computed(() => categoryStore.categoriesData)


onMounted(async () => {
    await categoryStore.getAllCategories()
})
</script>

<template>

  <PopUpWindow @cancel="cancel" :style="props.isShown ? '' : 'display: none' " title="Erstelle eine neue Task">
      <!-- Inputs-->
      <div class="mx-auto w-full max-w-[34rem]">
        <form @submit.prevent="submit" class="w-full flex flex-col gap-3 sm:gap-4 mt-3 sm:mt-5">

          <div class="flex items-start justify-center flex-col gap-1.5">
            <label class="text-sm font-semibold ml-0.5" for="title">Task Titel *</label>
            <input required v-model="title" class="input-hover w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)] text-sm sm:text-base" id="title" type="text" placeholder="Was musst du als nächstes machen?">
          </div>

          <div class="flex items-start justify-center flex-col gap-1.5">
            <label class="text-sm font-semibold ml-0.5" for="title">Beschreibung</label>
            <textarea v-model="description" class="input-hover w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)] h-[80px] sm:h-[100px] text-sm sm:text-base resize-none" id="title" placeholder="Füge details oder notizen hinzu ..."></textarea>
          </div>

          <div class="flex flex-col sm:flex-row items-start justify-center gap-3 w-full">

            <div class="flex items-start justify-center flex-col gap-1.5 w-full h-full">
              <span class="text-sm font-semibold ml-0.5">Schwierigkeit *</span>
              <div class="grid grid-cols-3 items-start justify-center gap-1.5 w-full">
                  <button type="button" @click="setDifficulty(1)" :class="difficulty === 1 ? 'active-easy' : 'inactive' " class=" input-hover w-full cursor-pointer px-2 sm:px-3 py-2 border border-gray-200 text-[var(--text-color-light)] text-xs sm:text-sm font-semibold rounded-lg">Easy</button>
                  <button type="button" @click="setDifficulty(2)" :class="difficulty === 2 ? 'active-medium' : 'inactive' " class="input-hover w-full cursor-pointer px-2 sm:px-3 py-2 border border-gray-200 text-[var(--text-color-light)] text-xs sm:text-sm font-semibold rounded-lg">Medium</button>
                  <button type="button" @click="setDifficulty(3)" :class="difficulty === 3 ? 'active-hard' : 'inactive' " class="input-hover w-full cursor-pointer px-2 sm:px-3 py-2 border border-gray-200 text-[var(--text-color-light)] text-xs sm:text-sm font-semibold rounded-lg">Hard</button>
              </div>
            </div>

            <div class="flex items-start justify-center flex-col gap-1.5 w-full">
              <label class="text-sm font-semibold ml-0.5" for="title">Dauer (min) *</label>
              <input required v-model="duration" class="input-hover w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)] text-sm sm:text-base" type="number" id="title" placeholder="30">
            </div>
          </div>

          <div class="flex flex-col sm:flex-row items-start justify-center gap-3 w-full">

            <div class="flex items-start justify-center flex-col gap-1.5 w-full">
              <label class="text-sm font-semibold ml-0.5" for="title">Kategorie</label>
              <select v-model="categoryId" class="input-hover w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)] text-sm sm:text-base" id="title">
                <option :value="null">Auswählen</option>
                <option v-for="category in categoryData" :key="category.id" :value="category.id">{{ category.name }}</option>
              </select>
            </div>

            <div class="flex items-start justify-center flex-col gap-1.5 w-full">
              <label class="text-sm font-semibold ml-0.5" for="title">Abschlussdatum</label>
              <input v-model="dueDate" class="input-hover cursor-pointer w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)] text-sm sm:text-base" type="date" id="title" placeholder="30">
            </div>
          </div>

          <!-- divider-->
          <div class="h-0.5 bg-gray-100 w-full"></div>

          <!-- submit & cancel-->
          <div class="w-full flex flex-col-reverse sm:flex-row justify-end items-stretch sm:items-center gap-2 sm:gap-3">
            <button type="button" @click="cancel" class="w-full sm:w-auto hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] transition duration-200 cursor-pointer px-4 py-2 bg-transparent border border-gray-200 rounded-lg text-[var(--text-color-light)] font-semibold text-sm sm:text-base">Cancel</button>
            <button type="submit" class="w-full sm:w-auto scale-animation-sm cursor-pointer px-4 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] border rounded-lg text-[var(--text-color-white)] font-semibold text-sm sm:text-base">Task erstellen</button>
          </div>
        </form>

        <span v-if="error" class="text-red-400 text-sm mt-2 block">{{ error }}</span>
      </div>
  </PopUpWindow>
</template>

<style scoped>
  input, textarea, select {
    padding: 9px 10px;
  }

  @media (min-width: 640px) {
    input, textarea, select {
      padding: 10px 10px;
    }
  }

  .active-easy{
    background-color: oklch(98.6% 0.031 120.757);
    border-color: oklch(72.3% 0.219 149.579);
    border-width: 1px;
    color: oklch(72.3% 0.219 149.579);
  }
  .active-medium{
    background-color: oklch(98.7% 0.026 102.212);
    border-color: oklch(79.5% 0.184 86.047);
    border-width: 1px;
    color: oklch(79.5% 0.184 86.047);
  }
  .active-hard{
    background-color: oklch(97.1% 0.013 17.38);
    border-color: oklch(63.7% 0.237 25.331);
    border-width: 1px;
    color: oklch(63.7% 0.237 25.331);
  }
</style>