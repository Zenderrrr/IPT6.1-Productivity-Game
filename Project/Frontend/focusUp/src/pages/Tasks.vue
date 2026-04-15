<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import Categories from '@/components/ui/Categories.vue'
import { computed, onMounted, ref } from 'vue'
import TasksComponent from '@/components/ui/TasksComponent.vue'
import Tag from '@/components/ui/Tag.vue'
import { useTaskStore } from '@/stores/taskStore.ts'
import type { Task } from '@/types/task.ts'
import { useStatsStore } from '@/stores/statsStore.ts'
import type { Dashboard } from '@/types/dashboard.ts'
import type { Productivity } from '@/types/productivity.ts'
import PopUpCreateTask from '@/components/ui/PopUpCreateTask.vue'
import type { CreateTask } from '@/types/createTask.ts'
import { useCategoryStore } from '@/stores/categoryStore.ts'
import type { Category } from '@/types/category.ts'
import CreateCategory from '@/components/ui/CreateCategory.vue'
import DeleteTask from '@/components/ui/DeleteTask.vue'

// categories logic
const whichIsActive = ref<number>(0)
function changeActiveCategory(value: number) {
  whichIsActive.value = value
}

// task logic
const taskStore = useTaskStore()
const taskData = ref<Task[] | null>(null)
const filteredTaskData = computed(() => {
  if(taskData.value === null) {
    return []
  }

  const mappedTask = taskData.value.map(t => ({
    ...t,
    category: t.categoryId ? getCategoryById(t.categoryId) : null,
  }))

  if(viewOption.value === 1){
    return mappedTask
  } else if(viewOption.value === 2){
    return mappedTask?.filter(t => t.status !== 3)
  }else if(viewOption.value === 3){
    return mappedTask?.filter(t => t.status === 3)
  }

  return mappedTask
})

// get category to task
function getCategoryById(id: number) {
  return categoryData.value?.find(t => t.id === id) ?? null
}

const error = ref<string | null>(null)

// productivity logic
const statsStore = useStatsStore()
const prodData = ref<Productivity[] | null>(null)

const taskDoneToday = computed(() => {
  return prodData.value?.reduce((n, { completedTasks }) => n + completedTasks, 0) ?? 0
})

const xpEarnedToday = computed(() => {
  return prodData.value?.reduce((n, { xpGained }) => n + xpGained, 0) ?? 0
})

// dashboard logic
const dashboardData = ref<Dashboard | null>(null)

const levelProgress = computed(() => {
  return (dashboardData.value?.progressToNextLevel ?? 0) * 100
})

// category logic
const categoryStore = useCategoryStore()
const categoryData = ref<Category[] | null>(null)

onMounted(async () => {
  try {
    await taskStore.getAllTasks()
    taskData.value = taskStore.allTasksData

    await statsStore.productivity(1)
    prodData.value = statsStore.productivityData

    await statsStore.dashboard('1')
    dashboardData.value = statsStore.dashboardData

    await categoryStore.getAllCategories()
    categoryData.value = categoryStore.categoriesData
  } catch (e) {
    error.value = e ? e.message : 'Failed to fetch task data'
  }
})

// which view option
const viewOption = ref<number>(1);
function changeViewOption(id: number) {
  viewOption.value = id
}

// show pop-up task
const showPopUpTask = ref<boolean>(false)

async function submitTask(task: CreateTask) {
  taskStore.error = null
  try{
    await taskStore.createTask(task)
  }catch(e){
    console.error(e)
  } finally {
    if(!taskStore.error)
      showPopUpTask.value = false
  }
}

// show pop-up category
const showPopUpCategory = ref<boolean>(false)

async function submitCategory(category: Category) : Promise<void> {
  try{
    const categoryId = await categoryStore.createCategory(category)

    if(categoryId !== null){
      showPopUpCategory.value = false
      await categoryStore.getAllCategories()
    }
  }catch(e){
    console.error(e)
  }
}

// show delete pop-up
const showPopUpDelete = ref<boolean>(false)
const deleteTaskId = ref<number | null>(null)

function showDeleteTask(taskId: number) {
  showPopUpDelete.value = true
  deleteTaskId.value = taskId
}

async function updateTask(task: Task) {

}

