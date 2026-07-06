<script lang="ts" setup>

import Logo from '@/components/ui/Logo.vue'
import SectionStruct from '@/components/ui/Landingpage/SectionStruct.vue'
import Steps from '@/components/ui/Landingpage/Steps.vue'
import Footer from '@/components/layout/Footer.vue'
import Card from '@/components/ui/Landingpage/Card.vue'
</script>

<template>
  <div class="w-full min-h-screen">
    <div class="z-999 fixed top-0 left-0 right-0 w-full bg-[var(--surface-color)] border border-b-[var(--border-color)] border-transparent shadow-xs">
      <nav class="max-w-[1400px] mx-auto px-5 py-4 flex items-center justify-between gap-4">
        <Logo link="/"></Logo>
        <div class="flex items-center justify-between gap-3 text-[var(--text-color-light)] font-medium">
          <a href="#" class="hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2">Funktionen</a>
          <a href="#" class="hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2">So funktioniert's</a>
          <a href="#" class="hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2">Vorteile</a>
        </div>
        <div class="flex items-center justify-between gap-3">
          <RouterLink to="/login" class="text-[var(--text-color-light)] font-medium hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2">Anmelden</RouterLink>
          <RouterLink to="/register" class="hover:text-transparent hover:bg-clip-text hover:border-[var(--primary-color)] transition duration-125 border-2 border-transparent bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] rounded-xl font-bold text-[var(--text-color-white)] px-4 py-2">Registrieren</RouterLink>
        </div>
      </nav>
    </div>

    <header class="mt-[98px] h-screen max-h-[900px] border border-transparent border-b-[var(--border-color)]">
      <div class="mx-auto max-w-[1400px] flex items-center justify-between gap-[20px] p-8">
        <div class="flex flex-col justify-center items-start gap-4">
          <h1 class="text-[55px] font-extrabold text-[var(--text-color)] max-w-[750px]">Bring deine
            <span class="bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] bg-clip-text text-transparent">Produktivität</span>
            aufs nächste Level</h1>
          <p class="text-[var(--text-color-light)] text-lg max-w-[550px]">Verwandle deine täglichen Aufgaben in messbaren Fortschritt. Sammle XP, schalte Level frei und baue Streaks auf, die dich wirklich am Laufen halten.</p>
          <div class="flex items-center justify-between gap-3 mt-3">
            <button class="rounded-xl text-[var(--text-color-white)] text-lg font-semibold px-5 py-2 bg-linear-to-br from-[var(--primary-color)] shadow-lg to-[var(--secondary-color)]">Registrieren</button>
            <button class="bg-[var(--surface-color)] text-[var(--text-color)] text-lg px-5 py-2 rounded-xl border border-[var(--border-color)] shadow-lg font-semibold">Anmelden</button>
          </div>
        </div>

        <div class="min-w-[550px] min-h-[650px] rounded-3xl bg-[var(--surface-color)] border border-[var(--border-color)]">

        </div>
      </div>
    </header>

    <SectionStruct class="bg-[var(--surface-color)]" kicker="So funktioniert FocusUp" subtitle="Wir haben die motivierendsten Elemente aus Spielen wie Fortschritt, Belohnungen und Schwung genommen und auf deinen Alltag übertragen." title="Produktivität trifft Spielmechanik">
      <div class="flex items-start justify-start gap-8 mt-5">
        <div class="mt-5 flex flex-col h-full items-stretch gap-5">
          <div class="flex items-start justify-start gap-5">
            <div class="w-[37px] h-[37px] rounded-xl bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] flex items-center justify-center text-[var(--text-color-white)] font-bold shadow-lg">
              <span>1</span>
            </div>
            <div>
              <span class="font-semibold">Erstelle Aufgaben</span>
              <p class="text-[var(--text-color-light)] max-w-[600px] text-sm">Teile deine Ziele in umsetzbare Aufgaben auf. Jede Aufgabe hat einen Schwierigkeitsgrad, der bestimmt, wie viel XP du erhältst.</p>
            </div>
          </div>

          <div class="flex items-start justify-start gap-5">
            <div class="w-[37px] h-[37px] rounded-xl bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] flex items-center justify-center text-[var(--text-color-white)] font-bold shadow-lg">
              <span>2</span>
            </div>
            <div>
              <span class="font-semibold">Sammle XP und steige auf</span>
              <p class="text-[var(--text-color-light)] max-w-[600px] text-sm">Das Erledigen von Aufgaben belohnt dich mit Erfahrungspunkten. Fülle deine XP-Leiste, um das nächste Level zu erreichen und neue Belohnungen freizuschalten.</p>
            </div>
          </div>

          <div class="flex items-start justify-start gap-5">
            <div class="w-[37px] h-[37px] rounded-xl bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] flex items-center justify-center text-[var(--text-color-white)] font-bold shadow-lg">
              <span>3</span>
            </div>
            <div>
              <span class="font-semibold">Baue unaufhaltsame Streaks auf</span>
              <p class="text-[var(--text-color-light)] max-w-[600px] text-sm">Bleib konsequent und baue tägliche Streaks auf. Verpasst du einen Tag, wird dein Streak zurückgesetzt das hält dich motiviert, jeden Tag dabei zu bleiben.</p>
            </div>
          </div>
        </div>

        <div class="w-[550px] h-[400px] bg-[var(--background-color)] rounded-2xl"></div>
      </div>
    </SectionStruct>

    <SectionStruct title="Alles, was du brauchst, um im Flow zu bleiben" subtitle="Entwickelt für Menschen, die Ergebnisse wollen nicht nur eine weitere To-Do-Liste." kicker="Funktionen" class="p-8">
        <div class="grid grid-cols-3 gap-4 mt-8">
          <Card title="Intelligentes Aufgabenmanagement" description="Organisiere Aufgaben nach Priorität, Kategorie und Schwierigkeit. Setze Deadlines, füge Unteraufgaben hinzu und erlebe, wie deine Produktivität mit einem System wächst, das wirklich zu deiner Denkweise passt." f-point="Prioritätsstufen" s-point="Labels" t-point="Deadlines"></Card>
          <Card title="XP- & Level-System" description="Jede erledigte Aufgabe bringt dir XP basierend auf ihrer Schwierigkeit. Mit steigender XP steigst du im Level auf — und schaltest neue Titel, Abzeichen und eine klare visuelle Darstellung deines Wachstums frei." f-point="Dynamische XP" s-point="Level 1–100" t-point="Badges"></Card>
          <Card title="Streak-System" description="Beständigkeit zahlt sich aus. Erledige mindestens eine Aufgabe pro Tag, um deinen Streak zu halten. Je länger dein Streak, desto höher der XP-Multiplikator — bleib am Ball und sieh, wie sich dein Fortschritt vervielfacht." f-point="Tägliche Streaks" s-point="Multiplikator" t-point="Motivation"></Card>
        </div>
    </SectionStruct>

    <SectionStruct class="bg-[var(--surface-color)]" title="Drei einfache Schritte" kicker="So funktioniert's" subtitle="Starte in wenigen Minuten. Keine komplizierte Einrichtung einfach die App öffnen und losgehen.">
      <div class="relative grid grid-cols-3 gap-4 mt-8">
        <div class="absolute top-6 w-full flex justify-center items-center">
          <div class="mx-auto w-[800px] h-[2px] bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"></div>
        </div>

        <Steps title="Erstelle deine Aufgaben" :number="1" description="Füge Aufgaben mit Namen, Kategorie und Schwierigkeitsgrad hinzu. Teile große Ziele in kleine, umsetzbare Schritte auf.">
          <div class="grid grid-cols-1 grid-rows-20 h-full gap-2">
            <div class="relative px-2 py-1 flex justify-end items-center bg-[var(--surface-color)] border border-[var(--border-color)] rounded-lg w-full row-span-7">
              <div class="flex items-center justify-center text-[var(--text-color-white)] text-sm w-[25px] h-[25px] bg-[var(--primary-color)] rounded-lg">
                <i class="fa-solid fa-plus"></i>
              </div>
              <span class="absolute inset-0 text-center flex items-center justify-center h-full text-[var(--text-color-light)] text-sm">Neue Aufgabe hinzufügen...</span>
            </div>

            <div class="px-2 py-1 flex justify-between items-center bg-[var(--surface-color)] border border-[var(--border-color)] rounded-lg w-full row-span-6">
              <div class="w-[15px] h-[15px] border border-[var(--border-color)] rounded-sm"></div>
              <span class="text-[var(--text-color)] text-sm">Morgen-Workout</span>
              <span class="text-[var(--text-color-light)] text-xs">+50 XP</span>
            </div>

            <div class="px-2 py-1 flex justify-between items-center bg-[var(--surface-color)] border border-[var(--border-color)] rounded-lg w-full row-span-6">
              <div class="w-[15px] h-[15px] border border-[var(--border-color)] rounded-sm"></div>
              <span class="text-[var(--text-color)] text-sm">Fokus-Arbeitsblock</span>
              <span class="text-[var(--text-color-light)] text-xs">+80 XP</span>
            </div>
          </div>
        </Steps>
        <Steps title="Erledige deine Aufgaben" :number="2" description="Hake Aufgaben ab, sobald du sie erledigt hast. Jede Erledigung wird erfasst und deine XP in Echtzeit aktualisiert.">
          <div class="grid grid-cols-1 grid-rows-20 h-full gap-2">
            <div class="px-2 py-1 flex justify-between items-center bg-[var(--primary-color-light)] border border-[var(--primary-color)] rounded-lg w-full row-span-6">
              <div class="w-[15px] h-[15px] bg-[var(--primary-color)] rounded-full flex items-center justify-center text-[var(--text-color-white)] text-[8px]">
                <i class="fa-solid fa-check -translate-x-[1px]"></i>
              </div>
              <span class="text-[var(--primary-color)] text-sm line-through">Morgen-Workout</span>
              <span class="text-[var(--primary-color)] text-xs font-bold">+50 XP</span>
            </div>

            <div class="px-2 py-1 flex justify-between items-center bg-[var(--primary-color-light)] border border-[var(--primary-color)] rounded-lg w-full row-span-6">
              <div class="w-[15px] h-[15px] bg-[var(--primary-color)] rounded-full flex items-center justify-center text-[var(--text-color-white)] text-[8px]">
                <i class="fa-solid fa-check -translate-x-[1px]"></i>
              </div>
              <span class="text-[var(--primary-color)] text-sm line-through">Fokus-Arbeitsblock</span>
              <span class="text-[var(--primary-color)] text-xs font-bold">+80 XP</span>
            </div>

            <div class="w-full row-span-7 bg-[var(--secondary-color-light)] shadow-sm font-bold text-[var(--secondary-color)] rounded-lg flex items-center justify-center gap-2 text-sm">
              <div class="flex items-center justify-center text-xs">
                <i class="fa-solid fa-bolt"></i>
              </div>
              <span>+130 XP heute gesammelt</span>
            </div>
          </div>
        </Steps>
        <Steps title="Sammle XP & steige auf" :number="3" description="Beobachte, wie sich deine Erfahrungsleiste füllt. Erreiche Meilensteine, steige auf und halte deinen Streak Tag für Tag am Leben.">
          <div class="w-full h-full bg-[var(--text-color)] flex flex-col items-center justify-center gap-2 rounded-xl px-3 py-2">
           <div class="w-[45px] h-[45px] flex items-center justify-center rounded-full bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] text-md">
             <i class="fa-regular fa-star"></i>
           </div>

            <span class="uppercase font-bold text-[var(--text-color-light)] text-xs">Level Up</span>
            <span class="font-bold text-[var(--text-color-white)] text-2xl">Level 15</span>

            <div class="w-full h-[6px] rounded-full level-bg">
              <div class="w-[120px] h-full rounded-full bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"></div>
            </div>
          </div>
        </Steps>
      </div>
    </SectionStruct>

    <section class="w-full h-[600px] px-8 py-7 bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] flex flex-col items-center justify-center gap-2">
      <h2 class="text-[75px] text-[var(--text-color-white)] font-extrabold">Starte deine Reise heute.</h2>
      <span class="text-[var(--text-color-white)] text-lg max-w-[700px] text-center">Hör auf, Produktivität nur zu planen. Erstelle dein Konto in 30 Sekunden und erledige deine erste Aufgabe noch heute.</span>
      <div class="flex items-center justify-center gap-4 mt-5">
        <RouterLink to="/register" class="hover:border-[var(--text-color)] hover:shadow-2xl transition duration-100 border-2 border-transparent px-7 py-3 rounded-xl shadow-sm font-bold bg-[var(--surface-color)] flex items-center justify-center gap-2">
          <div class="flex items-center justify-center">
            <i class="fa-solid fa-bolt"></i>
          </div>
          <span>Konto jetzt erstellen</span>
        </RouterLink>
        <RouterLink to="/register" class="hover:border-[var(--text-color)] hover:shadow-2xl transition duration-100 border-2 border-transparent px-7 py-3 rounded-xl shadow-sm font-bold bg-[var(--surface-color)] flex items-center justify-center gap-2">
          <div class="flex items-center justify-center">
            <i class="fa-solid fa-arrow-right-to-bracket"></i>
          </div>
          <span>Anmelden</span>
        </RouterLink>
      </div>
    </section>

    <Footer></Footer>
  </div>
</template>

<style scoped>
  .level-bg{
    background: rgba(255, 255, 255, .1);
  }
</style>
