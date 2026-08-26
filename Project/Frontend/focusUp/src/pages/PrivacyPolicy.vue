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
        Datenschutzerklärung
      </h1>

      <div class="space-y-8 text-[var(--text-color-light)] text-lg leading-8">
        <section>
          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3">
            Lorem Ipsum
          </h2>
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor
            incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud
            exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
          </p>
        </section>

        <section>
          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3">
            Lorem Ipsum
          </h2>
          <p>
            Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat
            nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui
            officia deserunt mollit anim id est laborum.
          </p>
        </section>

        <section>
          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3">
            Lorem Ipsum
          </h2>
          <p>
            Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent
            libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum
            imperdiet.
          </p>
        </section>

        <section>
          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3">
            Lorem Ipsum
          </h2>
          <p>
            Cras mattis consectetur purus sit amet fermentum. Donec sed odio dui. Maecenas faucibus
            mollis interdum. Aenean lacinia bibendum nulla sed consectetur.
          </p>
        </section>

        <section>
          <h2 class="text-2xl font-bold text-[var(--text-color)] mb-3">
            Lorem Ipsum
          </h2>
          <p>
            Curabitur blandit tempus porttitor. Nullam quis risus eget urna mollis ornare vel eu
            leo. Donec ullamcorper nulla non metus auctor fringilla.
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
