<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import Categories from '@/components/ui/Categories.vue'
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import TasksComponent from '@/components/ui/TasksComponent.vue'
import Tag from '@/components/ui/Tag.vue'
import { useTaskStore } from '@/stores/taskStore.ts'
import type { Task } from '@/types/task.ts'
import { useStatsStore } from '@/stores/statsStore.ts'
import type { Productivity } from '@/types/productivity.ts'
import CreateTask from '@/components/ui/CreateTask.vue'
import type { CreateTaskType } from '@/types/createTaskType.ts'
import { useCategoryStore } from '@/stores/categoryStore.ts'
import DeleteTask from '@/components/ui/DeleteTask.vue'
import type { UpdateTask } from '@/types/updateTask.ts'
import { useAuthStore } from '@/stores/authStore.ts'
import UpdateTaskComponent from '@/components/ui/UpdateTaskComponent.vue'
import TaskCompleteHandler from '@/components/ui/CompleteTaskComponents/TaskCompleteHandler.vue'
import type { TaskCompleteType } from '@/types/taskComplete.ts'
import Filter from '@/components/ui/Filter.vue'
import { storeToRefs } from 'pinia'
import PlaceholderTask from '@/components/ui/PlaceholderTask.vue'
import { applyUIMode } from '@/utils/modeUI.ts'
import type { CreateCategoryType } from '@/types/createCategoryType.ts'
import Loading from '@/components/ui/Loading.vue'
import { formatTime } from '@/utils/date.ts'
import CreateCategory from '@/components/ui/CreateCategory.vue'

// date
const date = new Date()
const day = new Intl.DateTimeFormat('de-CH', { weekday: 'long' }).format(date)
const month = new Intl.DateTimeFormat('de-CH', { month: 'long' }).format(date)

// window height
const windowHeight = ref<number>(0)
const SMALLWINDOWHEIGHT = 1000

const updateWindowHeight = () => {
  windowHeight.value = window.innerHeight
}

// categories logic
const whichIsActive = ref<number>(0)
function changeActiveCategory(value: number) {
  whichIsActive.value = value
}

// task logic
const taskStore = useTaskStore()
const taskData = computed(() => taskStore.allTasksData)

const taskFilter = ref<string>('')
const isFilterDate = ref<boolean>(false)
const filteredTaskData = computed(() => {
  if (taskData.value === null) {
    return []
  }

  let mappedTask = taskData.value.map((t) => ({
    ...t,
    category: t.categoryId ? getCategoryById(t.categoryId) : null,
  }))

  mappedTask = mappedTask.filter((t) => {
    return t.title.toLowerCase().includes(taskFilter.value.toLowerCase())
  })

  if (whichIsActive.value !== 0) {
    mappedTask = mappedTask.filter((t) => {
      return t.categoryId === whichIsActive.value
    })
  }

  if (submittedFilterTask.value) {
    const ids = new Set(submittedFilterTask.value.map((t) => t.id))
    mappedTask = mappedTask.filter((t) => {
      return ids.has(t.id)
    })
  }

  if (isFilterDate.value) {
    mappedTask = mappedTask.sort((a, b) => {
      if (!a.dueDate && !b.dueDate) return 0
      if (!a.dueDate) return 1
      if (!b.dueDate) return -1

      return a.dueDate.getTime() - b.dueDate.getTime()
    })
  }

  if (viewOption.value === 1) {
    return mappedTask
  } else if (viewOption.value === 2) {
    return mappedTask?.filter((t) => t.status !== 3)
  } else if (viewOption.value === 3) {
    return mappedTask?.filter((t) => t.status === 3)
  }

  return mappedTask
})

