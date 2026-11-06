<script lang="ts" setup>
import DefaultNav from '@/components/layout/DefaultNav.vue'
import { onMounted, ref } from 'vue'
import { applyUIMode } from '@/utils/modeUI.ts'
import { useAuthStore } from '@/stores/authStore.ts'
import type { ForgotPasswortRequest } from '@/types/forgotPasswortRequest.ts'

onMounted(async () => {
  applyUIMode()
})

const authStore = useAuthStore()

const email = ref<string>('')
const error = ref<string>('')
const success = ref<string>('')
async function sendForgotPassword() {
  const forgotPasswort : ForgotPasswortRequest = {
    email: email.value
  }

  try{
    await authStore.forgotPassword(forgotPasswort)

    success.value = 'Anfrage erfolgreich gesendet.'
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
    <div
      class="bg-linear-to-b min-h-screen from-[var(--surface-color)] to-[var(--secondary-color-light)]"
    >
      <div class="relative z-50">
        <DefaultNav></DefaultNav>
      </div>

      <div
        class="absolute top-0 left-0 flex items-center min-h-full w-full justify-center px-4 py-8"
      >
        <div class="bg-[var(--surface-color)] base-element sm:w-[400px] w-full">
          <div class="flex items-center justify-start gap-3">
            <div
              class="flex items-center justify-center w-[40px] h-[40px] bg-[var(--primary-color-light)] text-[var(--primary-color)] rounded-lg"
            >
              <i class="fa-solid fa-key"></i>
            </div>
            <h2 class="text-2xl font-bold text-nowrap">Passwort vergessen?</h2>
          </div>
          <p class="mt-3 text-sm text-[var(--text-color-light)]">
            Gebe deine E-Mail ein, um dein Passwort zurückzusetzen.
          </p>

          <form
            @submit.prevent="sendForgotPassword"
            class="flex flex-col items-start justify-center gap-1 mt-4"
          >
<!--            <div-->
<!--              class="flex items-center justify-start gap-1 text-[var(&#45;&#45;text-color-light)] text-xs uppercase"-->
<!--            >-->
<!--              <i class="fa-regular fa-envelope"></i>-->
<!--              <label class="font-semibold" for="email">E-Mail</label>-->
<!--            </div>-->
<!--            <input-->
<!--              required-->
<!--              class="border border-[var(&#45;&#45;border-color)] rounded-xl px-4 py-3 w-full outline-[var(&#45;&#45;primary-color)]"-->
<!--              placeholder="name@beispiel.ch"-->
<!--              id="email"-->
<!--              name="email"-->
<!--              type="email"-->
<!--              v-model="email"-->
<!--            />-->

            <div class="flex flex-col items-start justify-center gap-1 w-full mb-1">
              <div
                class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase"
              >
                <i class="fa-regular fa-envelope"></i>
                <label class="font-semibold" for="email">E-Mail</label>
              </div>
              <input
                class="input-hover-default bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-gray-200 outline-[var(--primary-color)]"
                id="email"
                type="email"
                placeholder="max@beispiel.ch"
                required
                v-model="email"
              />
            </div>

            <button
              type="submit"
              class="cursor-pointer scale-animation-sm flex items-center justify-center gap-2 w-full bg-[var(--primary-color)] rounded-xl text-[var(--text-color-white)] px-4 py-3 mt-2"
            >
              <span>Reset-Link senden</span>
              <div class="flex items-center justify-center">
                <i class="fa-solid fa-angle-right"></i>
              </div>
            </button>

            <span v-if="error" class="text-[var(--error-color)] text-sm mt-1">{{ error }}</span>
            <span v-if="success" class="text-[var(--accent-color)] text-sm mt-1">{{ success }}</span>

            <div class="flex items-center justify-center gap-2 text-sm mt-3 w-full">
              <div class="flex items-center justify-center">
                <i class="fa-regular fa-clock"></i>
              </div>
              <p>Der Link ist 30 Minuten gültig.</p>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
