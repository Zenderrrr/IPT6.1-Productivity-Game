import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Task } from '@/types/task.ts'
import {
  completeTaskApi,
  createTaskApi,
  deleteTaskApi,
  getAllTasksApi,
  getTaskByIdApi,
  updateTaskApi,
} from '@/api/task.api.ts'
import type { CreateTaskType } from '@/types/createTaskType.ts'
import type { UpdateTask } from '@/types/updateTask.ts'
import type { TaskCompleteType } from '@/types/taskComplete.ts'

export const useTaskStore = defineStore('task', () => {
  const loading = ref<boolean>(false)
  const error = ref<string | null>(null)
  const allTasksData = ref<Task[] | null>(null);

  async function getAllTasks(){
    loading.value = true
    error.value = null

    try{
      const data = await getAllTasksApi()

      allTasksData.value = data.map( t => ({
        ...t,
        dueDate: t.dueDate ? new Date(t.dueDate) : undefined,
        createdAt: new Date(t.createdAt),
        updatedAt: new Date(t.updatedAt),
        completedAt: t.completedAt ? new Date(t.completedAt) : undefined,
      }))
    }catch(e){
      error.value = e ? e.message : 'Unable to fetch tasks'
    } finally {
      loading.value = false
    }
  }

  async function getTaskById(id:number) {
    loading.value = true
    error.value = null

    try{
      const data = await getTaskByIdApi(id)

      return {
        ...data,
        dueDate: data.dueDate ? new Date(data.dueDate) : undefined,
        createdAt: new Date(data.createdAt),
        updatedAt: new Date(data.updatedAt),
        completedAt: data.completedAt ? new Date(data.completedAt) : undefined,
      }
    }catch(e){
      error.value = e ? e.message : 'Unable to fetch task by id'
    } finally{
      loading.value = false
    }
  }

  async function createTask(task: CreateTaskType){
    error.value = null

    try{
      const newTaskId = await createTaskApi(task)
      await getAllTasks()
      return newTaskId
    }catch(e){
      error.value = e ? e.message : 'Unable to create task'
    }
  }

  async function updateTask(taskId: number, task: UpdateTask) {
    error.value = null

    try{
      return await updateTaskApi(taskId, task)
    }catch(e){
      error.value = e ? e.message : 'Unable to update task'
    } finally {
      if(!error.value){
        await getAllTasks()
      }
    }
  }

  async function completeTask(taskId: number) : Promise<TaskCompleteType | undefined> {
    error.value = null
    try{
      return await completeTaskApi(taskId)
    }catch(e){
      error.value = e ? e.message : 'Unable to complete task'
    }
  }

  async function deleteTask(id:number){
    error.value = null

    try{
      await deleteTaskApi(id)
    }catch(e){
      error.value = e ? e.message : 'Unable to delete task'
    }
  }


  return {getAllTasks, loading, error, allTasksData, getTaskById, createTask, updateTask, deleteTask, completeTask}
})
