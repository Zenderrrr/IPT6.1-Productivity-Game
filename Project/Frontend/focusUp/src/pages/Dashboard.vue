<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import StatsOverviewCard from '@/components/ui/StatsOverviewCard.vue'
import StatsCompletedTask from '@/components/ui/StatsCompletedTask.vue'
import CurrentWeekStats from '@/components/ui/CurrentWeekStats.vue'
</script>

<template>
  <NavAuth nameInitials="SS"></NavAuth>
  <main class="w-full max-w-[64rem] mx-auto  my-10">

    <!-- Greeting Section-->
    <section class="">
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

    <!-- Last completed tasks & quick actions-->
    <section class="grid grid-cols-6 gap-4">
      <!-- Title -->
      <div class="col-span-4 bg-[var(--surface-color)] gen-padding rounded-2xl">
        <div class="flex items-center justify-between">
          <span class="font-semibold text-lg">Letzte erledigte Tasks</span>
          <div class="flex items-center justify-center gap-1 text-[var(--primary-color)] text-sm font-semibold">
            <span>Alle ansehen</span>
            <div>
              <i class="fa-solid fa-arrow-right"></i>
            </div>
          </div>
        </div>

        <!-- Tasks -->
        <div class="grid grid-cols-1 grid-rows-5 gap-3 mt-4">
          <div v-for="i in 5" :key="i">
            <StatsCompletedTask title="Landing Page Navbar fertigstellen" :date="new Date(2026, 3, 6, 20, 9, 54)" :xp="53"></StatsCompletedTask>
          </div>
        </div>
      </div>

      <div class="col-span-2 bg-[var(--surface-color)] rounded-2xl gen-padding">
        <span class="font-bold ">Schnelle Aktionen</span>

        <!-- Quick Actions-->
        <div class="mb-4">
          <div class="flex items-center justify-start mt-4 gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] rounded-xl px-4 py-4 cursor-pointer">
            <div class="w-[30px] h-[30px] flex items-center justify-center text-[var(--primary-color-white)] bg-white/20 backdrop-blur-2lg rounded-lg">
              <i class="fa-solid fa-plus text-sm"></i>
            </div>
            <span class="font-semibold">Task erstellen</span>
          </div>

          <div class="flex items-center justify-start mt-4 gap-2 bg-[var(--background-color)] border border-gray-200 rounded-xl px-4 py-4 cursor-pointer">
            <div class="w-[30px] h-[30px] flex items-center justify-center rounded-lg bg-[var(--text-color-white)]">
              <i class="fa-solid fa-list-check text-sm"></i>
            </div>
            <span class="font-semibold">Tasks anzeigen</span>
          </div>
        </div>

        <!-- Stats Current Week-->
        <span class="uppercase text-[var(--text-color-light)] text-sm font-semibold">Diese Woche</span>
        <div class="grid grid-cols-2 grid-rows-2 gap-3 mt-4">
          <CurrentWeekStats :stats-value="23" name="Tasks"></CurrentWeekStats>
          <CurrentWeekStats :stats-value="8.4" name="Fokus" digit="h"></CurrentWeekStats>
          <CurrentWeekStats :stats-value="340" name="XP Gewonnen" digit="+" color-tailwind="text-[var(--primary-color)]"></CurrentWeekStats>
          <CurrentWeekStats :stats-value="79" name="Ziel erreicht" digit="%" color-tailwind="text-[var(--accent-color)]"></CurrentWeekStats>
        </div>
      </div>
    </section>
  </main>
</template>

<style scoped>
  section{
    margin-bottom: calc(var(--spacing) * 5);
  }

  .gen-padding{
    padding: calc(var(--spacing) * 5) calc(var(--spacing) * 7);
  }
</style>