// get category to task
function getCategoryById(id: number) {
  return categoriesData.value?.find((t) => t.id === id) ?? null
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
const dashboardData = computed(() => statsStore.dashboardData)

const levelProgress = computed(() => {
  return (dashboardData.value?.progressToNextLevel ?? 0) * 100
})

// category logic
const categoryStore = useCategoryStore()
const { categoriesData } = storeToRefs(categoryStore)

// auth logic
const authStore = useAuthStore()

onMounted(async () => {
  applyUIMode()

  updateWindowHeight()
  window.addEventListener('resize', updateWindowHeight)

  try {
    getCheckedTasks()
    await taskStore.getAllTasks()
    await statsStore.productivity(1)
    prodData.value = statsStore.productivityData
    await statsStore.dashboard('1')
    await categoryStore.getAllCategories()
  } catch (e) {
    error.value = e instanceof Error ? e.message : 'Failed to fetch task data'
  }
})

onBeforeUnmount(() => {
  window.removeEventListener('resize', updateWindowHeight)
})

// which view option
const viewOption = ref<number>(2)
function changeViewOption(id: number) {
  viewOption.value = id
}

// show pop-up task
const showPopUpTask = ref<boolean>(false)

async function submitTask(task: CreateTaskType): Promise<boolean> {
  taskStore.error = null
  try {
    await taskStore.createTask(task)

    showPopUpTask.value = false
    return true
  } catch {
    return false
  }
}

// show pop-up category
const showPopUpCategory = ref<boolean>(false)

async function submitCategory(category: CreateCategoryType): Promise<void> {
  try {
    const categoryId = await categoryStore.createCategory(category)

    if (categoryId !== null) {
      showPopUpCategory.value = false
      await categoryStore.getAllCategories()
    }
  } catch (e) {
    console.error(e)
  }
}

// delete category
async function deleteCategory(id: number): Promise<void> {
  await categoryStore.deleteCategory(id)
}

// show delete pop-up
const showPopUpDelete = ref<boolean>(false)
const deleteTaskId = ref<number | null>(null)

function showDeleteTask(taskId: number) {
  showPopUpDelete.value = true
  deleteTaskId.value = taskId
}

const errorTask = ref<string | null>(null)
async function submitDeleteTask(taskId: number): Promise<void> {
  errorTask.value = null
  try {
    await taskStore.deleteTask(taskId)
  } catch {
    errorTask.value = taskStore.error
  } finally {
    if (!errorTask.value) {
      showPopUpDelete.value = false
    }
  }
}

// task checked logic
const checkedTasks = ref<number[]>([])
function changeCheckedTasks(taskId: number) {
  getCheckedTasks()

  if (checkedTasks.value.some((t) => t === taskId))
    checkedTasks.value = checkedTasks.value.filter((t) => t !== taskId)
  else checkedTasks.value.push(taskId)

  const string = JSON.stringify(checkedTasks.value)
  localStorage.setItem(`checkedTasks_${authStore.user?.id}`, string)
}

function getCheckedTasks() {
  const data = localStorage.getItem(`checkedTasks_${authStore.user?.id}`)
  checkedTasks.value = Array.isArray(JSON.parse(data ?? '[]')) ? JSON.parse(data ?? '[]') : []
}

function isTaskChecked(taskId: number) {
  return checkedTasks.value.some((t) => t === taskId)
}

const isTaskCompleteHandlerVisible = ref<boolean>(false)
const taskCompleteKey = ref<number>(0)
const tasksForCompleteHandler = ref<TaskCompleteType[] | undefined>(undefined)

async function completeTask() {
  getCheckedTasks()

  const taskComplete: TaskCompleteType[] = []

  for (const task of checkedTasks.value) {
    taskComplete.push((await taskStore.completeTask(task)) as TaskCompleteType)
  }

  localStorage.removeItem(`checkedTasks_${authStore.user?.id}`)
  checkedTasks.value = []

  tasksForCompleteHandler.value = taskComplete
  taskCompleteKey.value++
  isTaskCompleteHandlerVisible.value = true
}

async function closeTaskCompleteHandler() {
  isTaskCompleteHandlerVisible.value = false

  await taskStore.getAllTasks()

  await statsStore.productivity(1)
  prodData.value = statsStore.productivityData

  await statsStore.dashboard('1')
}

// show update task pop-up
const showPopUpUpdate = ref<boolean>(false)
const updateTask = ref<Task | null>(null)

function closeWindow() {
  showPopUpUpdate.value = false
  updateTask.value = null
}

function showUpdateTask(task: Task) {
  showPopUpUpdate.value = true
  updateTask.value = task
}

async function submitUpdateTask(taskId: number, task: UpdateTask) {
  try {
    await taskStore.updateTask(taskId, task)
  } catch (e) {
    console.error(e)
  } finally {
    if (taskStore.error === null) {
      showPopUpUpdate.value = false
    }
  }
}

const filterShown = ref<boolean>(false)
const submittedFilterTask = ref<Task[] | null>(null)
function submitFilter(tasks: Task[]) {
  filterShown.value = false

  if (submittedFilterTask.value !== null) {
    resetFilter()
  }
  submittedFilterTask.value = tasks
}

function resetFilter() {
  submittedFilterTask.value = null
}

// Category change small windows
function handleCategoryChange(event: Event) {
  const select = event.target as HTMLSelectElement

  if(select.value === 'new') {
    showPopUpCategory.value = true
    select.value = ''
  }
}
</script>

<template>
  <Filter
    :is-shown="filterShown"
    :tasks="taskStore.allTasksData ?? []"
    :categories="categoryStore.categoriesData ?? []"
    @cancel="filterShown = false"
    @submit="submitFilter"
  ></Filter>

  <TaskCompleteHandler
    :key="taskCompleteKey"
    v-if="isTaskCompleteHandlerVisible"
    @close="closeTaskCompleteHandler"
    :tasksCompleteArr="tasksForCompleteHandler ?? []"
  ></TaskCompleteHandler>

  <Transition name="popUp">
    <UpdateTaskComponent
      v-if="showPopUpUpdate && updateTask"
      :is-shown="showPopUpUpdate"
      :task="updateTask"
      @submit="submitUpdateTask"
      @cancel="closeWindow"
    ></UpdateTaskComponent>
  </Transition>
  <Transition name="popUp">
    <DeleteTask
      v-if="deleteTaskId"
      @cancel="showPopUpDelete = false"
      @confirm="submitDeleteTask"
      :task-id="deleteTaskId"
      :is-shown="showPopUpDelete"
    ></DeleteTask>
  </Transition>
  <CreateCategory
    :is-shown="showPopUpCategory"
    @cancel="showPopUpCategory = false"
    @submit="submitCategory"
  ></CreateCategory>
  <CreateTask
    :on-submit="submitTask"
    :is-shown="showPopUpTask"
    @cancel="showPopUpTask = false"
  ></CreateTask>

  <div class="min-h-screen lg:h-screen flex flex-col lg:overflow-hidden">
    <NavAuth></NavAuth>

    <Loading v-if="taskStore.loading"></Loading>

    <main
      v-if="!taskStore.loading"
      :style="windowHeight < SMALLWINDOWHEIGHT ? 'overflow-y:scroll' : 'overflow-y:hidden'"
      class="deactivate-scrollbar w-full max-w-[80rem] mx-auto flex-1 flex flex-col min-h-0 px-4 sm:px-6 xl:px-0 py-4 lg:overflow-x-hidden"
    >
      <div
        class="flex flex-col flex-1 min-h-0 lg:overflow-hidden"
        v-if="windowHeight > SMALLWINDOWHEIGHT"
      >
        <GreetingsSection
          class="shrink-0"
          title="Meine Tasks"
          subtitle="Verwalte deine Aufgaben und bleib fokussiert!"
        ></GreetingsSection>

        <div class="grid grid-cols-1 lg:grid-cols-8 gap-4 flex-1 min-h-0 lg:overflow-hidden">
          <section class="lg:col-span-6 flex flex-col min-h-0">
            <!-- search area-->
            <div class="base-element grid grid-cols-1 sm:grid-cols-[1fr_auto_auto] gap-2 shrink-0">
              <div
                class="searchbar input-hover-default flex items-center justify-start gap-2 bg-[var(--background-color)] px-4 py-2 rounded-lg"
              >
                <i class="fa-solid fa-magnifying-glass text-[var(--text-color-light)]"></i>
                <input
                  v-model="taskFilter"
                  class="w-full outline-0 bg-transparent"
                  type="text"
                  placeholder="Tasks suchen ..."
                />
              </div>

              <button
                @click="isFilterDate = !isFilterDate"
                :style="
                  isFilterDate
                    ? 'color:var(--primary-color); border-color:var(--primary-color)'
                    : ''
                "
                class="select-none hover:text-[var(--primary-color)] hover:border-[var(--primary-color)] transition duration-200 border border-[var(--border-color)] cursor-pointer flex items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--background-color)] px-4 py-2"
              >
                <i class="fa-solid fa-layer-group"></i>
                <span>nach Datum</span>
              </button>

              <button
                :style="
                  submittedFilterTask !== null
                    ? 'border-color:var(--primary-color); color:var(--primary-color)'
                    : ''
                "
                @click="submittedFilterTask !== null ? resetFilter() : (filterShown = true)"
                class="select-none hover:text-[var(--primary-color)] hover:border-[var(--primary-color)] transition duration-200 border border-[var(--border-color)] cursor-pointer flex items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--background-color)] px-4 py-2"
              >
                <i class="fa-solid fa-filter"></i>
                <span>Filter</span>
              </button>
            </div>

            <!-- Categories-->
            <div
              class="flex items-center justify-start mt-4 gap-2 min-h-0 shrink-0 overflow-x-auto pb-2"
            >
              <Categories
                :id="0"
                :can-be-remove="false"
                text="Alle Kategorien"
                :is-active="whichIsActive === 0"
                @clicked="changeActiveCategory(0)"
              ></Categories>

              <Categories
                v-for="category in categoriesData ?? []"
                :id="category.id"
                :key="category.id"
                :can-be-remove="true"
                :text="category.name"
                :isActive="whichIsActive === category.id"
                @clicked="changeActiveCategory(category.id)"
                @remove="deleteCategory"
              ></Categories>
              <div
                @click="showPopUpCategory = true"
                class="scale-animation-sm shadow-lg bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] border-[var(--primary-color)] cursor-pointer text-center px-3 py-2 text-sm border text-nowrap rounded-full inline shrink-0"
              >
                <span>Erstelle Kategorie</span>
              </div>
            </div>

            <!-- View choosing-->
            <div
              class="grid grid-cols-1 sm:grid-cols-3 mt-4 gap-2 bg-[var(--surface-color)] shadow-lg rounded-xl p-2 text-sm shrink-0"
            >
              <div
                @click="changeViewOption(1)"
                :class="viewOption === 1 ? 'activeView' : ''"
                class="hover:bg-[var(--hover-light-color)] duration-200 transition cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-2"
              >
                <span class="">Alle</span>
                <span
                  class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                  >{{ (dashboardData?.tasksDone ?? 0) + (dashboardData?.tasksOpen ?? 0) }}</span
                >
              </div>
              <div
                class="hover:bg-[var(--hover-light-color)] duration-200 transition bg-transparent cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-2"
                @click="changeViewOption(2)"
                :class="viewOption === 2 ? 'activeView' : ''"
              >
                <span class="">Offen</span>
                <span
                  class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                  >{{ dashboardData?.tasksOpen ?? 0 }}</span
                >
              </div>
              <div
                class="hover:bg-[var(--hover-light-color)] duration-200 transition cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-2"
                @click="changeViewOption(3)"
                :class="viewOption === 3 ? 'activeView' : ''"
              >
                <span class="">Erledigt</span>
                <span
                  class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                  >{{ dashboardData?.tasksDone ?? 0 }}</span
                >
              </div>
            </div>

            <!-- Tasks-->
            <div
              class="deactivate-scrollbar! flex flex-1 flex-col justify-start mt-4 pr-0 lg:pr-2 gap-3 overflow-y-auto overflow-x-hidden min-h-[450px] lg:min-h-0"
            >
              <TasksComponent
                v-if="!taskStore.loading"
                v-for="task in filteredTaskData"
                :key="task.id"
                :task-title="task.title"
                :task-description="task.description"
                :timeMin="task.durationMin"
                :date="task.dueDate"
                :xp="task.xp"
                :completed="task.status === 3"
                :difficulty="task.difficulty"
                :is-checked="isTaskChecked(task.id)"
                :is-completed="task.status === 3"
                @update="showUpdateTask(task)"
                @delete="showDeleteTask(task.id)"
                @checked="changeCheckedTasks(task.id)"
              >
                <Tag
                  v-if="task.category !== null && task.status !== 3 && !isTaskChecked(task.id)"
                  :name="task.category.name"
                  :color-hex="task.category.color"
                  text-color-hex="#FFFFFF"
                ></Tag>
                <Tag
                  v-if="task.category !== null && (task.status === 3 || isTaskChecked(task.id))"
                  :name="task.category.name"
                  color-hex="#d1d5dc"
                  text-color-hex="#FFFFFF"
                ></Tag>
              </TasksComponent>

              <div
                v-if="taskStore.loading || filteredTaskData.length === 0"
                class="flex flex-col items-center justify-center gap-3"
              >
                <PlaceholderTask v-for="i in 10" :key="i"></PlaceholderTask>
              </div>
            </div>
          </section>

          <section class="lg:col-span-2 pb-4 lg:pb-0">
            <div
              class="flex items-center justify-center w-full base-element border-1 border-gray-200"
            >
              <button
                @click="showPopUpTask = true"
                class="select-none scale-animation-sm cursor-pointer flex justify-center items-center gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] w-full px-4 py-2 rounded-lg text-[var(--text-color-white)] text-nowrap font-semibold border border-gray-200 text-md"
              >
                <i class="fa-solid fa-plus"></i>
                <span>Neue Task erstellen</span>
              </button>
            </div>

            <!-- To next level-->
            <div class="base-element mt-4 text-sm text-[var(--text-color-light)]">
              <span class="uppercase font-semibold">Bis zum nächsten Level</span>
              <div class="flex items-center justify-between w-full mt-4 mb-2 gap-2">
                <span>Lv. {{ dashboardData?.level }}</span>
                <span class="text-[var(--text-color)] font-semibold text-right"
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
            <!--          <div class="base-element mt-4">-->
            <!--            <span class="uppercase text-[var(&#45;&#45;text-color-light)] text-sm font-semibold"-->
            <!--              >Task Details</span-->
            <!--            >-->
            <!--            <div-->
            <!--              class="h-[150px] mt-2 flex justify-center items-center w-full rounded-lg text-[var(&#45;&#45;text-color-light)]"-->
            <!--            >-->
            <!--              <div-->
            <!--                class="flex flex-col justify-center items-center text-center gap-3 h-full w-1/3 text-[var(&#45;&#45;text-color-light)]"-->
            <!--              >-->
            <!--                <i class="fa-solid fa-list text-2xl"></i>-->
            <!--                <span class="text-xs leading-5">Task auswählen für Details</span>-->
            <!--              </div>-->
            <!--            </div>-->
            <!--          </div>-->

            <!-- Today Insights-->
            <div class="base-element mt-4">
              <span class="uppercase text-[var(--text-color-light)] text-sm font-semibold"
                >Heute</span
              >
              <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-2 w-full gap-2 mt-2">
                <div
                  class="px-2 py-2 flex flex-col justify-center items-center w-full rounded-lg text-[var(--text-color-light)] bg-[var(--background-color)]"
                >
                  <span class="text-[var(--accent-color)] text-2xl font-semibold">{{
                    taskDoneToday
                  }}</span>
                  <span class="text-xs">Erledigt</span>
                </div>
                <div
                  class="px-2 py-2 flex flex-col justify-center items-center w-full rounded-lg text-[var(--text-color-light)] bg-[var(--background-color)]"
                >
                  <span class="text-[var(--primary-color)] text-2xl font-semibold">{{
                    xpEarnedToday
                  }}</span>
                  <span class="text-xs">XP verdient</span>
                </div>
              </div>
            </div>

            <button
              v-if="checkedTasks.length > 0"
              @click="completeTask"
              class="scale-animation-sm border-2 flex items-center justify-center gap-2 uppercase border-b-gray-200 font-semibold text-sm cursor-pointer base-element mt-4 w-full text-[var(--text-color-white)] !bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"
            >
              <div class="flex items-center justify-center w-[25px] h-[25px] text-xl">
                <i class="fa-solid fa-clipboard-check"></i>
              </div>
              <span>Tasks abschliessen</span>
            </button>
          </section>
        </div>
      </div>

      <div v-else>
        <section class="flex flex-col gap-4">
          <header class="sm:flex hidden items-center justify-between gap-5">
            <div>
              <h1 class="font-bold text-3xl tracking-wide">
                Meine Tasks
                <!--                <span class="text-[var(&#45;&#45;primary-color)]"> {{ props.userName }}</span>-->
              </h1>
            </div>
            <div class="flex gap-2 items-center justify-center">
              <div
                class="text-nowrap text-[var(--text-color-light)] text-sm font-semibold px-2.5 py-1.5"
              >
                {{ ` ${day}, ${formatTime(date.getDate())} ${month} ${date.getFullYear()}` }}
              </div>
              <div class="flex items-center justify-center border-1 border-gray-200">
                <button
                  @click="showPopUpTask = true"
                  class="select-none scale-animation-sm cursor-pointer flex justify-center items-center gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] w-full px-4 py-2 rounded-lg text-[var(--text-color-white)] text-nowrap font-semibold border border-gray-200 text-md"
                >
                  <i class="fa-solid fa-plus"></i>
                  <span>Neue Task</span>
                </button>
              </div>
            </div>
          </header>

          <div class="sm:hidden z-999 fixed bottom-0 left-0 m-4 w-[50px] h-[50px] flex items-center justify-center">
            <button
              @click="showPopUpTask = true"
              class="select-none scale-animation-sm cursor-pointer flex justify-center items-center gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] w-full h-full px-4 py-2 rounded-2xl text-[var(--text-color-white)] text-nowrap font-semibold border border-gray-200 text-xl"
            >
              <i class="fa-solid fa-plus"></i>
            </button>
          </div>

          <!-- search area-->
          <div class="grid grid-cols-[1fr_auto] md:grid-cols-[1fr_auto_auto_auto] gap-2 shrink-0">
            <div
              class="searchbar input-hover-default flex items-center justify-start gap-2 bg-[var(--surface-color)] border! border-[var(--border-color)]! px-4 py-2 rounded-lg"
            >
              <i class="fa-solid fa-magnifying-glass text-[var(--text-color-light)]"></i>
              <input
                v-model="taskFilter"
                class="w-full outline-0 bg-transparent"
                type="text"
                placeholder="Tasks suchen ..."
              />
            </div>

            <button
              @click="isFilterDate = !isFilterDate"
              :style="
                isFilterDate ? 'color:var(--primary-color); border-color:var(--primary-color)' : ''
              "
              class="hidden md:flex select-none hover:text-[var(--primary-color)] hover:border-[var(--primary-color)] transition duration-200 border border-[var(--border-color)] cursor-pointer items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--surface-color)] px-4 py-2"
            >
              <i class="fa-solid fa-layer-group"></i>
              <span>nach Datum</span>
            </button>

            <select
              @change="handleCategoryChange"
              name="categories"
              class="outline-0 hidden md:flex select-none hover:text-[var(--primary-color)] hover:border-[var(--primary-color)] transition duration-200 border border-[var(--border-color)] cursor-pointer items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--surface-color)] px-4 py-2"
            >
              <option value="">Alle Kategorien</option>
              <option v-for="category in categoriesData ?? []" :key="category.id" :value="category.name">{{ category.name }}</option>
              <option value="new">Neue Kategorie</option>
            </select>

            <button
              :style="
                submittedFilterTask !== null
                  ? 'border-color:var(--primary-color); color:var(--primary-color)'
                  : ''
              "
              @click="submittedFilterTask !== null ? resetFilter() : (filterShown = true)"
              class="hidden md:flex select-none hover:text-[var(--primary-color)] hover:border-[var(--primary-color)] transition duration-200 border border-[var(--border-color)] cursor-pointer items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--surface-color)] px-4 py-2"
            >
              <i class="fa-solid fa-filter"></i>
              <span>Filter</span>
            </button>

            <button
              class="md:hidden w-full select-none hover:text-[var(--primary-color)] hover:border-[var(--primary-color)] transition duration-200 border border-[var(--border-color)] cursor-pointer flex items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--surface-color)] px-4 py-2"
            >
              <i class="fa-solid fa-filter"></i>
              <span class="hidden sm:block">Filter & Sortieren</span>
              <span class="block sm:hidden">Filter</span>
            </button>
          </div>

          <div class="flex flex-col sm:flex-row gap-4 items-center justify-between">
            <!-- View choosing-->
            <div
              class="grid grid-cols-3 gap-2 rounded-xl text-sm shrink-0 max-w-[320px]"
            >
              <div
                @click="changeViewOption(1)"
                :class="viewOption === 1 ? 'activeView' : ''"
                class="hover:border-[var(--border-color)] border border-transparent hover:bg-[var(--hover-light-color)] duration-200 transition cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-2"
              >
                <span class="">Alle</span>
                <span
                  class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                  >{{ (dashboardData?.tasksDone ?? 0) + (dashboardData?.tasksOpen ?? 0) }}</span
                >
              </div>
              <div
                class="hover:border-[var(--border-color)] border border-transparent hover:bg-[var(--hover-light-color)] duration-200 transition bg-transparent cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-2"
                @click="changeViewOption(2)"
                :class="viewOption === 2 ? 'activeView' : ''"
              >
                <span class="">Offen</span>
                <span
                  class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                  >{{ dashboardData?.tasksOpen ?? 0 }}</span
                >
              </div>
              <div
                class="hover:border-[var(--border-color)] border border-transparent hover:bg-[var(--hover-light-color)] duration-200 transition cursor-pointer flex items-center justify-center gap-2 w-full rounded-xl px-4 py-2"
                @click="changeViewOption(3)"
                :class="viewOption === 3 ? 'activeView' : ''"
              >
                <span class="">Erledigt</span>
                <span
                  class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200"
                  >{{ dashboardData?.tasksDone ?? 0 }}</span
                >
              </div>
            </div>

            <div class="hidden sm:flex gap-4 items-center justify-center">
              <button
                v-if="checkedTasks.length > 0"
                @click="completeTask"
                class="scale-animation-sm flex items-center justify-center gap-2 uppercase font-semibold text-sm cursor-pointer base-element w-full text-[var(--text-color-white)] !bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"
              >
                <div class="flex items-center justify-center w-[25px] h-[25px] text-xl">
                  <i class="fa-solid fa-clipboard-check"></i>
                </div>
                <span>Tasks abschliessen</span>
              </button>

              <div class="base-element text-sm text-[var(--text-color-light)] shrink-0">
                <div class="flex items-center justify-between w-full gap-2">
                  <span>Lv. {{ dashboardData?.level }}</span>
                  <span class="text-[var(--text-color)] font-semibold text-right"
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
            </div>
          </div>
        </section>

        <section>
          <!-- Tasks-->
          <div class="flex flex-col gap-3">
            <TasksComponent
              v-if="!taskStore.loading"
              v-for="task in filteredTaskData"
              :key="task.id"
              :task-title="task.title"
              :task-description="task.description"
              :timeMin="task.durationMin"
              :date="task.dueDate"
              :xp="task.xp"
              :completed="task.status === 3"
              :difficulty="task.difficulty"
              :is-checked="isTaskChecked(task.id)"
              :is-completed="task.status === 3"
              @update="showUpdateTask(task)"
              @delete="showDeleteTask(task.id)"
              @checked="changeCheckedTasks(task.id)"
            >
              <Tag
                v-if="task.category !== null && task.status !== 3 && !isTaskChecked(task.id)"
                :name="task.category.name"
                :color-hex="task.category.color"
                text-color-hex="#FFFFFF"
              ></Tag>
              <Tag
                v-if="task.category !== null && (task.status === 3 || isTaskChecked(task.id))"
                :name="task.category.name"
                color-hex="#d1d5dc"
                text-color-hex="#FFFFFF"
              ></Tag>
            </TasksComponent>

            <div
              v-if="taskStore.loading || filteredTaskData.length === 0"
              class="flex flex-col items-center justify-center gap-3"
            >
              <PlaceholderTask v-for="i in 10" :key="i"></PlaceholderTask>
            </div>
          </div>
        </section>
      </div>
    </main>
  </div>
</template>

<style scoped>
.deactivate-scrollbar {
  scrollbar-width: none;
}

.deactivate-scrollbar::-webkit-scrollbar {
  display: none;
}

.searchbar:has(input:focus) {
  border: 1px solid var(--primary-color);
}

.scrollbar {
  overflow-y: scroll;
  scroll-behavior: smooth;
  scrollbar-width: thin;
}

.activeView {
  background-color: var(--primary-color);
  color: var(--text-color-white);
  border-color: var(--primary-color);
}

.inactiveView {
  background-color: var(--surface-color);
  color: var(--text-color);
}
</style>
