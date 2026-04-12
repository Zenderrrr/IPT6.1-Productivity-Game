<script lang="ts" setup>
import Logo from '@/components/ui/Logo.vue'
import { ref } from 'vue'

const isPasswordVisible = ref<boolean>(false)
function showPassword() {
  return isPasswordVisible.value
}
</script>

<template>
  <div class="w-full h-[100vh] bg-dots overflow-hidden">
    <div class="sm:w-[600px] w-full sm:h-[600px] bg-primary-gradient"></div>
    <div class="absolute bottom-0 right-0 sm:w-[600px] w-full sm:h-[600px] bg-secondary-gradient"></div>

    <!-- Login window-->
    <div class="flex items-center h-full justify-center sm:-translate-y-1/2">
      <div class="bg-[var(--surface-color)] base-element sm:w-[400px] w-full">
        <!-- Header -->
        <div class="flex items-center justify-between">
          <RouterLink
            to="/"
            class="flex items-center justify-center gap-1 text-xs font-semibold text-[var(--text-color-light)]"
          >
            <div class="">
              <i class="fa-solid fa-arrow-left"></i>
            </div>
            <span>Zurück</span>
          </RouterLink>
          <Logo link="/"></Logo>
        </div>

        <!-- Visual-->
        <div
          class="w-full h-[120px] bg-[var(--background-color)] rounded-lg my-3 flex justify-center items-center text-[var(--primary-color)] uppercase"
        >
          <span>Kommt bald</span>
        </div>

        <div>
          <!-- Title-->
          <h1 class="font-bold text-2xl">
            Willkommen <em class="text-[var(--primary-color)]">zurück</em>
          </h1>
          <p class="text-sm text-[var(--text-color-light)]">
            Melde dich an, um weiter Fortschritte zu machen
          </p>

          <!-- Form (input elements)-->
          <form @submit.prevent>
            <div class="flex flex-col items-start justify-center mt-5 gap-1">
              <div class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase">
                <i class="fa-regular fa-envelope"></i>
                <label
                  class="font-semibold"
                  for="email"
                >E-Mail</label>
              </div>
              <input
                class="bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-gray-200 outline-[var(--primary-color)]"
                id="email"
                type="email"
                placeholder="max@beispiel.ch"
              />
            </div>

            <div class="relative flex flex-col items-start justify-center mt-5 gap-1">
              <div class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase">
                <i class="fa-solid fa-key"></i>
                <label
                  class="font-semibold"
                  for="password"
                >Passwort</label>
              </div>
              <input
                class=" bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-gray-200 outline-[var(--primary-color)]"
                id="password"
                :type="showPassword() ? 'text' : 'password'"
                placeholder="••••••••"
              />

              <button @click="isPasswordVisible = true" v-if="!showPassword()" class="absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer">
                <i class="fa-solid fa-eye"></i>
              </button>

              <button @click="isPasswordVisible = false" v-if="showPassword()" class="absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer">
                <i class="fa-solid fa-xmark"></i>
              </button>
            </div>

            <div class="flex flex-col items-end justify-center mt-2 mb-3">
              <a class="text-xs text-[var(--secondary-color)] font-semibold cursor-pointer"
                >Passwort vergessen?</a
              >
            </div>
            <button
              class="w-full bg-[var(--primary-color)] text-[var(--text-color-white)] cursor-pointer font-semibold text-center py-3 rounded-2xl shadow-lg"
              type="submit"
            >
              Anmelden
            </button>
          </form>

          <div class="flex justify-center items-center gap-1 my-4">
            <div class="h-0.5 w-[100%] bg-gray-200 rounded-2xl"></div>
            <span class="text-[var(--text-color-light)] text-xs">oder</span>
            <div class="h-0.5 w-[100%] bg-gray-200 rounded-2xl"></div>
          </div>

          <RouterLink to="/register">
            <button
              class="w-full text-[var(--secondary-color)] border-2 border-[var(--secondary-color)] cursor-pointer font-semibold text-center py-3 rounded-2xl"
            >
              Konto erstellen
            </button>
          </RouterLink>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.bg-dots {
  background-image: radial-gradient(gray 0.1px, transparent 1px);
  background-size: 20px 20px;
  background-position: center;
}

.bg-primary-gradient {
  background: linear-gradient(
    to bottom right,
    rgba(20, 184, 166, 0.4),
    transparent 50%,
    transparent 100%
  );
}

.bg-secondary-gradient {
  background: linear-gradient(
    to top left,
    rgba(14, 165, 233, 0.4),
    transparent 50%,
    transparent 100%
  );
}
</style>
