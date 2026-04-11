<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import StatsOverviewCard from '@/components/ui/StatsOverviewCard.vue'
</script>

<template>
  <NavAuth nameInitials="SS"></NavAuth>
  <main class="w-full max-w-[64rem] mx-auto  my-10">

    <!-- Greeting Section-->
    <section class="w-full mb-5">
      <header class="flex items-center justify-between">
        <div>
          <h1 class="font-bold text-3xl tracking-wide">Willkommen zurück,
            <span class="text-[var(--primary-color)]">Sanjivan</span>
          </h1>
          <p class="text-[var(--text-color-light)]">Du bist auf einem guten Weg, bleib dran!</p>
        </div>
        <div class="text-[var(--text-color-light)] text-sm font-semibold px-2.5 py-1.5 bg-[var(--surface-color)] border border-gray-200 rounded-full">
          Dienstag, 31. März 2026
        </div>
      </header>
    </section>

    <!-- Card overview section-->
    <section>
      <div class="grid grid-cols-4 gap-4 auto-rows-[150px]">
        <StatsOverviewCard svg="fa-solid fa-star" stats-name="Gesamt XP" :statsValue="12840">
          <span><em class="text-[var(--accent-color)]">+340 XP</em> diese Woche</span>
        </StatsOverviewCard>
        <StatsOverviewCard svg="fa-solid fa-chart-line" stats-name="Level" :stats-value="12">
          <div>
            <div class="w-full bg-[var(--background-color)] h-2 rounded-full overflow-hidden mb-2.5">
              <div class="w-65/100 h-full bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] rounded-full"></div>
            </div>
            <span>3200 / 4000 XP zum nächsten Level</span>
          </div>
        </StatsOverviewCard>
        <StatsOverviewCard svg="fa-solid fa-fire" stats-name="Streak" :stats-value="5">
          <span>Tage in Folge</span>
          <div class="grid grid-cols-7 gap-2 mt-2">
            <div v-for="i in 5" :key="i" class="h-1.5 bg-[var(--primary-color)] rounded-full"></div>
            <div v-for="i in 2" :key="i" class="h-1.5 bg-[var(--background-color)] rounded-full"></div>
          </div>
        </StatsOverviewCard>
        <StatsOverviewCard svg="fa-solid fa-check" stats-name="Erledigte Tasks" :stats-value="47">
          <span><em class="text-[var(--accent-color)]">+8</em> heute - 12 offen</span>
        </StatsOverviewCard>
      </div>
    </section>


  </main>
</template>
