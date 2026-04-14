import type { Task } from '@/types/task.ts'
import { apiFetch } from '@/api/api.ts'
import type { CreateTask } from '@/types/createTask.ts'

export async function createTaskApi(task: CreateTask): Promise<number> {
  return await apiFetch('/tasks', {
    method: 'POST',
    body: JSON.stringify(task),
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`,
    },
  })
}

export async function updateTaskApi(task: Task): Promise<void> {
  await apiFetch(`/tasks/${task.id}`, {
    method: 'PUT',
    body: JSON.stringify(task),
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`,
    }
  })
}

export async function deleteTaskApi(id: number): Promise<void> {
  await apiFetch(`/tasks/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`,
    }
  })
}

export async function getAllTasksApi(): Promise<Task[]> {
  return await apiFetch('/tasks', {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`,
    }
  })
}

export async function getTaskByIdApi(id: number): Promise<Task> {
  return await apiFetch(`/tasks/${id}`, {
    method: 'GET',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`,
    }
  })
}
