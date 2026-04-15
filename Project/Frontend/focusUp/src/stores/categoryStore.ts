import { defineStore } from 'pinia'
import { ref } from 'vue'
import {
  createCategoryApi,
  deleteCategoryApi,
  getAllCategoriesApi,
  getCategoryByIdApi,
  updateCategoryApi,
} from '@/api/category.api.ts'
import type { Category } from '@/types/category.ts'
import type { CreateCategory } from '@/types/createCategory.ts'

export const useCategoryStore = defineStore('category', () => {
  const loading = ref<boolean>(false)
  const error = ref<string | null>(null)

  const categoriesData = ref<Category[] | null>(null)

  async function getAllCategories() {
    loading.value = true
    error.value = null
    try {
      categoriesData.value = await getAllCategoriesApi()
    }catch(e) {
      error.value = e ? e.message : 'Failed to fetch categories'
    } finally {
      loading.value = false
    }
  }

  async function getCategoryById(id: number) {
    loading.value = true
    error.value = null
    try {
      return await getCategoryByIdApi(id)
    }catch(e) {
      error.value = e ? e.message : 'Failed to fetch category by id'
    } finally {
      loading.value = false
    }
  }

  async function createCategory(category: CreateCategory) {
    loading.value = true
    error.value = null
    try {
      return await createCategoryApi(category)
    }catch(e) {
      error.value = e ? e.message : 'Failed to create category'
    } finally {
      loading.value = false
    }
  }

  async function updateCategory(category: Category) {
    loading.value = true
    error.value = null
    try {
      await updateCategoryApi(category)
    }catch(e) {
      error.value = e ? e.message : 'Failed to update category'
    } finally {
      loading.value = false
    }
  }

  async function deleteCategory(id: number) {
    loading.value = true
    error.value = null
    try {
      await deleteCategoryApi(id)
    }catch(e) {
      error.value = e ? e.message : 'Failed to delete category'
    } finally {
      loading.value = false
    }
  }

  return {loading, error, categoriesData, getAllCategories, getCategoryById: getCategoryById, createCategory, updateCategory, deleteCategory}
})
