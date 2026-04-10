<script lang="ts" setup>
const heroTasks = [
  { name: 'Morning workout routine', xp: '+50 XP' },
  { name: 'Write weekly report', xp: '+80 XP' },
  { name: 'Review pull requests', xp: '+60 XP' },
  { name: 'Read 20 pages', xp: '+40 XP' },
]

const mechanics = [
  {
    title: 'Create meaningful tasks',
    text: 'Break your goals into actionable tasks. Each task has a difficulty level that determines how much XP you earn.',
  },
  {
    title: 'Earn XP and level up',
    text: 'Completing tasks rewards you with experience points. Fill your XP bar to reach the next level and unlock new rewards.',
  },
  {
    title: 'Build unstoppable streaks',
    text: 'Stay consistent and build daily streaks. Missing a day resets your streak — so you’ll stay motivated to show up every day.',
  },
]

const weeklyBars = [
  { day: '10', value: 76, highlight: false },
  { day: '11', value: 80, highlight: false },
  { day: '12', value: 72, highlight: false },
  { day: '13', value: 83, highlight: false },
  { day: '14', value: 79, highlight: false },
  { day: '15', value: 96, highlight: true },
  { day: '16', value: 74, highlight: false },
]

const weekStats = [
  { label: 'Total XP', value: '3,420', note: '+18% this week' },
  { label: 'Tasks done', value: '42', note: '+6 vs last week' },
  { label: 'Current level', value: '14', note: '68% to Level 15' },
  { label: 'Best streak', value: '17', note: 'Personal record' },
]

const features = [
  {
    icon: '🗂',
    iconClass: 'teal-bg',
    title: 'Smart Task Management',
    text: 'Organize tasks by priority, category, and difficulty. Set deadlines, add subtasks, and watch your productivity soar with a system built around how you actually think.',
    tags: ['Priority Levels', 'Subtasks', 'Labels'],
  },
  {
    icon: '✦',
    iconClass: 'blue-bg',
    title: 'XP & Level System',
    text: 'Every completed task earns you XP based on its difficulty. As you gain XP, you level up — unlocking new titles, badges, and a clear visual record of your growth.',
    tags: ['Dynamic XP', 'Levels 1–100', 'Badges'],
  },
  {
    icon: '🔥',
    iconClass: 'lime-bg',
    title: 'Streak System',
    text: 'Consistency compounds. Complete at least one task per day to maintain your streak. The longer your streak, the greater the XP multiplier — stay on a roll and watch your progress multiply.',
    tags: ['Daily Streaks', 'XP Multiplier', 'Calendar View'],
  },
]
const steps = [
  {
    title: 'Create your tasks',
    text: 'Add tasks with a name, category, and difficulty level. Break big goals into small, actionable steps.',
  },
  {
    title: 'Complete your tasks',
    text: 'Check off tasks as you complete them. Each completion is logged and your XP is updated in real time.',
  },
  {
    title: 'Earn XP & level up',
    text: 'Watch your experience bar fill up. Hit milestones, level up, and keep your streak alive day after day.',
  },
]

const benefits = [
  {
    icon: '◎',
    iconClass: 'teal-bg',
    title: 'Stay consistent',
    text: 'Daily streaks create accountability. Show up every day, even when motivation is low — your streak is on the line.',
  },
  {
    icon: '↗️',
    iconClass: 'blue-bg',
    title: 'Track real progress',
    text: 'XP, levels, and task history give you a clear visual picture of how far you’ve come — not just a list of checkboxes.',
  },
  {
    icon: '⚡️',
    iconClass: 'lime-bg',
    title: 'Stay motivated',
    text: 'Rewards feel earned. Leveling up after a hard week is genuinely satisfying — and it keeps you coming back.',
  },
  {
    icon: '🏆',
    iconClass: 'amber-bg',
    title: 'Build better habits',
    text: 'Gamification isn’t gimmick — research shows that immediate, visible rewards reinforce habit formation and long-term consistency.',
  },
]

const streakDays = Array.from({ length: 31 }, (_, index) => ({
  day: index + 1,
  active: index + 1 <= 29,
  highlight: index + 1 === 29,
}))
</script>
