<script lang="ts" setup xmlns="http://www.w3.org/1999/html">
import Logo from '@/components/ui/Logo.vue'
import SectionStruct from '@/components/ui/Landingpage/SectionStruct.vue'
import Steps from '@/components/ui/Landingpage/Steps.vue'
import Footer from '@/components/layout/Footer.vue'
import Card from '@/components/ui/Landingpage/Card.vue'
import { computed, onMounted, onUnmounted, ref } from 'vue'
import { useElementVisible } from '@/utils/elementVisible.ts'
import { applyUIMode, setUIMode } from '@/utils/modeUI.ts'

const prefersDarkMode = window.matchMedia('(prefers-color-scheme: dark)').matches

const isVisible = ref<boolean>(false)
const isAtNextSection = ref(false)

const titleWords = [
  { text: 'Bring' },
  { text: 'deine' },
  { text: 'Produktivität', gradient: true },
  { text: 'aufs' },
  { text: 'nächste' },
  { text: 'Level' },
]

const descriptionLines = [
  { text: 'Verwandle deine täglichen Aufgaben in messbaren Fortschritt.' },
  { text: 'Sammle XP, schalte Level frei und baue Streaks auf, die dich wirklich' },
  { text: 'am Laufen halten.' },
]

const functionSectionText = [
  {
    title: 'Erstelle Aufgaben',
    description:
      'Teile deine Ziele in umsetzbare Aufgaben auf. Jede Aufgabe hat einen Schwierigkeitsgrad, der bestimmt, wie viel XP du erhältst.',
  },
  {
    title: 'Sammle XP und steige auf',
    description:
      'Das Erledigen von Aufgaben belohnt dich mit Erfahrungspunkten. Fülle deine XP-Leiste, um das nächste Level zu erreichen und neue Belohnungen freizuschalten.',
  },
  {
    title: 'Baue unaufhaltsame Streaks auf',
    description:
      'Bleib konsequent und baue tägliche Streaks auf. Verpasst du einen Tag, wird dein Streak zurückgesetzt das hält dich motiviert, jeden Tag dabei zu bleiben.',
  },
]

const handleScroll = () => {
  const nextSection = document.querySelector('.next-section')
  if (!nextSection) return

  isAtNextSection.value = window.scrollY >= nextSection.offsetTop - 80
}

const effects = ref([])
let intervalId = null

function randomItem(items) {
  return items[Math.floor(Math.random() * items.length)]
}

function spawnEffect() {
  const type = randomItem(['xp', 'xp', 'xp', 'xp', 'xp', 'xp', 'check', 'streak', 'badge'])

  const effect = {
    id: Date.now() + Math.random(),
    type,
    left: Math.floor(Math.random() * 90) + 5,
    duration: Math.random() * 5 + 6,
    class: 'text-teal-500',
  }

  if (type === 'xp') {
    effect.amount = randomItem([10, 25, 50])
    effect.class = randomItem(['text-teal-500', 'text-sky-500', 'text-lime-500'])
  }

  if (type === 'check') {
    effect.class = 'text-white bg-emerald-400/80 text-xl'
  }

  if (type === 'streak') {
    effect.days = randomItem([3, 7, 14, 30])
    effect.class = 'text-orange-500'
  }

  if (type === 'badge') {
    effect.class = 'text-yellow-500'
  }

  effects.value.push(effect)

  setTimeout(() => {
    effects.value = effects.value.filter((item) => item.id !== effect.id)
  }, effect.duration * 1000)
}

const tasks = [
  ['Mathe lernen', 25],
  ['Englisch-Vokabeln üben', 10],
  ['Präsentation vorbereiten', 25],
  ['Hausaufgaben erledigen', 25],
  ['Lernzettel erstellen', 25],
  ['Für eine Prüfung lernen', 50],
  ['Zusammenfassung schreiben', 25],
  ['Projektarbeit fortsetzen', 50],
  ['E-Mails beantworten', 10],
  ['Stundenplan organisieren', 10],

  ['Inbox aufräumen', 10],
  ['Tagesplanung erstellen', 10],
  ['Fokus-Session abschliessen', 25],
  ['Arbeitsplatz aufräumen', 10],
  ['Tagesziele festlegen', 10],
  ['Kalender aktualisieren', 10],
  ['Notizen sortieren', 10],
  ['Dokumente organisieren', 25],

  ['30 Minuten Sport machen', 25],
  ['10 Minuten meditieren', 10],
  ['Spazieren gehen', 10],
  ['Genug Wasser trinken', 10],
  ['Dehnübungen machen', 10],
  ['Workout abschliessen', 50],

  ['Zimmer aufräumen', 25],
  ['Wäsche waschen', 25],
  ['Einkaufen gehen', 25],
  ['Rechnung bezahlen', 10],
  ['Freunde anrufen', 10],
  ['Pflanzen giessen', 10],
  ['Schreibtisch aufräumen', 10],
]

