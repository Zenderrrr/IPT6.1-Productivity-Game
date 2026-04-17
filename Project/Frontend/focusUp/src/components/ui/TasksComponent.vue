<script lang="ts" setup>
import Tag from '@/components/ui/Tag.vue'
import { formatTime } from '@/utils/date.ts'
import { computed, ref } from 'vue'
import { updateTaskApi } from '@/api/task.api.ts'

const props = defineProps<{
  taskTitle: string
  taskDescription: string
  timeMin: number
  xp: number
  date?: Date
  completed: boolean,
  difficulty: number,
  isChecked: boolean,
  isCompleted: boolean,
}>()

const emit = defineEmits<{
  (e: 'delete') : void
  (e: 'update') : void
  (e: 'checked') : void
}>()

function deleteTask() {
  emit('delete')
}

function updateTask() {
  emit('update')
}

const month = computed(() => {
  if(props.date == null) {
    return
  }
  return Intl.DateTimeFormat('de-CH', { month: 'short' }).format(props.date)
})
// task finished logic
const isChecked = ref<boolean>(props.completed || props.isChecked)

function changeChecked() {
  isChecked.value = !isChecked.value
  emit('checked')
}
</script>

<template>
  <div
    :class="isChecked ? 'checked' : 'unchecked'"
    class="shrink-0 group hover:!border-[var(--primary-color)] transition-all duration-100 overflow-hidden relative border-1 border-transparent cursor-pointer base-element w-full flex items-center justify-start gap-4"
  >
    <div class="h-full w-0.5 group-hover:bg-[var(--primary-color)] transition-all duration-100 bg-transparent absolute top-0 left-0"></div>

    <label class="container -translate-y-2.5">
      <input @click="changeChecked" type="checkbox" :checked="isChecked" :disabled="props.isCompleted" />
      <span class="checkmark rounded-2xl" :style="{ backgroundColor: props.isCompleted ? 'oklch(87.2% 0.01 258.338)' : '' }"></span>
    </label>

    <div class="flex items-center justify-between w-full">
      <div class="flex flex-col items-start justify-center">
        <span class="title font-semibold">{{ props.taskTitle }}</span>
        <span class="text-[var(--text-color-light)] text-sm">{{ props.taskDescription }}</span>
        <div class="mt-1 flex items-center justify-start gap-2">
          <!-- Tags-->
          <slot></slot>
          <Tag v-if="difficulty === 1 && !props.isCompleted && !props.isChecked" name="Einfach" color-hex="#f7fee7" text-color-hex="#00c951" border-color-hex="#00c951"></Tag>
          <Tag v-if="difficulty === 2 && !props.isCompleted && !props.isChecked" name="Mittel" color-hex="#fefce8" text-color-hex="#efb100" border-color-hex="#efb100"></Tag>
          <Tag v-if="difficulty === 3 && !props.isCompleted && !props.isChecked" name="Schwer" color-hex="#fef2f2" text-color-hex="#fb2c36" border-color-hex="#fb2c36"></Tag>

          <Tag v-if="difficulty === 1 && (props.isCompleted || props.isChecked)" name="Einfach" color-hex="#d1d5dc" text-color-hex="#FFFFFF"></Tag>
          <Tag v-if="difficulty === 2 && (props.isCompleted || props.isChecked)" name="Mittel" color-hex="#d1d5dc" text-color-hex="#FFFFFF"></Tag>
          <Tag v-if="difficulty === 3 && (props.isCompleted || props.isChecked)" name="Schwer" color-hex="#d1d5dc" text-color-hex="#FFFFFF"></Tag>

          <!-- Time-->
          <div
            class="flex gap-1 items-center justify-center rounded-lg px-2 py-1 text-xs font-semibold bg-gray-100 text-[var(--text-color-light)]"
          >
            <i class="fa-regular fa-clock"></i>
            <span>{{ props.timeMin }} Min.</span>
          </div>

          <!-- Date-->
          <div
            v-if="props.date !== null && props.date !== undefined"
            class="flex gap-1 items-center justify-center rounded-lg px-2 py-1 text-xs font-semibold bg-gray-100 text-[var(--text-color-light)]"
          >
            <i class="fa-regular fa-calendar"></i>
            <span>{{ `${formatTime(props.date.getDate())}. ${month}.` }}</span>
          </div>

          <!-- XP-->
          <div
            class="flex gap-1 items-center justify-center rounded-lg px-2 py-1 text-xs font-semibold bg-gray-100 text-[var(--text-color-light)]"
          >
            <span class="text-[var(--primary-color)]">+ {{ props.xp }} XP</span>
          </div>
        </div>
      </div>
      <div v-if="!props.isCompleted" class="group group-hover:opacity-100 opacity-0 flex duration-200 transition items-center justify-center gap-2">
        <button @click="updateTask" class="hover:bg-gray-100 transition duration-200 cursor-pointer flex items-center justify-center text-[var(--text-color)] border-gray-500 border w-[35px] h-[35px] rounded-lg">
          <i class="fa-solid fa-pencil"></i>
        </button>
        <button @click="deleteTask" class="hover:border-red-500 hover:text-red-500 transition duration-200 border-1 border-transparent cursor-pointer flex items-center justify-center text-red-400 bg-red-100 w-[35px] h-[35px] rounded-lg">
          <i class="fa-solid fa-trash-can"></i>
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.checked {
  background-color: oklch(96.7% 0.003 264.542);
}

.checked .title {
  text-decoration: line-through;
  color: var(--text-color-light);
}

.unchecked {
  text-decoration: none;
}

/* Customize the label (the container) */
.container {
  display: inline;
  position: relative;
  padding-left: 35px;
  margin: 0;
  width: 20px;
  cursor: pointer;
  font-size: 22px;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;
}

/* Hide the browser's default checkbox */
.container input {
  position: absolute;
  opacity: 0;
  cursor: pointer;
  height: 0;
  width: 0;
}

/* Create a custom checkbox */
.checkmark {
  position: absolute;
  top: 0;
  left: 0;
  height: 25px;
  width: 25px;
  background-color: transparent;
  border: 2px solid var(--text-color-light);
}

/* On mouse-over, add a grey background color */
.container:hover input ~ .checkmark {
  //background-color: #ccc;
}

/* When the checkbox is checked, add a blue background */
.container input:checked ~ .checkmark {
  background-color: var(--accent-color);
}

/* Create the checkmark/indicator (hidden when not checked) */
.checkmark:after {
  content: '';
  position: absolute;
  display: none;
}

/* Show the checkmark when checked */
.container input:checked ~ .checkmark:after {
  display: block;
}

.container input:checked ~ .checkmark {
  border: none;
}

/* Style the checkmark/indicator */
.container .checkmark:after {
  left: 9px;
  top: 5px;
  width: 5px;
  height: 10px;
  border: solid white;
  border-width: 0 3px 3px 0;
  -webkit-transform: rotate(45deg);
  -ms-transform: rotate(45deg);
  transform: rotate(45deg);
}
</style>
