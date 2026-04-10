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

<template>
  <main class="homepage">
    <section class="hero-section">
      <div class="container hero-grid">
        <div class="hero-content">
          <div class="hero-badge">
            <span class="hero-badge-dot">•</span>
            Now in public beta
          </div>

          <h1 class="hero-title">
            Level up your
            <span>productivity</span>
          </h1>

          <p class="hero-description">
            Turn your daily tasks into measurable progress — earn XP, unlock levels, and build
            streaks that actually keep you going.
          </p>

          <div class="hero-actions">
            <button class="button button-primary">✦ Get started free</button>
            <button class="button button-secondary">▶️ View demo</button>
          </div>

          <div class="hero-stats">
            <div class="stat-item">
              <div class="stat-value">12k+</div>
              <div class="stat-label">Active users</div>
            </div>
            <div class="stat-item">
              <div class="stat-value">98%</div>
              <div class="stat-label">Satisfaction</div>
            </div>
            <div class="stat-item">
              <div class="stat-value">4.9★</div>
              <div class="stat-label">Rating</div>
            </div>
          </div>
        </div>

        <div class="hero-visual">
          <div class="xp-toast">
            <div class="xp-toast-icon">⚡️</div>
            <div>
              <div class="xp-toast-value">+120 XP</div>
              <div class="xp-toast-label">Task completed</div>
            </div>
          </div>

          <div class="dashboard-card">
            <div class="browser-bar">
              <span class="browser-dot browser-dot-red"></span>
              <span class="browser-dot browser-dot-yellow"></span>
              <span class="browser-dot browser-dot-green"></span>
              <div class="browser-address">app.focusup.io/dashboard</div>
            </div>

            <div class="dashboard-body">
              <div class="dashboard-top">
                <div class="profile-block">
                  <div class="avatar">S</div>
                  <div>
                    <div class="profile-name">John Pork</div>
                    <div class="profile-subtitle">Good morning</div>
                  </div>
                </div>
                <div class="level-pill">✦ Level 14</div>
              </div>

              <div class="progress-card dark-card">
                <div class="progress-head">
                  <span>XP Progress</span>
                  <span class="progress-total">3,420 / 5,000 XP</span>
                </div>
                <div class="progress-track">
                  <div class="progress-fill"></div>
                </div>
                <div class="progress-levels">
                  <span>Level 14</span>
                  <span>Level 15</span>
                </div>
              </div>

              <div class="dashboard-stats">
                <div class="mini-card">
                  <div class="mini-label">Tasks</div>
                  <div class="mini-value">24</div>
                  <div class="mini-subtitle">done today</div>
                </div>
                <div class="mini-card">
                  <div class="mini-label">Streak</div>
                  <div class="mini-value mini-value-teal">17</div>
                  <div class="mini-subtitle">day streak</div>
                </div>
                <div class="mini-card">
                  <div class="mini-label">XP Today</div>
                  <div class="mini-value">+480</div>
                  <div class="mini-subtitle">earned</div>
                </div>
              </div>

              <div class="tasks-section">
                <div class="tasks-header">
                  <div class="tasks-title">Today's Tasks</div>
                  <button class="tasks-add">+</button>
                </div>
                <div class="tasks-list">
                  <div v-for="task in heroTasks" :key="task.name" class="task-row">
                    <div class="task-left">
                      <div class="task-checkbox"></div>
                      <span class="task-name">{{ task.name }}</span>
                    </div>
                    <span class="task-xp">{{ task.xp }}</span>
                  </div>
                </div>
              </div>

              <div class="achievement-card">
                <div class="achievement-icon">🏆</div>
                <div>
                  <div class="achievement-title">New Achievement</div>
                  <div class="achievement-subtitle">7-day streak unlocked</div>
                </div>
                <div class="achievement-days">
                  <span class="day-chip blue">Di</span>
                  <span class="day-chip cyan">Mi</span>
                  <span class="day-chip sky">Do</span>
                  <span class="day-chip blue">Fr</span>
                  <span class="day-chip lime">Sa</span>
                  <span class="day-chip gray">So</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <section class="white-section">
      <div class="container two-column-section">
        <div class="section-copy">
          <div class="section-kicker">How FocusUp works</div>
          <h2 class="section-title">
            Productivity meets
            <span>game mechanics</span>
          </h2>
          <p class="section-description">
            We’ve taken the most motivating elements of gaming — progress, rewards, and momentum —
            and applied them to your everyday workflow.
          </p>

          <div class="mechanics-list">
            <div v-for="(item, index) in mechanics" :key="item.title" class="mechanic-item">
              <div class="mechanic-number">{{ index + 1 }}</div>
              <div>
                <h3 class="mechanic-title">{{ item.title }}</h3>
                <p class="mechanic-text">{{ item.text }}</p>
              </div>
            </div>
          </div>
        </div>

        <div class="overview-card">
          <div class="overview-kicker">Weekly XP overview</div>

          <div class="overview-bars">
            <div v-for="bar in weeklyBars" :key="bar.day" class="overview-bar-item">
              <div class="overview-bar-track">
                <div
                  class="overview-bar-fill"
                  :class="{ highlight: bar.highlight }"
                  :style="{ width: `${bar.value}%` }"
                ></div>
              </div>
              <span class="overview-day">{{ bar.day }}</span>
            </div>
          </div>

          <div class="overview-grid">
            <div v-for="stat in weekStats" :key="stat.label" class="overview-stat-card">
              <div class="overview-stat-label">{{ stat.label }}</div>
              <div class="overview-stat-value">{{ stat.value }}</div>
              <div class="overview-stat-note">{{ stat.note }}</div>
            </div>
          </div>
        </div>
      </div>
    </section>
    <section class="features-section">
      <div class="container">
        <div class="section-header centered">
          <div class="section-kicker">Features</div>
          <h2 class="section-title centered-title">Everything you need to stay in flow</h2>
          <p class="section-description centered-description">
            Designed for people who want results, not just another todo list.
          </p>
        </div>
        <div class="features-grid">
          <article v-for="feature in features" :key="feature.title" class="feature-card">
            <div class="feature-icon" :class="feature.iconClass">{{ feature.icon }}</div>
            <h3 class="feature-title">{{ feature.title }}</h3>
            <p class="feature-text">{{ feature.text }}</p>
            <div class="feature-tags">
              <span v-for="tag in feature.tags" :key="tag" class="feature-tag">{{ tag }}</span>
            </div>
          </article>
        </div>
      </div>
    </section>
  </main>
</template>
