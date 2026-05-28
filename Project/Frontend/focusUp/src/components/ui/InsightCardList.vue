<script lang="ts" setup>
import InsightCard from '@/components/ui/InsightCard.vue'
import type { InsightType } from '@/types/insightType.ts'
import { Stats } from '@/types/stats.ts'
import { computed } from 'vue'

const props = defineProps<{
  stats: Stats
}>()

const insights = computed<InsightType[]>(() => {
  const result: InsightType[] = []

  if (props.stats.bestStreak > 7) {
    result.push({
      title: 'Starke Streak',
      svg: 'fa-solid fa-fire',
      cardValue: `${props.stats.streakCount} Tage`,
      description: `Du hast bereits ${props.stats.streakCount} Tage in Folge gearbeitet.`,
      type: 'success',
    })
  }

  if(props.stats.tasksOpen > props.stats.tasksDone) {
    result.push({
      title: 'Task-Stau',
      svg: 'fa-solid fa-triangle-exclamation',
      cardValue: `${props.stats.tasksOpen} Offen`,
      description: 'Du hast mehr offene als erledigte Tasks. Versuche heute ein paar abzuschliessen.',
      type: 'warning',
    })
  }

  if(props.stats.totalXp > 1000){
    result.push({
      title: 'Guter Fortschritt',
      svg: 'fa-solid fa-chart-line',
      cardValue: `${props.stats.totalXp} XP`,
      description: 'Du hast schon über 1000 XP gesammelt.',
      type: 'info',
    })
  }

  if(props.stats.tasksDone === 0){
    result.push({
      title: 'Der Start wartet',
      svg: '',
      cardValue: '',
      description: 'Du hast noch keine Task abgeschlossen. Die erste erledigte Task bringt dich ins System.',
      type: 'info',
    })
  }

  if(props.stats.tasksDone >= 1){
    result.push({
      title: 'Erster Fortschritt',
      svg: 'fa-regular fa-circle-play',
      cardValue: '0 Tasks',
      description: `Du hast bereits ${props.stats.tasksDone} Tasks abgeschlossen. Dein Produktivitätslauf hat begonnen.`,
      type: 'success',
    })
  }

  if(props.stats.tasksDone >= 10){
    result.push({
      title: 'Task-Maschine',
      svg: 'fa-solid fa-shoe-prints',
      cardValue: `${props.stats.tasksDone} erledigt`,
      description: `Mit ${props.stats.tasksDone} erledigten Tasks zeigst du starke Konstanz.`,
      type: 'success',
    })
  }

  if(props.stats.tasksDone > 0 && props.stats.tasksOpen === 0){
    result.push({
      title: 'Cleaner des Monats',
      svg: 'fa-solid fa-broom',
      cardValue: 'Alles erledigt',
      description: `Du hast keine offenen Tasks. Sehr sauber gearbeitet.`,
      type: 'success',
    })
  }

  if(props.stats.totalXp > 500){
    result.push({
      title: 'XP-Sammler',
      svg: 'fa-solid fa-coins',
      cardValue: `${props.stats.totalXp} XP`,
      description: `Du hast bereits ${props.stats.totalXp} XP gesammelt. Dein Fortschritt ist klar sichtbar.`,
      type: 'success',
    })
  }

  if(props.stats.totalXp > 2000){
    result.push({
      title: 'Level-Grinder',
      svg: 'fa-solid fa-arrow-up-right-dots',
      cardValue: `${props.stats.totalXp} XP`,
      description: `${props.stats.totalXp} XP zeigen, dass du langfristig aktiv bist.`,
      type: 'success',
    })
  }

  if(props.stats.totalTimeMin > 60){
    result.push({
      title: 'Eine Stunde investiert',
      svg: 'fa-regular fa-clock',
      cardValue: `${Math.round(props.stats.totalTimeMin / 60)} h`,
      description: `Du hast bereits ${Math.round(props.stats.totalTimeMin / 60)} Stunden produktive Zeit gesammelt.`,
      type: 'success',
    })
  }

  if(props.stats.totalTimeMin > 1000){
    result.push({
      title: 'Deep Work Veteran',
      svg: 'fa-solid fa-brain',
      cardValue: `${Math.round(props.stats.totalTimeMin / 60)} h`,
      description: `Über ${Math.round(props.stats.totalTimeMin / 60)} Stunden Fokuszeit sind ein starkes Zeichen für Disziplin.`,
      type: 'success',
    })
  }

  if(props.stats.streakCount >= 3){
    result.push({
      title: 'Konstanz aufgebaut',
      svg: 'fa-solid fa-fire',
      cardValue: `${props.stats.streakCount} Tage`,
      description: `Du bist seit ${props.stats.streakCount} Tagen aktiv. Genau so entsteht Routine.`,
      type: 'success',
    })
  }

  if(props.stats.streakCount === 0 && props.stats.bestStreak > 0){
    result.push({
      title: 'Wann kommt das Comeback',
      svg: 'fa-solid fa-rotate-left',
      cardValue: `${props.stats.bestStreak} Rekord`,
      description: `Deine beste Streak war ${props.stats.bestStreak} Tage. Du kannst sie wieder aufbauen.`,
      type: 'warning',
    })
  }

  if((props.stats.streakCount === props.stats.bestStreak) && props.stats.streakCount > 0){
    result.push({
      title: 'Beste Streak jemals!',
      svg: 'fa-solid fa-trophy',
      cardValue: `${props.stats.bestStreak} Tage`,
      description: `Deine aktuelle Streak ist gleichzeitig dein Rekord: ${props.stats.bestStreak} Tage.`,
      type: 'success',
    })
  }

  if(props.stats.tasksDone > 0 && props.stats.bestStreak > 0){
    result.push({
      title: 'Potenzial verschenkt!',
      svg: 'fa-solid fa-battery-quarter',
      cardValue: '0 Streak',
      description: 'Du hast schon Tasks erledigt, aber aktuell keine Streak. Eine Task heute könnte den Neustart bringen.',
      type: 'danger',
    })
  }

  return result
})
</script>

<template>
  <div class="mt-3 grid grid-cols-3 gap-4 w-full min-h-[150px]">
    <InsightCard
      v-for="insight in insights.slice(0, 3)"
      class="h-full col-span-1"
      :title="insight.title"
      :card-value="insight.cardValue"
      :description="insight.description"
      :svg="insight.svg"
      :type="insight.type"
      >
    </InsightCard>
  </div>
</template>
