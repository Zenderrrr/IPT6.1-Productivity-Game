<script lang="ts" setup>
import Logo from '@/components/ui/Logo.vue'
import { onMounted } from 'vue'
import { applyUIMode } from '@/utils/modeUI.ts'

onMounted(async () => {
  applyUIMode()
})
</script>

<template>
  <div class="bg-linear-to-b min-h-screen from-[var(--surface-color)] to-[var(--secondary-color-light)] flex flex-col">
    <!-- Header -->
    <div class="w-full h-fit">
      <nav class="flex items-center justify-between max-w-[1400px] mx-auto w-full py-5">
        <Logo link="/" class="bg-transparent"></Logo>

        <RouterLink
          to="/"
          class="hover:text-[var(--primary-color)] hover:shadow-2xl hover:border-[var(--primary-color)] transition-all duration-100 text-sm px-4 py-2.5 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl text-[var(--text-color)] font-semibold flex items-center justify-center gap-2"
        >
          <div class="flex items-center justify-center">
            <i class="fa-regular fa-house"></i>
          </div>
          Zurück zur Startseite
        </RouterLink>
      </nav>
    </div>

    <!-- Main -->
    <main class="flex-1 max-w-[900px] mx-auto w-full px-5 py-16">
      <h1 class="text-[3.5rem] leading-tight font-extrabold text-[var(--text-color)] mb-10">
        Impressum
      </h1>

      <div class="space-y-8 text-[var(--text-color-light)] text-lg leading-8">
        <section>
          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3">
            Betreiber
          </h2>
          <p>
            Sanjivan Selvarasa & Egor Biriukov <br>
            FocusUp
          </p>

          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3 mt-5">
            Kontaktadresse
          </h2>
          <address class="not-italic">
            Oberhofstrasse 41 <br>
            6020 Emmenbrücke <br>
            Schweiz
          </address>

          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3 mt-5">
            Kontakt
          </h2>
          <a href="mailto:kontakt@focusup.ch">
            E-Mail: kontakt@focusup.ch
          </a>

          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3 mt-5">
            Verantwortung für den Inhalt
          </h2>
          <p>
            Sanjivan Selvarasa & Egor Biriukov
          </p>

          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3 mt-5">
            Hinweis
          </h2>
          <p>
            FocusUp ist in der laufenden Entwicklung.
          </p>
        </section>
      </div>
    </main>

    <!-- Footer -->
    <footer class="w-full border-t border-[var(--border-color)]">
      <div class="max-w-[1400px] mx-auto w-full px-5 py-8 flex items-center justify-between">
        <Logo link="/" class="bg-transparent"></Logo>

        <div class="flex items-center gap-5 text-sm text-[var(--text-color-light)]">
          <RouterLink
            to="/legal-notice"
            class="hover:text-[var(--text-color)] transition-all duration-100"
          >
            Impressum
          </RouterLink>

          <RouterLink
            to="/privacy-policy"
            class="hover:text-[var(--text-color)] transition-all duration-100"
          >
            Datenschutz
          </RouterLink>
        </div>
      </div>
    </footer>
  </div>
</template>