const cardCount = 3

const currTasks = ref([randomTask(), randomTask(), randomTask()])

const swapping = ref([false, false, false, false])
const popping = ref([false, false, false, false])

const timeoutIds = []

function randomTask() {
  return tasks[Math.floor(Math.random() * tasks.length)]
}

function randomDelay() {
  return Math.random() * 8000 + 4000 // 4–8 Sekunden
}

function changeTask(index) {
  swapping.value[index] = true

  setTimeout(() => {
    currTasks.value[index] = randomTask()
    swapping.value[index] = false

    popping.value[index] = true

    setTimeout(() => {
      popping.value[index] = false
    }, 600)
  }, 300)
}

function changeTaskLoop(index) {
  changeTask(index)

  timeoutIds[index] = setTimeout(() => {
    changeTaskLoop(index)
  }, randomDelay())
}

let randomUiHeight = ref<number[]>([])

function getRandomNumber(min: number, max: number) {
  return Math.floor(Math.random() * (max - min)) + min
}

function updateRandomHeight() {
  randomUiHeight.value = Array.from({ length: 7 }, () => getRandomNumber(15, 100))
}

let intervalRandomUIId = null
onMounted(() => {
  setUIMode(prefersDarkMode)
  applyUIMode()

  isVisible.value = true

  updateRandomHeight()
  intervalRandomUIId = setInterval(updateRandomHeight, 2000)

  window.addEventListener('scroll', handleScroll)
  intervalId = setInterval(spawnEffect, 500)
  handleScroll()

  for (let i = 0; i < cardCount; i++) {
    timeoutIds[i] = setTimeout(() => {
      changeTaskLoop(i)
    }, randomDelay())
  }
})

const sectionFunctionElement = ref<HTMLElement | null>(null)
const sectionProcessElement = ref<HTMLElement | null>(null)
const sectionCTAElement = ref<HTMLElement | null>(null)

const { isVisible: isVisibleFunctionSection } = useElementVisible(sectionFunctionElement)
const { isVisible: isVisibleProcessSection } = useElementVisible(sectionProcessElement)
const { isVisible: isVisibleCTASection } = useElementVisible(sectionCTAElement)

onUnmounted(() => {
  window.removeEventListener('scroll', handleScroll)
  clearInterval(intervalId)
  clearInterval(intervalRandomUIId)

  timeoutIds.forEach((id) => clearTimeout(id))
})
</script>

