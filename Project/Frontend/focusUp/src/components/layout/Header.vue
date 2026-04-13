<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <header class="sticky top-0 z-100 w-full border-b border-slate-200 bg-white/85 backdrop-blur-md">
    <div class="mx-auto w-full max-w-[1360px] px-10 max-[960px]:px-6">
      <div class="flex min-h-[72px] items-center justify-between">
        <a href="/#hero-section" class="text-[22px] font-black tracking-[-0.04em] no-underline">
          <span class="text-[var(--text-color)]">Focus</span>
          <span class="text-[var(--primary-color)]">Up</span>
        </a>

        <nav class="flex gap-8 max-[960px]:hidden">
          <a
            href="/#features-section"
            class="text-[15px] font-semibold text-slate-500 no-underline hover:text-[var(--text-color)]"
          >
            Funktionen
          </a>
          <a
            href="/#steps-section"
            class="text-[15px] font-semibold text-slate-500 no-underline hover:text-[var(--text-color)]"
          >
            So funktioniert's
          </a>
          <a
            href="/#momentum-section"
            class="text-[15px] font-semibold text-slate-500 no-underline hover:text-[var(--text-color)]"
          >
            Vorteile
          </a>
        </nav>

        <div class="flex items-center gap-[18px]">
          <RouterLink
            to="/login"
            class="font-bold text-slate-500 no-underline hover:text-[var(--text-color)] max-[960px]:hidden"
          >
            Anmelden
          </RouterLink>

          <RouterLink
            to="/login"
            class="inline-flex items-center gap-2 rounded-[14px] bg-[linear-gradient(135deg,var(--primary-color),var(--secondary-color))] px-[18px] py-3 text-[14px] font-extrabold text-white no-underline shadow-[0_10px_24px_rgba(14,165,233,0.25)] transition-all duration-200 ease-in-out hover:-translate-y-[1px] hover:brightness-95"
          >
            Kostenlos starten
            <span class="text-[14px]">→</span>
          </RouterLink>
        </div>
      </div>
    </div>
  </header>
</template>
