<script lang="ts" setup>
import PopUpWindow from '@/components/ui/PopUpWindow.vue'
import DefaultButton from '@/components/ui/DefaultButton.vue'
import { reactive, ref, watch } from 'vue'
import type { Task } from '@/types/task.ts'
import type { Category } from '@/types/category.ts'
import { status } from '@/utils/status.ts'
import { difficulty } from '@/utils/difficulty.ts'

const props = defineProps<{
  isShown: boolean
  tasks: Task[]
  categories: Category[]
}>()

const emit = defineEmits<{
  (e: 'cancel'): void
  (e: 'submit', task: Task[]): void
}>()

function onCancel() {
  emit('cancel')
}

const isExtended = ref<boolean>(false)

const isActive = ref<string>('')

const selectedStatus = ref<string | null>(null)
const selectedDifficulty = ref<string | null>(null)
const selectedCategory = ref<string | null>(null)
const selectedLength = ref<string | null>(null)
const selectedDueDate = ref<string | null>(null)
const selectedFromDate = ref<string | null>(null)
const selectedToDate = ref<string | null>(null)

const filter = reactive({
  status: selectedStatus,
  difficulty: selectedDifficulty,
  category: selectedCategory,
  length: selectedLength,
  dueDate: selectedDueDate,
  fromDate: selectedFromDate,
  toDate: selectedToDate,
})

watch(filter, (newValue, oldValue) => {
  isActive.value = ''
})

function onSubmit() : void {
  let filteredTasks: Task[] = props.tasks

  if(isActive.value !== ''){

    if(isActive.value === 'today'){
      filteredTasks = filteredTasks.filter(t => {
        return t.dueDate === new Date()
      })
    }

    else if(isActive.value === 'highXp'){
      filteredTasks = filteredTasks.filter(t => {
        return t.xp >= 100
      })
    }

    else if(isActive.value === 'lowXp'){
      filteredTasks = filteredTasks.filter(t => {
        return t.xp <= 30
      })
    }

    else if(isActive.value === 'shortTasks'){
      filteredTasks = filteredTasks.filter(t => {
        return t.durationMin <= 15
      })
    }

    emit('submit', filteredTasks)
    return
  }

  if (selectedStatus.value !== null) {
    filteredTasks = filteredTasks.filter((t) => status[t.status] === selectedStatus.value)
  }
  if (selectedDifficulty.value !== null) {
    filteredTasks = filteredTasks.filter(
      (t) => difficulty[t.difficulty]?.toLowerCase() === selectedDifficulty.value,
    )
  }

  if (selectedCategory.value !== null) {
    filteredTasks = filteredTasks.filter((t) => t.categoryId === Number(selectedCategory.value))
  }

  if (selectedLength.value !== null) {
    if (selectedLength.value === '<30')
      filteredTasks = filteredTasks.filter((t) => t.durationMin < 30)
    if (selectedLength.value === '30-60')
      filteredTasks = filteredTasks.filter((t) => t.durationMin >= 30 && t.durationMin <= 60)
    if (selectedLength.value === '>60')
      filteredTasks = filteredTasks.filter((t) => t.durationMin > 60)
  }

  if (selectedDueDate.value !== null) {
    if(selectedDueDate.value === 'overdue')
      filteredTasks = filteredTasks.filter((t) => {
        if(t.dueDate === null || t.dueDate === undefined) return false
        return t.dueDate <= new Date() && t.completedAt === undefined
      })

    if(selectedDueDate.value === 'today'){
      filteredTasks = filteredTasks.filter((t) => {
        if(t.dueDate === null || t.dueDate === undefined) return false
        return t.dueDate === new Date() && t.completedAt === undefined
      })
    }

    if(selectedDueDate.value === 'week'){
      filteredTasks = filteredTasks.filter((t) => {
        if(t.dueDate === null || t.dueDate === undefined) return false
        return t.dueDate >= new Date() && t.completedAt === undefined
      })
    }
  }

  if (selectedFromDate.value !== null && selectedToDate.value !== null) {
    const fromDate = new Date(selectedFromDate.value)
    const toDate = new Date(selectedToDate.value)

    filteredTasks = filteredTasks.filter((t) => {
      if (t.dueDate === null || t.dueDate === undefined) return false
      return t.dueDate >= fromDate && t.dueDate <= toDate
    })
  }

  emit('submit', filteredTasks)
}