<template>
  <div class="w-full min-h-screen overflow-x-hidden">
    <div
      :class="[
        isAtNextSection
          ? 'top-0 left-0 right-0 rounded-none max-w-full mx-0'
          : 'top-5 left-5 right-5 rounded-3xl max-w-[1400px] mx-auto',
        !isVisible ? 'opacity-0 scale-95' : 'opacity-100 scale-100',
      ]"
      class="hidden md:block z-999 fixed bg-white/60 backdrop-blur-md border border-b-[var(--border-color)] border-transparent shadow-xs min-h-[50px] transition-all duration-500 ease-in-out"
    >
      <nav class="max-w-[1400px] mx-auto px-5 py-4 flex items-center justify-between gap-4">
        <Logo class="bg-transparent" link="/"></Logo>
        <div
          class="flex items-center justify-between gap-3 text-[var(--text-color-light)] font-medium"
        >
          <a
            href="#prinzip"
            class="hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2 text-nowrap"
            >So funktioniert's</a
          >
          <a
            href="#funktionen"
            class="hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2 text-nowrap"
            >Funktionen</a
          >
          <a
            href="#ablauf"
            class="hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2 text-nowrap"
            >Ablauf</a
          >
        </div>
        <div class="flex items-center justify-between gap-3">
          <RouterLink
            to="/login"
            class="text-[var(--text-color-light)] font-medium hover:bg-[var(--background-color)] hover:text-[var(--text-color)] transition duration-125 rounded-lg px-4 py-2"
            >Anmelden</RouterLink
          >
          <RouterLink
            to="/register"
            class="hover:text-transparent hover:bg-clip-text hover:border-[var(--primary-color)] transition duration-125 border-2 border-transparent bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] rounded-xl font-bold text-[var(--text-color-white)] px-4 py-2"
            >Registrieren</RouterLink
          >
        </div>
      </nav>
    </div>

    <header
      class="relative z-20 pt-[150px] h-screen border border-transparent border-b-[var(--border-color)] flex flex-col justify-between overflow-hidden bg-gradient-to-b from-[var(--surface-color)] via-[var(--primary-color-light)] to-[var(--secondary-color)]"
    >
      <div
        class="pointer-events-none absolute left-1/2 bottom-[-172rem] z-5 h-[200rem] w-[200rem] -translate-x-1/2 rounded-full bg-[radial-gradient(closest-side_at_50%_0%,rgba(255,255,255,0.95),rgba(224,242,254,0.9)_30%,rgba(191,219,254,0.75)_60%,rgba(165,199,246,0.6))] shadow-[0_-30px_90px_rgba(125,211,252,0.55),0_-6px_24px_rgba(255,255,255,0.9),inset_0_40px_80px_rgba(255,255,255,0.9)]"
      ></div>

      <div class="absolute inset-0 overflow-hidden pointer-events-none z-10">
        <span
          v-for="effect in effects"
          :key="effect.id"
          class="absolute bottom-[-80px] rounded-full bg-white/70 shadow-lg backdrop-blur-md animate-float-up flex items-center gap-2 font-bold text-nowrap"
          :class="[
            effect.class,
            effect.type === 'check' ? 'h-10 w-10 justify-center p-0' : 'px-5 py-2',
          ]"
          :style="{
            left: effect.left + '%',
            animationDuration: effect.duration + 's',
          }"
        >
          <span v-if="effect.type === 'xp'"> +{{ effect.amount }} XP </span>

          <span v-else-if="effect.type === 'check'">
            <div class="flex items-center justify-center text-[var(--primary-color)]">
              <i class="fa-solid fa-check"></i>
            </div>
          </span>

          <span v-else-if="effect.type === 'streak'" class="flex items-center justify-center gap-2">
            <div class="flex items-center justify-between">
              <i class="fa-solid fa-fire-flame-curved"></i>
            </div>
            {{ effect.days }} Tage
          </span>

          <span v-else-if="effect.type === 'badge'" class="flex items-center justify-center gap-2">
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-award"></i>
            </div>
            Badge
          </span>
        </span>
      </div>

      <div class="mx-auto max-w-[1400px] flex items-center justify-center gap-[20px] p-8">
        <div class="flex flex-col justify-center items-center gap-4">
          <h1
            class="z-15 sm:text-[75px] text-[60px] font-extrabold leading-19 text-[var(--text-color)] text-center max-w-[750px]"
          >
            <span v-for="(word, index) in titleWords" :key="index" class="mr-[0.75rem]">
              <span
                class="inline-block transition-all duration-700 ease-in-out"
                :style="{ transitionDelay: `${index * 100}ms` }"
                :class="[
                  isVisible
                    ? 'translate-0 rotate-0 opacity-100'
                    : 'translate-y-[30px] rotate-3 opacity-0',
                  word.gradient
                    ? 'bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)] bg-clip-text text-transparent'
                    : 'text-[var(--text-color)]',
                ]"
              >
                {{ word.text }}
              </span>
            </span>
          </h1>

          <p
            v-for="(line, index) in descriptionLines"
            :key="index"
            class="leading-4 lg:text-[var(--text-color-light)] text-[var(--text-color)] z-15 sm:z-0 text-lg max-w-[550px] text-center"
          >
            <span
              :style="{ transitionDelay: `${index * 100 + 1000}ms` }"
              :class="!isVisible ? 'translate-y-full opacity-0' : 'translate-0 opacity-100'"
              class="inline-block overflow-hidden transition-all duration-800 ease-in-out"
            >
              {{ line.text }}
            </span>
          </p>
          <div
            class="flex sm:flex-row flex-col w-full sm:w-fit items-center justify-between gap-5 mt-3"
          >
            <div
              class="transition-all duration-700 delay-100"
              :class="
                !isVisible
                  ? 'opacity-0 scale-95 translate-y-[20px]'
                  : 'opacity-100 scale-100 translate-0'
              "
            >
              <RouterLink
                to="/register"
                class="w-full sm:w-fit z-15 hover:from-[var(--surface-color)] hover:to-[var(--surface-color)] hover:border-[var(--primary-color)] hover:shadow-xl hover:text-[var(--primary-color)] transition duration-100 border border-transparent flex items-center justify-center gap-2 rounded-2xl text-[var(--text-color-white)] text-lg font-semibold px-5 py-4 bg-linear-to-br from-[var(--primary-color)] shadow-lg to-[var(--secondary-color)]"
              >
                <div class="flex items-center justify-center">
                  <i class="fa-solid fa-bolt"></i>
                </div>
                <span class="text-nowrap">Konto jetzt erstellen</span>
              </RouterLink>
            </div>
            <div
              class="transition-all duration-700 delay-150"
              :class="
                !isVisible
                  ? 'opacity-0 scale-95 translate-y-[20px]'
                  : 'opacity-100 scale-100 translate-0'
              "
            >
              <RouterLink
                to="/login"
                class="w-full sm:w-fit z-15 hover:shadow-xl hover:border-[var(--text-color)] transition duration-100 flex items-center justify-center gap-2 bg-[var(--surface-color)] text-[var(--text-color)] text-lg px-5 py-4 rounded-2xl border border-[var(--border-color)] shadow-lg font-semibold"
              >
                <div class="flex items-center justify-center">
                  <i class="fa-solid fa-arrow-right-to-bracket"></i>
                </div>
                <span>Anmelden</span>
              </RouterLink>
            </div>
          </div>
        </div>
      </div>

      <div
        class="hidden xl:grid mx-auto w-full max-w-[1400px] px-5 py-15 grid grid-cols-3 overflow-hidden"
      >
        <div
          v-for="(currTask, index) in currTasks"
          :key="index"
          class="task-card transition-all z-20 flex w-fit min-w-[0] xl:min-w-[400px] items-center justify-center gap-5 rounded-2xl bg-[var(--surface-color)] text-nowrap p-6 border border-[var(--border-color)] shadow-lg"
          :style="{ transitionDuration: `${index * 800 + 1000}ms` }"
          :class="{
            'swap-out': swapping[index],
            pop: popping[index],
            'show-up-off': !isVisible,
            'show-up-on': isVisible,
          }"
        >
          <div
            class="task-check flex items-center justify-center min-w-[25px] min-h-[25px] text-xs text-[var(--text-color-white)] bg-[var(--primary-color)] rounded-full shadow-sm"
          >
            <i class="fa-solid fa-check"></i>
          </div>
          <span class="task-title font-bold text-md">{{ currTask![0] }}</span>
          <span class="task-xp text-[var(--primary-color)] font-bold text-sm"
            >+{{ currTask![1] }} XP</span
          >
        </div>
      </div>
    </header>

    <div ref="sectionFunctionElement">
      <SectionStruct
        id="prinzip"
        class="next-section bg-[var(--surface-color)]"
        kicker="So funktioniert FocusUp"
        subtitle="Wir haben die motivierendsten Elemente aus Spielen wie Fortschritt, Belohnungen und Schwung genommen und auf deinen Alltag übertragen."
        title="Produktivität trifft Spielmechanik"
      >
        <div class="flex items-start justify-start gap-8 mt-5">
          <div class="mt-5 flex flex-col h-full items-stretch gap-5">
            <div
              v-for="(func, index) in functionSectionText"
              :key="index"
              :class="!isVisibleFunctionSection ? 'show-up-off-xl' : 'show-up-on' "
              :style="{ transitionDelay: `${index * 400}ms` }"
              class="transition-all duration-500 flex items-start justify-start gap-5"
            >
              <div
                class="min-w-[37px] min-h-[37px] rounded-xl bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] flex items-center justify-center text-[var(--text-color-white)] font-bold shadow-lg"
              >
                <span>{{ index + 1 }}</span>
              </div>
              <div>
                <span class="font-bold text-xl">{{ func.title }}</span>
                <p class="text-[var(--text-color-light)] max-w-[600px] text-lg">
                  {{ func.description }}
                </p>
              </div>
            </div>
          </div>

          <div
            :class="!isVisibleFunctionSection ? 'show-up-off-xl' : 'show-up-on' "
            :style="{ transitionDelay: '1200ms' }"
            class="transition-all duration-700 flex-col select-none hidden xl:flex w-[550px] h-[400px] bg-[var(--background-color)] rounded-2xl shadow-lg border border-[var(--border-color)] p-5"
          >
            <span class="uppercase font-bold text-[var(--text-color-light)] text-xs"
              >Wöchentliche XP-Übersicht</span
            >

            <div class="grid grid-cols-7 min-h-[100px] h-[100px] gap-2 w-full items-end mt-5">
              <div class="flex flex-col gap-1 text-center justify-end h-full">
                <div
                  :style="{ height: `${randomUiHeight[0]}%` }"
                  class="col-span-1 rounded-t-lg bg-[var(--secondary-color)] shadow-sm transition-all duration-500 ease-in-out"
                ></div>
                <span class="text-xs text-[var(--text-color-light)]">Mo</span>
              </div>

              <div class="flex flex-col gap-1 text-center justify-end h-full">
                <div
                  :style="{ height: `${randomUiHeight[1]}%` }"
                  class="col-span-1 rounded-t-lg bg-[var(--secondary-color)] shadow-sm transition-all duration-500 ease-in-out"
                ></div>
                <span class="text-xs text-[var(--text-color-light)]">Di</span>
              </div>

              <div class="flex flex-col gap-1 text-center justify-end h-full">
                <div
                  :style="{ height: `${randomUiHeight[2]}%` }"
                  class="col-span-1 rounded-t-lg bg-[var(--secondary-color)] shadow-sm transition-all duration-500 ease-in-out"
                ></div>
                <span class="text-xs text-[var(--text-color-light)]">Mi</span>
              </div>

              <div class="flex flex-col gap-1 text-center justify-end h-full">
                <div
                  :style="{ height: `${randomUiHeight[3]}%` }"
                  class="col-span-1 rounded-t-lg bg-[var(--secondary-color)] shadow-sm transition-all duration-500 ease-in-out"
                ></div>
                <span class="text-xs text-[var(--text-color-light)]">Do</span>
              </div>

              <div class="flex flex-col gap-1 text-center justify-end h-full">
                <div
                  :style="{ height: `${randomUiHeight[4]}%` }"
                  class="col-span-1 rounded-t-lg bg-[var(--primary-color)] shadow-sm transition-all duration-500 ease-in-out"
                ></div>
                <span class="text-xs text-[var(--text-color-light)]">Fr</span>
              </div>

              <div class="flex flex-col gap-1 text-center justify-end h-full">
                <div
                  class="col-span-1 h-[5%] rounded-t-lg bg-[var(--secondary-color-light)] shadow-sm"
                ></div>
                <span class="text-xs text-[var(--text-color-light)]">Sa</span>
              </div>

              <div class="flex flex-col gap-1 text-center justify-end h-full">
                <div
                  class="col-span-1 h-[5%] rounded-t-lg bg-[var(--secondary-color-light)] shadow-sm"
                ></div>
                <span class="text-xs text-[var(--text-color-light)]">So</span>
              </div>
            </div>

            <div class="grid grid-cols-2 grid-rows-2 w-full flex-1 mt-4 gap-3">
              <div
                class="p-4 flex flex-col justify-between items-start col-span-1 row-span-1 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl"
              >
                <span class="uppercase text-[var(--text-color-light)] text-xs">Gesamt-Xp</span>
                <span class="uppercase text-[var(--text-color)] text-3xl font-extrabold">3.420</span>
                <div
                  class="flex items-center justify-center text-[var(--primary-color)] text-xs font-bold"
                >
                  <div class="flex items-center justify-center text-[10px]">
                    <i class="fa-solid fa-arrow-up"></i>
                  </div>
                  <span>+14% diese Woche</span>
                </div>
              </div>

              <div
                class="p-4 flex flex-col justify-between items-start col-span-1 row-span-1 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl"
              >
                <span class="uppercase text-[var(--text-color-light)] text-xs"
                  >Erledigte Aufgaben</span
                >
                <span class="uppercase text-[var(--text-color)] text-3xl font-extrabold">42</span>
                <div
                  class="flex items-center justify-center text-[var(--primary-color)] text-xs font-bold"
                >
                  <div class="flex items-center justify-center text-[10px]">
                    <i class="fa-solid fa-arrow-up"></i>
                  </div>
                  <span>+6 mehr als letzte Woche</span>
                </div>
              </div>

              <div
                class="p-4 flex flex-col justify-between items-start col-span-1 row-span-1 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl"
              >
                <span class="uppercase text-[var(--text-color-light)] text-xs">Aktuelles Level</span>
                <span class="uppercase text-[var(--text-color)] text-3xl font-extrabold">14</span>
                <div
                  class="flex items-center justify-center text-[var(--primary-color)] text-xs font-bold"
                >
                  <span>68% bis Level 21</span>
                </div>
              </div>

              <div
                class="p-4 flex flex-col justify-between items-start col-span-1 row-span-1 bg-[var(--surface-color)] border border-[var(--border-color)] rounded-xl"
              >
                <span class="uppercase text-[var(--text-color-light)] text-xs">Bester Streak</span>
                <span class="uppercase text-[var(--text-color)] text-3xl font-extrabold">17</span>
                <div
                  class="gap-0.5 flex items-center justify-center text-[var(--primary-color)] text-xs font-bold"
                >
                  <div class="flex items-center justify-center text-[10px]">
                    <i class="fa-solid fa-trophy"></i>
                  </div>
                  <span>Persönlicher Rekord</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </SectionStruct>
    </div>

    <SectionStruct
      id="funktionen"
      title="Alles, was du brauchst, um im Flow zu bleiben"
      subtitle="Entwickelt für Menschen, die Ergebnisse wollen nicht nur eine weitere To-Do-Liste."
      kicker="Funktionen"
      class="p-8"
    >
      <div class="grid xl:grid-cols-3 md:grid-cols-2 grid-cols-1 gap-4 mt-8">
        <Card
          svg="fa-regular fa-clipboard"
          title="Intelligentes Aufgabenmanagement"
          description="Organisiere Aufgaben nach Priorität, Kategorie und Schwierigkeit. Setze Deadlines, füge Unteraufgaben hinzu und erlebe, wie deine Produktivität mit einem System wächst, das wirklich zu deiner Denkweise passt."
          f-point="Prioritätsstufen"
          s-point="Labels"
          t-point="Deadlines"
        ></Card>
        <Card
          svg="fa-regular fa-star"
          title="XP- & Level-System"
          description="Jede erledigte Aufgabe bringt dir XP basierend auf ihrer Schwierigkeit. Mit steigender XP steigst du im Level auf und schaltest neue Titel, Abzeichen und eine klare visuelle Darstellung deines Wachstums frei."
          f-point="Dynamische XP"
          s-point="Level 1–100"
          t-point="Badges"
        ></Card>
        <Card
          svg="fa-solid fa-fire-flame-curved"
          title="Streak-System"
          description="Beständigkeit zahlt sich aus. Erledige mindestens eine Aufgabe pro Tag, um deinen Streak zu halten. Je länger dein Streak, desto höher der XP-Multiplikator bleib am Ball und sieh, wie sich dein Fortschritt vervielfacht."
          f-point="Tägliche Streaks"
          s-point="Multiplikator"
          t-point="Motivation"
        ></Card>
      </div>
    </SectionStruct>

    <div ref="sectionProcessElement">
      <SectionStruct
        id="ablauf"
        class="bg-[var(--surface-color)]"
        title="Drei einfache Schritte"
        kicker="Ablauf"
        subtitle="Starte in wenigen Minuten. Keine komplizierte Einrichtung einfach die Website öffnen und losgehen."
      >
        <div class="relative grid xl:grid-cols-3 md:grid-cols-2 grid-cols-1 gap-x-4 gap-y-8 mt-8">
          <div
            :class="!isVisibleProcessSection ? 'left-[  0%] right-[100%]' : 'left-[16.66%] right-[16.66%]' "
            class="transition-all duration-1000 ease-in-out hidden xl:block absolute top-6 left-[16.66%] right-[16.66%] flex justify-center items-center"
          >
            <div
              class="mx-auto w-full h-[2px] bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"
            ></div>
          </div>

          <Steps
            title="Erstelle deine Aufgaben"
            :number="1"
            description="Füge Aufgaben mit Namen, Kategorie und Schwierigkeitsgrad hinzu. Teile große Ziele in kleine, umsetzbare Schritte auf."
          >
            <div class="grid grid-cols-1 grid-rows-20 h-full gap-2">
              <div
                class="relative px-2 py-1 flex justify-end items-center bg-[var(--surface-color)] border border-[var(--border-color)] rounded-lg w-full row-span-7"
              >
                <div
                  class="flex items-center justify-center text-[var(--text-color-white)] text-sm w-[25px] h-[25px] bg-[var(--primary-color)] rounded-lg"
                >
                  <i class="fa-solid fa-plus"></i>
                </div>
                <span
                  class="absolute inset-0 text-center flex items-center justify-center h-full text-[var(--text-color-light)] text-sm"
                  >Neue Aufgabe hinzufügen...</span
                >
              </div>

              <div
                class="px-2 py-1 flex justify-between items-center bg-[var(--surface-color)] border border-[var(--border-color)] rounded-lg w-full row-span-6"
              >
                <div class="w-[15px] h-[15px] border border-[var(--border-color)] rounded-sm"></div>
                <span class="text-[var(--text-color)] text-sm">Morgen-Workout</span>
                <span class="text-[var(--text-color-light)] text-xs">+50 XP</span>
              </div>

              <div
                class="px-2 py-1 flex justify-between items-center bg-[var(--surface-color)] border border-[var(--border-color)] rounded-lg w-full row-span-6"
              >
                <div class="w-[15px] h-[15px] border border-[var(--border-color)] rounded-sm"></div>
                <span class="text-[var(--text-color)] text-sm">Fokus-Arbeitsblock</span>
                <span class="text-[var(--text-color-light)] text-xs">+80 XP</span>
              </div>
            </div>
          </Steps>
          <Steps
            title="Erledige deine Aufgaben"
            :number="2"
            description="Hake Aufgaben ab, sobald du sie erledigt hast. Jede Erledigung wird erfasst und deine XP in Echtzeit aktualisiert."
          >
            <div class="grid grid-cols-1 grid-rows-20 h-full gap-2">
              <div
                class="px-2 py-1 flex justify-between items-center bg-[var(--primary-color-light)] border border-[var(--primary-color)] rounded-lg w-full row-span-6"
              >
                <div
                  class="w-[15px] h-[15px] bg-[var(--primary-color)] rounded-full flex items-center justify-center text-[var(--text-color-white)] text-[8px]"
                >
                  <i class="fa-solid fa-check -translate-x-[1px]"></i>
                </div>
                <span class="text-[var(--primary-color)] text-sm line-through">Morgen-Workout</span>
                <span class="text-[var(--primary-color)] text-xs font-bold">+50 XP</span>
              </div>

              <div
                class="px-2 py-1 flex justify-between items-center bg-[var(--primary-color-light)] border border-[var(--primary-color)] rounded-lg w-full row-span-6"
              >
                <div
                  class="w-[15px] h-[15px] bg-[var(--primary-color)] rounded-full flex items-center justify-center text-[var(--text-color-white)] text-[8px]"
                >
                  <i class="fa-solid fa-check -translate-x-[1px]"></i>
                </div>
                <span class="text-[var(--primary-color)] text-sm line-through"
                  >Fokus-Arbeitsblock</span
                >
                <span class="text-[var(--primary-color)] text-xs font-bold">+80 XP</span>
              </div>

              <div
                class="w-full row-span-7 bg-[var(--secondary-color-light)] shadow-sm font-bold text-[var(--secondary-color)] rounded-lg flex items-center justify-center gap-2 text-sm"
              >
                <div class="flex items-center justify-center text-xs">
                  <i class="fa-solid fa-bolt"></i>
                </div>
                <span>+130 XP heute gesammelt</span>
              </div>
            </div>
          </Steps>
          <Steps
            title="Sammle XP & steige auf"
            :number="3"
            description="Beobachte, wie sich deine Erfahrungsleiste füllt. Erreiche Meilensteine, steige auf und halte deinen Streak Tag für Tag am Leben."
          >
            <div
              class="w-full h-full bg-[#0F172A] flex flex-col items-center justify-center gap-2 rounded-xl px-3 py-2"
            >
              <div
                class="w-[45px] h-[45px] flex items-center justify-center rounded-full bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--text-color-white)] text-md"
              >
                <i class="fa-regular fa-star"></i>
              </div>

              <span class="uppercase font-bold text-[var(--text-color-light)] text-xs">Level Up</span>
              <span class="font-bold text-[var(--text-color-white)] text-2xl">Level 15</span>

              <div class="w-full h-[6px] rounded-full level-bg">
                <div
                  class="w-[120px] h-full rounded-full bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"
                ></div>
              </div>
            </div>
          </Steps>
        </div>
      </SectionStruct>
    </div>

    <section
      ref="sectionCTAElement"
      class="w-full h-[600px] px-8 py-7 bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] flex flex-col items-center justify-center gap-2"
    >
      <h2
        :class="!isVisibleCTASection ? 'show-up-off-xl' : 'show-up-on' "
        class="transition-all duration-700 lg:text-[75px] text-[55px] text-center text-[var(--text-color-white)] font-extrabold"
      >
        Starte deine Reise heute.
      </h2>
      <span
        :class="!isVisibleCTASection ? 'show-up-off-xl' : 'show-up-on' "
        class="transition-all duration-700 delay-400 text-[var(--text-color-white)] text-lg xl:max-w-[700px] max-w-[500px] text-center"
        >Hör auf, Produktivität nur zu planen. Erstelle dein Konto in 30 Sekunden und erledige deine
        erste Aufgabe noch heute.</span
      >
      <div class="flex sm:flex-row flex-col w-full sm:w-fit items-center justify-center gap-4 mt-5">
        <div :class="!isVisibleCTASection ? 'show-up-off-xl' : 'show-up-on' " class="transition-all duration-700 delay-200">
          <RouterLink
            to="/register"
            class="w-full sm:w-fit hover:border-[var(--text-color)] hover:shadow-2xl transition duration-100 border-2 border-transparent px-7 py-3 rounded-xl shadow-sm font-bold bg-[var(--surface-color)] flex items-center justify-center gap-2"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-bolt"></i>
            </div>
            <span class="text-nowrap">Konto jetzt erstellen</span>
          </RouterLink>
        </div>
        <div :class="!isVisibleCTASection ? 'show-up-off-xl' : 'show-up-on' " class="transition-all duration-700 delay-200">
          <RouterLink
            to="/login"
            class="w-full sm:w-fit hover:border-[var(--text-color)] hover:shadow-2xl transition duration-100 border-2 border-transparent px-7 py-3 rounded-xl shadow-sm font-bold bg-[var(--surface-color)] flex items-center justify-center gap-2"
          >
            <div class="flex items-center justify-center">
              <i class="fa-solid fa-arrow-right-to-bracket"></i>
            </div>
            <span>Anmelden</span>
          </RouterLink>
        </div>
      </div>
    </section>

    <Footer></Footer>
  </div>
