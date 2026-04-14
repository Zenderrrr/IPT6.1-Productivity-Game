import { defineStore } from 'pinia'
import { loginApi, meApi, registerApi } from '@/api/auth.api.ts'
import { computed, ref } from 'vue'
import type { User } from '@/types/user.ts'

export const useAuthStore = defineStore('auth', () => {
  const loading = ref<boolean>(false)
  const userData = ref<User | null>(null)
  const error = ref<string | null>(null)
  const token = ref<string | null>(localStorage.getItem('token'))

  const isAuth = computed(() => !!token.value)

  async function login(usernameOrEmail: string, password: string) {
    loading.value = true
    error.value = null

    try{
      const data = await loginApi(usernameOrEmail, password)
      token.value = data.token
      localStorage.setItem('token', token.value)
    }catch(e){
      error.value = e ? e.message : 'Unable to login'
    } finally {
      loading.value = false
    }
  }

  async function register(username: string, email: string, password: string) {
    loading.value = true
    error.value = null

    try{
      const data = await registerApi(username, email, password)
      // user.value.id = data
    }catch(e){
      error.value = e ? e.message : 'Unable to register'
    }
  }

  async function me() {
    if(token.value == null) {
      return
    }
    loading.value = true
    error.value = null

    try{
      userData.value = await meApi(token.value)
    }catch(e){
      error.value = e ? e.message : 'Unable to get user informations'
      token.value = null
      userData.value = null
      localStorage.removeItem('token')
    }finally {
      loading.value = false
    }
  }

  return { token, loading, error, login, me, user: userData, isAuth, register }
});
