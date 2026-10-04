<script lang="ts" setup>
import Logo from '@/components/ui/Logo.vue'
</script>

<template>
  <div class="w-full h-fit z-999 px-4">
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
</template>