</template>

<style scoped>
.level-bg {
  background: rgba(255, 255, 255, 0.1);
}

@keyframes float-up {
  0% {
    transform: translateY(0) translateX(0) rotate(-4deg);
    opacity: 0;
  }

  10% {
    opacity: 1;
  }

  80% {
    opacity: 1;
  }

  100% {
    transform: translateY(-110vh) translateX(35px) rotate(4deg);
    opacity: 0;
  }
}

.animate-float-up {
  animation-name: float-up;
  animation-timing-function: linear;
  animation-fill-mode: forwards;
}

.task-title,
.task-xp {
  transition:
    opacity 0.3s ease,
    transform 0.3s ease;
}

.task-card.swap-out .task-title,
.task-card.swap-out .task-xp {
  opacity: 0;
  transform: translateY(10px);
}

.task-check {
  width: 30px;
  height: 30px;
  border-radius: 999px;
  background: linear-gradient(140deg, #34d399, #10b981);
}

.task-card.pop .task-check {
  animation: checkPop 0.55s cubic-bezier(0.34, 1.56, 0.64, 1);
}

@keyframes checkPop {
  0% {
    transform: scale(0.3);
  }

  65% {
    transform: scale(1.18);
  }

  100% {
    transform: scale(1);
  }
}
</style>
