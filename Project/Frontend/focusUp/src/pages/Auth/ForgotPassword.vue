<script lang="ts" setup>
import DefaultNav from '@/components/layout/DefaultNav.vue'
import { onMounted } from 'vue'
import { applyUIMode } from '@/utils/modeUI.ts'

onMounted(async () => {
  applyUIMode()
})
</script>

<template>
  <div class="bg-linear-to-b min-h-screen from-[var(--surface-color)] to-[var(--secondary-color-light)]">
    <div class="relative z-50">
      <DefaultNav></DefaultNav>
    </div>

    <div class="absolute top-0 left-0 flex items-center min-h-full w-full justify-center px-4 py-8">
      <div class="bg-[var(--surface-color)] base-element sm:w-[400px] w-full">
        <div class="flex items-center justify-start gap-3">
          <div class="flex items-center justify-center w-[50px] h-[50px] bg-[var(--primary-color-light)] text-[var(--primary-color)] rounded-md">
            <i class="fa-solid fa-key"></i>
          </div>
          <h2 class="text-2xl font-bold text-nowrap">Passwort vergessen?</h2>
        </div>
        <p class="mt-3 text-sm text-[var(--text-color-light)]">Gebe deine E-Mail ein, um dein Passwort zurückzusetzen.</p>

        <div class="flex flex-col items-start justify-center gap-1 mt-4">
          <label class="text-sm font-semibold" for="email">E-Mail-Adresse</label>
          <input class="border border-[var(--border-color)] rounded-xl px-4 py-3 w-full outline-[var(--primary-color)]" placeholder="name@beispiel.ch" id="email" name="email" type="email" />

          <div class="scale-animation-sm flex items-center justify-center gap-2 w-full bg-[var(--primary-color)] rounded-xl text-[var(--text-color-white)] px-4 py-3 mt-2">
            <button>Reset-Link senden</button>
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-angle-right"></i>
            </div>
          </div>

          <div class="flex items-center justify-center gap-2 text-sm mt-3 w-full">
            <div class="flex items-center justify-center">
              <i class="fa-regular fa-clock"></i>
            </div>
            <p>Der Link ist 30 Minuten gültig.</p>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
