<script lang="ts" setup>
import type { CreateTask } from '@/types/createTask.ts'
import { ref } from 'vue'

const props = defineProps<{
  isShown: boolean
}>()

const emit = defineEmits<{
  (e: 'submit', task: CreateTask) : void
  (e: 'cancel') : void
}>()

const title = ref<string>('')
const description = ref<string>('')
const difficulty = ref<number | null>(null)
const duration = ref<number | null>(null)
const categoryId = ref<number | null>(null)
const dueDate = ref<Date | null>(null)

function submit(){
  if(difficulty.value === null )
    return console.error('difficulty is null')

  if(duration.value === null)
    return console.error('duration is null')

  const diffRecord: Record<number, string> = {
    1: 'Easy',
    2: 'Medium',
    3: 'Hard',
  }

  const diff = diffRecord[difficulty.value]
  if(diff === null || diff === undefined)
    return

  const task : CreateTask = {
    title: title.value,
    description: description.value,
    difficulty: diff,
    durationMin: duration.value,
    categoryId: categoryId.value ?? undefined,
    dueDate: dueDate.value?.toISOString() ?? undefined,
  }

  emit('submit', task)
}

function cancel(){
  emit('cancel')
}

function setDifficulty(diff: number){
  difficulty.value = diff
}
</script>

<template>
  <div :style="props.isShown ? '' : 'display: none' " class="absolute w-[100vw] h-[100vh] flex justify-center items-center backdrop-blur-xs z-999">
    <div class="base-element">

      <!-- Title and close-->
      <div class="flex items-center justify-between w-[500px]">
        <span class="font-semibold text-xl">Erstelle eine neue Task</span>
        <button @click="cancel" class="flex justify-center items-center bg-gray-100 text-gray-400 w-[35px] h-[35px] rounded-lg text-sm cursor-pointer">
          <i class="fa-solid fa-x"></i>
        </button>
      </div>

      <!-- Inputs-->
      <form @submit.prevent="submit" class="w-full flex flex-col gap-5 mt-6">

        <div class="flex items-start justify-center flex-col gap-1.5">
          <label class="text-sm font-semibold ml-0.5" for="title">Task Titel *</label>
          <input required v-model="title" class="w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)]" id="title" type="text" placeholder="Was musst du als nächstes machen?">
        </div>

        <div class="flex items-start justify-center flex-col gap-1.5">
          <label class="text-sm font-semibold ml-0.5" for="title">Beschreibung</label>
          <textarea v-model="description" class="w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)] h-[100px]" id="title" placeholder="Füge details oder notizen hinzu ..."></textarea>
        </div>

        <div class="flex items-start justify-center gap-3 w-full">

          <div class="flex items-start justify-center flex-col gap-1.5 w-full h-full">
            <span class="text-sm font-semibold ml-0.5">Schwierigkeit *</span>
            <div class="flex items-start justify-center gap-1.5 w-full">
                <button @click="setDifficulty(1)" :class="difficulty === 1 ? 'active-easy' : 'inactive' " class="w-full cursor-pointer px-3 py-2 border border-gray-200 text-[var(--text-color-light)] text-sm font-semibold rounded-lg">Easy</button>
                <button @click="setDifficulty(2)" :class="difficulty === 2 ? 'active-medium' : 'inactive' " class="w-full cursor-pointer px-3 py-2 border border-gray-200 text-[var(--text-color-light)] text-sm font-semibold rounded-lg">Medium</button>
                <button @click="setDifficulty(3)" :class="difficulty === 3 ? 'active-hard' : 'inactive' " class="w-full cursor-pointer px-3 py-2 border border-gray-200 text-[var(--text-color-light)] text-sm font-semibold rounded-lg">Hard</button>
            </div>
          </div>

          <div class="flex items-start justify-center flex-col gap-1.5 w-full">
            <label class="text-sm font-semibold ml-0.5" for="title">Dauer (min) *</label>
            <input required v-model="duration" class="w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)]" type="number" id="title" placeholder="30">
          </div>
        </div>

        <div class="flex items-start justify-center gap-3 w-full">

          <div class="flex items-start justify-center flex-col gap-1.5 w-full">
            <label class="text-sm font-semibold ml-0.5" for="title">Kategorie</label>
            <select class="w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)]" id="title">
              <option value="">Auswählen</option>
              <option value="">Beispiel1</option>
              <option value="">Beispiel1</option>
              <option value="">Beispiel1</option>
              <option value="">Beispiel1</option>
              <option value="">Beispiel1</option>
            </select>
          </div>

          <div class="flex items-start justify-center flex-col gap-1.5 w-full">
            <label class="text-sm font-semibold ml-0.5" for="title">Abschlussdatum</label>
            <input class="w-full px-3 py-1 border border-gray-200 rounded-lg outline-[var(--primary-color)]" type="date" id="title" placeholder="30"></input>
          </div>
        </div>

        <!-- divider-->
        <div class="h-0.5 bg-gray-100 w-full"></div>

        <!-- submit & cancel-->
        <div class="w-full flex justify-end items-center gap-3">
          <button @click="cancel" class="cursor-pointer px-4 py-2 bg-transparent border border-gray-200 rounded-lg text-[var(--text-color-light)] font-semibold">Cancel</button>
          <button class="cursor-pointer px-4 py-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] border rounded-lg text-[var(--text-color-white)] font-semibold">Task erstellen</button>
        </div>
      </form>
    </div>
  </div>
</template>

<style scoped>
  input, textarea, select {
    padding: 10px 10px;
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