function resetFilters(){
  selectedStatus.value = null
  selectedDifficulty.value = null
  selectedCategory.value = null
  selectedLength.value = null
  selectedDueDate.value = null
  selectedFromDate.value = null
  selectedToDate.value = null
}
</script>

<template>
  <PopUpWindow :style="props.isShown ? '' : 'display: none'" @cancel="onCancel" title="Filter">
    <div class="h-[1px] w-full bg-gray-200 my-3"></div>
    <div>
      <span class="subtitle">Schnellfilter</span>
      <div class="flex items-center justify-start gap-2 mt-2">
        <div
          @click="
            isActive = 'today';
            resetFilters()
          "
          :class="isActive === 'today' ? 'active-quick-filter' : ''"
          class="hover:bg-gray-100 duration-75 transition flex items-center justify-center gap-2 border border-gray-200 rounded-full px-4 py-2 text-sm cursor-pointer"
        >
          <div class="flex items-center justify-center">
            <i class="fa-regular fa-calendar"></i>
          </div>
          <button class="cursor-pointer">Heute</button>
        </div>

        <div
          @click="
            isActive = 'lowXp';
            resetFilters()
          "
          :class="isActive === 'lowXp' ? 'active-quick-filter' : ''"
          class="hover:bg-gray-100 duration-75 transition flex items-center justify-center gap-2 border border-gray-200 rounded-full px-4 py-2 text-sm cursor-pointer"
        >
          <div class="flex items-center justify-center">
            <i class="fa-solid fa-battery-quarter"></i>
          </div>
          <button class="cursor-pointer">Wenig XP</button>
        </div>

        <div
          @click="
            isActive = 'highXp';
            resetFilters()
          "
          :class="isActive === 'highXp' ? 'active-quick-filter' : ''"
          class="hover:bg-gray-100 duration-75 transition flex items-center justify-center gap-2 border border-gray-200 rounded-full px-4 py-2 text-sm cursor-pointer"
        >
          <div class="flex items-center justify-center">
            <i class="fa-regular fa-star"></i>
          </div>
          <button class="cursor-pointer">Hohes XP</button>
        </div>

        <div
          @click="
            isActive = 'shortTasks';
            resetFilters()
          "
          :class="isActive === 'shortTasks' ? 'active-quick-filter' : ''"
          class="hover:bg-gray-100 duration-75 transition flex items-center justify-center gap-2 border border-gray-200 rounded-full px-4 py-2 text-sm cursor-pointer"
        >
          <div class="flex items-center justify-center">
            <i class="fa-solid fa-exclamation"></i>
          </div>
          <button class="cursor-pointer">Kurze Aufgaben</button>
        </div>
      </div>
    </div>

    <div class="h-[1px] w-full bg-gray-200 my-4"></div>

    <div class="mt-3">
      <button
        @click="
          isExtended = !isExtended"
        class="flex items-center justify-between gap-2 w-full cursor-pointer hover:bg-gray-100 duration-75 transition rounded-lg py-2"
      >
        <span class="subtitle">Erweiterter Filter</span>
        <i v-if="!isExtended" class="fa-solid fa-angle-down"></i>
        <i v-if="isExtended" class="fa-solid fa-angle-up"></i>
      </button>

      <div :style="isExtended ? '' : 'display: none'">
        <div class="mt-3 flex items-center justify-between gap-5">
          <div>
            <span class="subtitle">Status</span>
            <div class="flex items-center justify-center gap-2 mt-2">
              <DefaultButton
                v-model="selectedStatus"
                name="status"
                text="Offen"
                value="Open"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedStatus"
                name="status"
                text="In Bearbeitung"
                value="InProgress"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedStatus"
                name="status"
                text="Erledigt"
                value="Completed"
              ></DefaultButton>
            </div>
          </div>

          <div>
            <span class="subtitle">Schwierigkeit</span>
            <div class="flex items-center justify-center gap-2 mt-2">
              <DefaultButton
                v-model="selectedDifficulty"
                value="easy"
                name="diff"
                text="Einfach"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedDifficulty"
                value="middle"
                name="diff"
                text="Mittel"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedDifficulty"
                value="hard"
                name="diff"
                text="Schwer"
              ></DefaultButton>
            </div>
          </div>
        </div>

        <div class="mt-3">
          <span class="subtitle">Kategorie</span>
          <div class="flex items-center justify-start gap-2 mt-2">
            <DefaultButton
              v-for="category in props.categories"
              :key="category.id"
              v-model="selectedCategory"
              :value="String(category.id)"
              name="category"
              :text="category.name"
            ></DefaultButton>
          </div>
        </div>

        <div class="mt-3 flex items-center justify-between gap-5">
          <div>
            <span class="subtitle">Dauer</span>
            <div class="flex items-center justify-center gap-2 mt-2">
              <DefaultButton
                v-model="selectedLength"
                value="<30"
                name="time"
                text="<30 Min"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedLength"
                value="30-60"
                name="time"
                text="30 - 60 Min"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedLength"
                value=">60"
                name="time"
                text=">60 Min"
              ></DefaultButton>
            </div>
          </div>

          <div>
            <span class="subtitle">Fälligkeit</span>
            <div class="flex items-center justify-center gap-2 mt-2">
              <DefaultButton
                v-model="selectedDueDate"
                value="overdue"
                name="dueTo"
                text="Überfällig"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedDueDate"
                value="today"
                name="dueTo"
                text="Heute"
              ></DefaultButton>
              <DefaultButton
                v-model="selectedDueDate"
                value="week"
                name="dueTo"
                text="Diese Woche"
              ></DefaultButton>
            </div>
          </div>
        </div>

        <div class="mt-3">
          <span class="subtitle">Zeitraum</span>
          <div class="flex items-center justify-center gap-2 mt-2">
            <div
              class="w-full cursor-pointer flex items-center justify-center gap-2 mt-2 px-4 py-2 text-sm rounded-lg border border-gray-200"
            >
              <input
                v-model="selectedFromDate"
                class="outline-none cursor-pointer w-full"
                type="date"
              />
            </div>

            <div
              class="h-full flex items-center justify-center text-sm text-[var(--text-color-light)]"
            >
              <i class="fa-solid fa-arrow-right"></i>
            </div>

            <div
              class="w-full cursor-pointer flex items-center justify-center gap-2 mt-2 px-4 py-2 text-sm rounded-lg border border-gray-200"
            >
              <input
                v-model="selectedToDate"
                class="outline-none cursor-pointer w-full"
                type="date"
              />
            </div>
          </div>
        </div>
      </div>

      <div class="w-full h-[1px] bg-gray-200 rounded-full my-5"></div>

      <div class="flex items-center justify-end gap-5">

        <div class="flex items-center justify-center gap-1">
          <button
            @click="resetFilters"
            class="hover:bg-red-100 hover:border-red-500 hover:text-red-500 transition duration-75 cursor-pointer px-2 py-1 text-[var(--text-color-light)] border border-gray-200 rounded-lg text-xs"
          >
            Alles zurücksetzen
          </button>

          <button
            @click="onSubmit"
            class="hover:bg-[var(--primary-color-light)] hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] transition duration-75 cursor-pointer px-2 py-1 text-[var(--text-color-light)] border border-gray-200 rounded-lg text-xs"
          >
            Anwenden
          </button>
        </div>
      </div>
    </div>
  </PopUpWindow>
</template>

<style scoped>
.subtitle {
  text-transform: uppercase;
  color: var(--text-color-light);
  font-size: var(--text-sm);
  line-height: calc(1.25 / 0.875);
}

.active-quick-filter {
  background: var(--primary-color-light);
  border-color: var(--primary-color);
  color: var(--primary-color);
}
</style>
