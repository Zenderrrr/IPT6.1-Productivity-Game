<script lang="ts" setup>

import Logo from '@/components/ui/Logo.vue'
</script>

<template>
  <div class="bg-linear-to-b min-h-screen from-[var(--surface-color)] to-[var(--secondary-color-light)]">
    <div class="w-full h-fit">
      <nav class="flex items-center justify-between max-w-[1400px] mx-auto w-full py-5">
        <Logo link="/" class="bg-transparent"></Logo>

        <div class="flex items-center justify-between gap-2">
          <RouterLink to="/register" class="hover:text-[var(--text-color)] hover:border-[var(--text-color)] transition-all duration-100 text-sm px-3 py-2 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl text-[var(--text-color-light)] font-semibold">
            Registrieren
          </RouterLink>

          <RouterLink to="/login" class="hover:text-[var(--text-color)] hover:border-[var(--text-color)] flex items-center justify-center gap-2 text-sm px-3 py-2 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl text-[var(--text-color-light)] font-semibold">
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-right-to-bracket"></i>
            </div>
            Anmelden
          </RouterLink>
        </div>
      </nav>
    </div>

    <div class="w-full h-fit flex flex-col gap-2 items-center justify-between">
      <h1 class="flex items-center justify-center gap-4 text-[14rem] leading-50 font-extrabold text-[var(--text-color)] text-center">
        <span>4</span>
        <svg class="loader shadow-lg rounded-full" viewBox="0 0 100 100">
          <defs>
            <linearGradient id="loader-gradient" x1="0%" y1="0%" x2="100%" y2="100%">
              <stop offset="0%" stop-color="var(--secondary-color)" />
              <stop offset="100%" stop-color="var(--primary-color)" />
            </linearGradient>
          </defs>

          <circle
            class="loader-background"
            cx="50"
            cy="50"
            r="40"
          />

          <circle
            class="loader-progress"
            cx="50"
            cy="50"
            r="40"
          />
        </svg>
        <span>4</span>
      </h1>

      <h2 class="text-[2.5rem] leading-13 font-extrabold max-w-[650px] text-center">Diese Seite hat ihren Fokus verloren.</h2>

      <p class="text-[var(--text-color-light)] text-lg max-w-[450px] text-center mt-2">Die gesuchte Seite existiert nicht, wurde verschoben oder ist nicht mehr verfügbar. Kehre zurück und setze deinen Fortschritt fort.</p>
    </div>

    <div class="flex items-center justify-center gap-5 mt-11">
      <RouterLink to="/" class="hover:text-[var(--primary-color)] hover:shadow-2xl hover:bg-[var(--surface-color)] hover:border-[var(--primary-color)] border border-transparent transition-all duration-100 shadow-lg flex items-center justify-center gap-2 font-bold px-6 py-4 bg-[var(--primary-color)] text-[var(--text-color-white)] rounded-2xl">
        <div class="flex items-center justify-center">
          <i class="fa-regular fa-house"></i>
        </div>
        Zur Startseite
      </RouterLink>

      <RouterLink to="/dashboard" class="hover:shadow-2xl hover:border-[var(--text-color)] transition-all duration-100 shadow-lg flex items-center justify-center gap-2 font-bold px-6 py-4 bg-[var(--surface-color)] text-[var(--text-color)] border border-[var(--border-color)] rounded-2xl">
        Zum Dashboard
      </RouterLink>
    </div>
  </div>
</template>

<style scoped>
.loader {
  width: 230px;
  height: 230px;
  animation: rotate 10s infinite linear;
}

.loader-background,
.loader-progress {
  fill: none;
  stroke-width: 8;
}

.loader-background {
  stroke: var(--border-color);
}

.loader-progress {
  stroke: url("#loader-gradient");
  stroke-linecap: round;
  stroke-dasharray: 170 220;
}

@keyframes rotate {
  to {
    transform: rotate(1turn);
  }
}
</style>