const errorTask = ref<string | null>(null)
async function submitDeleteTask(taskId: number) : Promise<void> {
  errorTask.value = null
  try{
    await taskStore.deleteTask(taskId)
  }catch{
    errorTask.value = taskStore.error
  } finally {
    if(!errorTask.value){
      showPopUpDelete.value = false
    }
  }
}
</script>

<template>
  <DeleteTask @cancel="showPopUpDelete = false" @confirm="submitDeleteTask" :task-id="deleteTaskId" :is-shown="showPopUpDelete"></DeleteTask>
  <CreateCategory :is-shown="showPopUpCategory" @cancel="showPopUpCategory = false" @submit="submitCategory"></CreateCategory>
  <PopUpCreateTask :is-shown="showPopUpTask" @cancel="showPopUpTask = false" @submit="submitTask"></PopUpCreateTask>

  <div class="h-screen flex flex-col overflow-hidden min-h-0">
    <NavAuth name-initials="SS"></NavAuth>
    <main class="xl:w-[80rem] flex-1 flex flex-col min-h-0">
      <GreetingsSection
        title="Meine Tasks"
        subtitle="Verwalte deine Aufgaben und bleib fokussiert!"
      ></GreetingsSection>

      <div class="grid grid-cols-8 gap-4 flex-1 min-h-0">
        <section class="col-span-6 flex flex-col overflow-auto">
          <!-- search area-->
          <div class="base-element grid grid-cols-[6fr_auto_auto] gap-2">
            <div
              class="flex items-center justify-start gap-2 bg-[var(--background-color)] px-4 py-2 rounded-lg"
            >
              <i class="fa-solid fa-magnifying-glass text-[var(--text-color-light)]"></i>
              <input class="w-full outline-0" type="text" placeholder="Tasks suchen ..." />
            </div>

            <button
              class="border border-gray-200 cursor-pointer flex items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--background-color)] px-4 py-2)]"
            >
              <i class="fa-solid fa-layer-group"></i>
              <span>nach Datum</span>
            </button>

            <button
              class="border border-gray-200 cursor-pointer flex items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--background-color)] px-4 py-2)]"
            >
              <i class="fa-solid fa-filter"></i>
              <span>Filter</span>
            </button>
          </div>

          <!-- Categories-->
          <div class="flex items-center justify-start mt-4 gap-2">
            <Categories text="Alle Kategorien" :is-active="whichIsActive === 0" @clicked="changeActiveCategory(0)"></Categories>
            <Categories
              v-for="category in categoryData"
              :key="category.id"
              :text="category.name"
              :isActive="whichIsActive === category.id"
              @clicked="changeActiveCategory(category.id)"
            ></Categories>
            <div @click="showPopUpCategory = true" class="shadow-lg bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] border-[var(--primary-color)] cursor-pointer text-center px-3 py-2 text-sm border text-nowrap rounded-full inline">
              <span>Erstelle Kategorie</span>
            </div>
          </div>

          <!-- View choosing-->
          <div
            class="flex items-center justify-evenly mt-4 gap-2 bg-[var(--surface-color)] shadow-lg rounded-xl p-2 text-sm"
          >
            <div
              @click="changeViewOption(1)"
              :class="viewOption === 1 ? 'activeView' : 'inactiveView' "
              class="cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-1"
            >
              <span class="">Alle</span>
              <span
                class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                >{{ (dashboardData?.tasksDone ?? 0) + (dashboardData?.tasksOpen ?? 0) }}</span
              >
            </div>
            <div class="cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-1" @click="changeViewOption(2)" :class="viewOption === 2 ? 'activeView' : 'inactiveView' ">
              <span class="">Offen</span>
              <span
                class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                >{{ dashboardData?.tasksOpen }}</span
              >
            </div>
            <div class="cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-1" @click="changeViewOption(3)" :class="viewOption === 3 ? 'activeView' : 'inactiveView' ">
              <span class="">Erledigt</span>
              <span
                class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                >{{ dashboardData?.tasksDone }}</span
              >
            </div>
          </div>

          <!-- Tasks-->
          <div
            class="scrollbar flex flex-col items-center justify-start mt-4 pr-2 gap-3 flex-1 overflow-y-auto"
          >
            <TasksComponent
              v-for="task in filteredTaskData"
              :key="task.id"
              :task-title="task.title"
              :task-description="task.description"
              :timeMin="task.durationMin"
              :date="task.dueDate"
              :xp="task.xp"
              :completed="task.status === 3"
              :difficulty="task.difficulty"
              @update=""
              @delete="showDeleteTask(task.id)"
            >
              <Tag v-if="task.category !== null" :name="task.category.name" :color-hex="task.category.color" text-color-hex="#FFFFFF"></Tag>
            </TasksComponent>
          </div>
        </section>

        <section class="col-span-2">
          <div
            class="flex items-center justify-center w-full base-element border-1 border-gray-200"
          >
            <button
              @click="showPopUpTask = true"
              class="cursor-pointer flex justify-center items-center gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] w-full px-4 py-2 rounded-lg text-[var(--text-color-white)] text-nowrap font-semibold border border-gray-200 text-md"
            >
              <i class="fa-solid fa-plus"></i>
              <span>Neue Task erstellen</span>
            </button>
          </div>

          <!-- To next level-->
          <div class="base-element mt-4 text-sm text-[var(--text-color-light)]">
            <span class="uppercase font-semibold">Bis zum nächsten Level</span>
            <div class="flex items-center justify-between w-full mt-4 mb-2">
              <span>Lv. {{ dashboardData?.level }} - Macher</span>
              <span class="text-[var(--text-color)] font-semibold"
                >{{ dashboardData?.xpCurrent }} / {{ dashboardData?.xpNext }} XP</span
              >
            </div>
            <div class="w-full h-1.5 bg-gray-200 rounded-full overflow-hidden">
              <div
                :style="{ width: `${levelProgress}%` }"
                class="h-full bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] rounded-full"
              ></div>
            </div>
          </div>

          <!-- Task Details-->
          <div class="base-element mt-4">
            <span class="uppercase text-[var(--text-color-light)] text-sm font-semibold"
              >Task Details</span
            >
            <div
              class="h-[150px] mt-2 flex justify-center items-center w-full rounded-lg text-[var(--text-color-light)]"
            >
              <div
                class="flex flex-col justify-center items-center text-center gap-3 h-full w-1/3 text-[var(--text-color-light)]"
              >
                <i class="fa-solid fa-list text-2xl"></i>
                <span class="text-xs leading-5">Task auswählen für Details</span>
              </div>
            </div>
          </div>

          <!-- Today Insights-->
          <div class="base-element mt-4">
            <span class="uppercase text-[var(--text-color-light)] text-sm font-semibold"
              >Heute</span
            >
            <div class="flex items-center justify-center w-full gap-2 mt-2">
              <div
                class="px-2 py-2 flex flex-col justify-center items-center w-full rounded-lg text-[var(--text-color-light)] bg-gray-100"
              >
                <span class="text-[var(--accent-color)] text-2xl font-semibold">{{
                  taskDoneToday
                }}</span>
                <span class="text-xs">Erledigt</span>
              </div>
              <div
                class="px-2 py-2 flex flex-col justify-center items-center w-full rounded-lg text-[var(--text-color-light)] bg-gray-100"
              >
                <span class="text-[var(--primary-color)] text-2xl font-semibold">{{ xpEarnedToday }}</span>
                <span class="text-xs">XP verdient</span>
              </div>
            </div>
          </div>

          <button class="flex items-center justify-center gap-2 uppercase border border-b-gray-200 font-semibold text-sm cursor-pointer base-element mt-4 w-full text-[var(--text-color-white)] !bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]">
            <div class="flex items-center justify-center w-[25px] h-[25px] text-xl">
              <i class="fa-solid fa-clipboard-check"></i>
            </div>
            <span>Tasks abschliessen</span>
          </button>
        </section>
      </div>
    </main>
  </div>
</template>

<style scoped>
.scrollbar {
  overflow-y: scroll;
  scroll-behavior: smooth;
  scrollbar-width: thin;
}

.activeView {
  background-color: var(--primary-color);
  color: var(--text-color-white);
}

.inactiveView {
  background-color: var(--surface-color);
  color: var(--text-color);
}
</style>
