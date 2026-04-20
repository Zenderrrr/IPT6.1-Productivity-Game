import { defineStore } from 'pinia'
import { deleteUserApi, loginApi, logoutApi, meApi, RefreshApi, registerApi } from '@/api/auth.api.ts'
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
      localStorage.setItem('token', token.value ?? '')
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
      return await registerApi(username, email, password)
    }catch(e){
      error.value = e ? e.message : 'Unable to register'
    } finally {
      if(!error.value){
        await login(username, password)
      }
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

  async function deleteUser() {
    if(token.value == null) {
      return
    }
    try{
      await deleteUserApi(token.value)
    }catch(e){
      error.value = e ? e.message : 'Unable to delete user'
    }
  }

  async function logout(){
    if(token.value == null) {
      return
    }

    try {
      await logoutApi(token.value)
      userData.value = null
      token.value = null
      localStorage.removeItem('token')
    }catch(e){
      error.value = e ? e.message : 'Unable to logout'
    }
  }

  async function refresh(){
    try {
      token.value = await RefreshApi()
      localStorage.setItem('token', token.value ?? '')
    }catch (e){
      await logout()
      error.value = e ? e.message : 'Unable to refresh'
    }
  }

  return { token, loading, error, login, me, user: userData, isAuth, register, logout, deleteUser, refresh}
});
