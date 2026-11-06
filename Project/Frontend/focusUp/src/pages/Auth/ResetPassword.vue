<script lang="ts" setup>
import { useRoute } from 'vue-router'
import DefaultNav from '@/components/layout/DefaultNav.vue'
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore.ts'
import type { ResetPasswordRequest } from '@/types/resetPasswordRequest.ts'

const authStore = useAuthStore()

const route = useRoute()
const token = typeof route.query.token === 'string' ? route.query.token : ''

const isPasswordVisible = ref<boolean>(false)
const isConfirmPasswordVisible = ref<boolean>(false)
function showPassword() {
  return isPasswordVisible.value
}

function showConfirmPassword() {
  return isConfirmPasswordVisible.value
}

const password = ref<string>('')
const confirmPassword = ref<string>('')

const error = ref<string | null>(null)
const success = ref<string | null>(null)

async function sendPasswordReset(){
  if(password.value != confirmPassword.value){
    error.value = 'Die beiden Passwörter stimmen nicht überein.'
    return
  }

  try{
    const resetRequest : ResetPasswordRequest = {
      token: token,
      newPassword: password.value,
    }
    await authStore.resetPassword(resetRequest)

    success.value = 'Das Passwort konnte erfolgreich geändert werden.'
  }catch (e){
    if (e instanceof Error) {
      const json = e.message.match(/\{.*\}/)

      if (json) {
        error.value = JSON.parse(json[0]).message
      } else {
        error.value = e.message
      }
    }
  }
}
</script>

<template>
  <div class="w-full min-h-screen bg-dots overflow-x-hidden">
    <div class="bg-linear-to-b min-h-screen from-[var(--surface-color)] to-[var(--secondary-color-light)]">
      <div class="relative z-50">
        <DefaultNav></DefaultNav>
      </div>

      <div class="absolute top-0 left-0 flex items-center min-h-full w-full justify-center px-4 py-8">
        <div class="bg-[var(--surface-color)] base-element sm:w-[400px] w-full">
          <div class="flex items-center justify-start gap-3">
            <div class="flex items-center justify-center w-[40px] h-[40px] bg-[var(--primary-color-light)] text-[var(--primary-color)] rounded-lg">
              <i class="fa-solid fa-key"></i>
            </div>
            <h2 class="text-2xl font-bold text-nowrap">Neues Passwort erstellen</h2>
          </div>
          <p class="mt-3 text-sm text-[var(--text-color-light)]">Gebe ein neues Passwort ein, um wieder Zugang zu deinem Konto zu bekommen.</p>

          <form @submit.prevent="sendPasswordReset" class="flex flex-col items-start justify-center gap-1">
              <div class="relative flex flex-col items-start justify-center mt-5 gap-1 w-full">
                <div
                  class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase"
                >
                  <i class="fa-solid fa-key"></i>
                  <label class="font-semibold" for="password">Neues Passwort</label>
                </div>
                <input
                  class="input-hover-default bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-[var(--border-color)] outline-[var(--primary-color)]"
                  id="password"
                  :type="showPassword() ? 'text' : 'password'"
                  placeholder="••••••••"
                  required
                  v-model="password"
                />

                <button
                  @click="isPasswordVisible = true"
                  v-if="!showPassword()"
                  class="hover:text-[var(--primary-color)] transition duration-200 absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer"
                >
                  <i class="fa-solid fa-eye"></i>
                </button>

                <button
                  @click="isPasswordVisible = false"
                  v-if="showPassword()"
                  class="hover:text-[var(--primary-color)] absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer"
                >
                  <i class="fa-solid fa-xmark"></i>
                </button>
              </div>

              <div class="relative flex flex-col items-start justify-center mt-5 gap-1 w-full">
                <div
                  class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase"
                >
                  <i class="fa-regular fa-circle-check"></i>
                  <label class="font-semibold" for="password">Bestätige Passwort</label>
                </div>
                <input
                  class="input-hover-default bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-[var(--border-color)] outline-[var(--primary-color)]"
                  id="password"
                  :type="showConfirmPassword() ? 'text' : 'password'"
                  placeholder="••••••••"
                  required
                  v-model="confirmPassword"
                />

                <button
                  @click="isConfirmPasswordVisible = true"
                  v-if="!showConfirmPassword()"
                  class="hover:text-[var(--primary-color)] transition duration-200 absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer"
                >
                  <i class="fa-solid fa-eye"></i>
                </button>

                <button
                  @click="isConfirmPasswordVisible = false"
                  v-if="showConfirmPassword()"
                  class="hover:text-[var(--primary-color)] absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer"
                >
                  <i class="fa-solid fa-xmark"></i>
                </button>
              </div>
            <button type="submit" class="cursor-pointer scale-animation-sm flex items-center justify-center gap-2 w-full bg-[var(--primary-color)] rounded-xl text-[var(--text-color-white)] px-4 py-3 mt-2">
              <span>Passwort zurücksetzen</span>
              <div class="flex items-center justify-center">
                <i class="fa-solid fa-angle-right"></i>
              </div>
            </button>

            <span v-if="error" class="text-[var(--error-color)] text-sm mt-1">{{ error }}</span>
            <span v-if="success" class="text-[var(--accent-color)] text-sm mt-1">{{ success }}</span>

            <RouterLink to="/login" class="hover:text-[var(--primary-color)] transition duration-75 flex items-center justify-center gap-1 text-sm mt-3 w-full text-[var(--text-color-light)]">
              <div class="flex items-center justify-center">
                <i class="fa-solid fa-angle-left"></i>
              </div>
              <p>Zurück zur Anmeldung</p>
            </RouterLink>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
