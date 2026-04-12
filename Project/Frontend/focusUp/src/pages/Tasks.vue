<script lang="ts" setup>

import NavAuth from '@/components/layout/NavAuth.vue'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import Categories from '@/components/ui/Categories.vue'
import { ref } from 'vue'
import TasksComponent from '@/components/ui/TasksComponent.vue'
import Tag from '@/components/ui/Tag.vue'

// categories logic
const whichIsActive = ref<number>(0);
function changeActiveCategory(value: number) {
  whichIsActive.value = value;
}

const categories = ['Alle Kategorien', 'Lernen', 'Sport', 'Arbeit', 'Kreativ', 'Alltag']
</script>

<template>
  <div class="h-screen flex flex-col overflow-hidden min-h-0">
  <NavAuth name-initials="SS"></NavAuth>
  <main class="flex-1 flex flex-col min-h-0">
    <GreetingsSection title="Meine Tasks" subtitle="Verwalte deine Aufgaben und bleib fokussiert!"></GreetingsSection>

    <div class="grid grid-cols-8 gap-4 flex-1 min-h-0">

      <section class="col-span-6 flex flex-col overflow-auto">

        <!-- search area-->
        <search class="base-element grid grid-cols-[6fr_auto_auto] gap-2">
          <div class="flex items-center justify-start gap-2 bg-[var(--background-color)] px-4 py-2 rounded-lg">
            <i class="fa-solid fa-magnifying-glass text-[var(--text-color-light)]"></i>
            <input class="w-full outline-0" type="text" placeholder="Tasks suchen ..." />
          </div>

          <button class="border border-gray-200 cursor-pointer flex items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--background-color)] px-4 py-2)]">
            <i class="fa-solid fa-layer-group"></i>
            <span>nach Datum</span>
          </button>

          <button class="border border-gray-200 cursor-pointer flex items-center justify-center text-nowrap gap-2 rounded-lg text-[var(--text-color)] bg-[var(--background-color)] px-4 py-2)]">
            <i class="fa-solid fa-filter"></i>
            <span>Filter</span>
          </button>
        </search>

        <!-- Categories-->
        <div class="flex items-center justify-start mt-4 gap-2">
          <Categories v-for="(category, i) in categories" :key="i" :text="category" :isActive="whichIsActive == i" @clicked="changeActiveCategory(i)"></Categories>
        </div>

        <!-- View choosing-->
        <div class="flex items-center justify-evenly mt-4 gap-2 bg-[var(--surface-color)] shadow-lg rounded-xl p-2 text-sm">
          <div class="activeView flex items-center justify-center gap-2 w-full rounded-xl px-4 py-1">
            <span class="">Alle</span>
            <span class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200">28</span>
          </div>
          <div class="inactiveView flex items-center justify-center gap-2 w-full rounded-xl">
            <span class="">Offen</span>
            <span class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200">8</span>
          </div>
          <div class="inactiveView flex items-center justify-center gap-2 w-full rounded-xl">
            <span class="">Erledigt</span>
            <span class="rounded-full px-2 py-0.5 bg-white/10 backdrop-blur-2xl border border-gray-200">20</span>
          </div>
        </div>

        <!-- Tasks-->
        <div class="scrollbar flex flex-col items-center justify-start mt-4 pr-2 gap-3 flex-1 overflow-y-auto overflow-x-hidden">
          <TasksComponent v-for="i in 20" :key="i" task-title="5km Laufen" task-description="Morgenlauf, Pace 5:30/km halten" :timeMin="35" :date="new Date(2026, 2, 25)">
            <Tag name="Sport" color-hex="#84CC16" text-color-hex="#FFFFFF"></Tag>
            <Tag name="Einfach" color-hex="#94a0af" text-color-hex="#FFFFFF"></Tag>
          </TasksComponent>
        </div>
      </section>

      <section class="col-span-2">
        <div class="flex items-center justify-center w-full base-element border-1 border-gray-200">
          <button class="cursor-pointer flex justify-center items-center gap-2 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] w-full px-4 py-2 rounded-lg text-[var(--text-color-white)] text-nowrap font-semibold border border-gray-200 text-md">
            <i class="fa-solid fa-plus"></i>
            <span>Neue Task erstellen</span>
          </button>
        </div>
      </section>
    </div>
  </main>
  </div>
</template>

<style scoped>
  .scrollbar{
    overflow-y: scroll;
    scroll-behavior: smooth;
    scrollbar-width: thin;
  }

  .activeView{
    background-color: var(--primary-color);
    color: var(--text-color-white);
  }

  .inactiveView{
    background-color: var(--surface-color);
    color: var(--text-color);
  }
</style>
