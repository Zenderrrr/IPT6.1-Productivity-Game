import type { Task } from '@/types/task.ts'
import { apiFetch } from '@/api/api.ts'
import type { CreateTaskType } from '@/types/createTaskType.ts'
import type { UpdateTask } from '@/types/updateTask.ts'

export async function createTaskApi(task: CreateTaskType): Promise<number> {
  return await apiFetch('/tasks', {
    method: 'POST',
    body: JSON.stringify(task),
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`,
    },
  })
}

export async function updateTaskApi(taskId: number, task: UpdateTask): Promise<Task> {
  return await apiFetch(`/tasks/${taskId}`, {
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

export async function completeTaskApi(id: number): Promise<void> {
  return await apiFetch(`/tasks/${id}/complete`, {
    method: 'POST',
    headers: {
      Authorization: `BEARER ${localStorage.getItem('token')}`,
    }
  })
}
