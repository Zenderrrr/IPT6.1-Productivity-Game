<script setup lang="ts">
import { RouterLink } from 'vue-router'
import Logo from '@/components/ui/Logo.vue'
</script>

<template>
  <footer class="w-full border-t border-slate-200 bg-[var(--surface-color)]">
    <div class="mx-auto w-full max-w-[1360px] px-10 max-[960px]:px-6">
      <div
        class="grid min-h-[84px] grid-cols-[1fr_auto_1fr] items-center gap-5 max-[960px]:grid-cols-1 max-[960px]:justify-items-center max-[960px]:py-6"
      >
        <!-- Logo -->
        <logo link="/"></logo>

        <!-- Links -->
        <div
          class="flex items-center justify-self-center gap-7 max-[960px]:flex-wrap max-[960px]:gap-[18px]"
        >
          <a
            href="#"
            class="text-[14px] font-semibold text-slate-400 no-underline transition-colors duration-200 hover:text-[var(--text-color)]"
          >
            Impressum
          </a>
          <a
            href="#"
            class="text-[14px] font-semibold text-slate-400 no-underline transition-colors duration-200 hover:text-[var(--text-color)]"
          >
            Datenschutz
          </a>
        </div>

        <!-- Copy -->
        <div
          class="justify-self-end text-right text-[14px] font-medium text-slate-400 max-[960px]:justify-self-center max-[960px]:text-center"
        >
          © 2026 FocusUp - Sanjivan & Egor
        </div>
      </div>
    </div>
  </footer>
</template>
