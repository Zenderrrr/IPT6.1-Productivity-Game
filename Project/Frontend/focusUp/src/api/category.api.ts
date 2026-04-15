import { apiFetch } from '@/api/api.ts'
import type { Category } from '@/types/category.ts'
import type { CreateCategory } from '@/types/createCategory.ts'

export async function getAllCategoriesApi() : Promise<Category[]> {
  return await apiFetch('/categories', {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  })
}

export async function getCategoryByIdApi(id: number) : Promise<Category> {
  return await apiFetch(`/categories/${id}`, {
    method: 'GET',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  })
}

export async function createCategoryApi(category: CreateCategory) : Promise<number> {
  return await apiFetch(`/categories`, {
    method: 'POST',
    body: JSON.stringify(category),
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  })
}

export async function updateCategoryApi(category: Category) {
  await apiFetch(`/categories/${category.id}`, {
    method: 'PUT',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  })
}

export async function deleteCategoryApi(id: number) {
  await apiFetch(`/categories/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: `Bearer ${localStorage.getItem('token')}`
    }
  })
}
