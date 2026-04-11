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
            <button class="button button-primary">⚡ Get started free</button>
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
                  <span class="tasks-add">+</span>
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
                  <span class="day-chip weekday-gradient">Di</span>
                  <span class="day-chip weekday-gradient">Mi</span>
                  <span class="day-chip weekday-gradient">Do</span>
                  <span class="day-chip weekday-gradient">Fr</span>
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
    <section class="white-section steps-section">
      <div class="container">
        <div class="section-header centered">
          <div class="section-kicker">How it works</div>
          <h2 class="section-title centered-title">Three simple steps</h2>
          <p class="section-description centered-description">
            Get started in minutes. No complicated setup — just open the app and begin.
          </p>
        </div>

        <div class="steps-timeline"></div>

        <div class="steps-grid">
          <div v-for="(step, index) in steps" :key="step.title" class="step-card">
            <div class="step-number">{{ index + 1 }}</div>
            <h3 class="step-title">{{ step.title }}</h3>
            <p class="step-text">{{ step.text }}</p>

            <div class="step-preview">
              <template v-if="index === 0">
                <div class="preview-input">Add a new task <span>+</span></div>
                <div class="preview-row"><span>Morning workout</span><span>+50 XP</span></div>
                <div class="preview-row"><span>Deep work session</span><span>+60 XP</span></div>
              </template>

              <template v-else-if="index === 1">
                <div class="preview-row strong">
                  <span>Morning workout</span><span>+50 XP</span>
                </div>
                <div class="preview-row strong">
                  <span>Deep work session</span><span>+80 XP</span>
                </div>
                <div class="preview-earned">+130 XP earned today</div>
              </template>

              <template v-else>
                <div class="levelup-card">
                  <div class="levelup-star">★</div>
                  <div class="levelup-label">Level up</div>
                  <div class="levelup-value">Level 15</div>
                  <div class="levelup-track">
                    <div class="levelup-fill"></div>
                  </div>
                </div>
              </template>
            </div>
          </div>
        </div>
      </div>
    </section>

    <section class="momentum-section">
      <div class="container two-column-section">
        <div class="section-copy">
          <div class="section-kicker">Why FocusUp</div>
          <h2 class="section-title">
            Built for long-term
            <span>momentum</span>
          </h2>
          <p class="section-description">
            Most productivity tools help you organize. FocusUp helps you actually do the work — day
            after day.
          </p>

          <div class="benefits-list">
            <div v-for="benefit in benefits" :key="benefit.title" class="benefit-item">
              <div class="benefit-icon" :class="benefit.iconClass">{{ benefit.icon }}</div>
              <div>
                <h3 class="benefit-title">{{ benefit.title }}</h3>
                <p class="benefit-text">{{ benefit.text }}</p>
              </div>
            </div>
          </div>
        </div>

        <div class="calendar-card">
          <div class="calendar-head">
            <div class="calendar-title">March Streak Calendar</div>
            <div class="calendar-year">2026</div>
          </div>
          <div class="calendar-grid">
            <div
              v-for="day in streakDays"
              :key="day.day"
              class="calendar-day"
              :class="{
                active: day.active,
                highlight: day.highlight,
                inactive: !day.active,
              }"
            >
              {{ day.day }}
            </div>
          </div>

          <div class="calendar-footer">
            <div>
              <div class="calendar-streak"><span>🔥</span><strong>29</strong></div>
              <div class="calendar-note">day streak — personal best</div>
            </div>
            <div class="calendar-bonus-wrap">
              <div class="calendar-bonus">1.5× XP Bonus</div>
              <div class="calendar-note">streak multiplier active</div>
            </div>
          </div>
        </div>
      </div>
    </section>
    <section class="cta-section">
      <div class="cta-inner">
        <div class="cta-badge">Join 13,000+ users</div>
        <h2 class="cta-title">
          Start your journey
          <span>today.</span>
        </h2>
        <p class="cta-text">
          Stop planning to be productive. Create your account in 30 seconds and complete your first
          task before the day is over.
        </p>
        <div class="cta-actions">
          <button class="button cta-primary">✦ Create free account</button>
          <button class="button cta-secondary">View demo →</button>
        </div>
        <div class="cta-note">For free · No credit card required</div>
      </div>
    </section>
  </main>
</template>

<style scoped>
.homepage {
  overflow-x: hidden;
  background: var(--background-color);
  color: var(--text-color);
  font-family: var(--font-family), sans-serif;
}

.container {
  width: 100%;
  max-width: 1280px;
  margin: 0 auto;
  padding: 0 56px;
}

.hero-section {
  padding: 56px 0 104px;
  background: var(--background-color);
}

.hero-grid {
  display: grid;
  grid-template-columns: minmax(420px, 1fr) minmax(520px, 1.1fr);
  align-items: center;
  gap: 80px;
}

.hero-content {
  padding-top: 18px;
}

.hero-badge,
.section-kicker,
.overview-kicker,
.cta-badge {
  display: inline-flex;
  align-items: center;
  font-size: 12px;
  font-weight: 800;
  letter-spacing: 0.18em;
  text-transform: uppercase;
}

.hero-badge {
  padding: 9px 16px;
  border: 1px solid #a7f3d0;
  border-radius: 999px;
  background: #ccfbf1;
  color: var(--primary-color);
  box-shadow: inset 0 0 0 1px rgba(20, 184, 166, 0.08);
}

.hero-badge-dot {
  margin-right: 8px;
  font-size: 10px;
}

.hero-title,
.section-title {
  margin: 0;
  font-weight: 900;
  letter-spacing: -0.065em;
  color: var(--text-color);
}

.cta-title {
  margin: 0;
  font-weight: 900;
  letter-spacing: -0.065em;
  color: var(--surface-color);
}

.hero-title {
  margin-top: 26px;
  font-size: 78px;
  line-height: 0.92;
}

.hero-title span,
.cta-title span {
  display: block;
  color: var(--primary-color);
}

.hero-description,
.section-description,
.cta-text {
  color: #64748b;
}

.hero-description {
  max-width: 470px;
  margin-top: 28px;
  font-size: 22px;
  line-height: 1.58;
  font-weight: 500;
}

.hero-actions,
.cta-actions {
  display: flex;
  align-items: center;
  gap: 16px;
}

.hero-actions {
  margin-top: 38px;
}

.button {
  border: none;
  border-radius: 18px;
  cursor: pointer;
  font-family: inherit;
  font-size: 21px;
  font-weight: 800;
  padding: 18px 28px;
  transition:
    transform 0.2s ease,
    background 0.2s ease;
}

.button:hover {
  transform: translateY(-1px);
}

.button-primary {
  color: var(--surface-color);
  background: var(--primary-color);
  box-shadow: 0 16px 34px rgba(20, 184, 166, 0.24);
}

.button-primary:hover {
  background: var(--primary-color);
  filter: brightness(0.92);
}

.button-secondary {
  color: var(--text-color);
  background: var(--surface-color);
  border: 1px solid #e2e8f0;
  box-shadow: 0 10px 24px rgba(15, 23, 42, 0.05);
}

.hero-stats {
  display: flex;
  gap: 52px;
  margin-top: 54px;
}

.stat-value {
  font-size: 30px;
  font-weight: 900;
  line-height: 1;
  letter-spacing: -0.04em;
}

.stat-label {
  margin-top: 10px;
  font-size: 18px;
  font-weight: 500;
  color: #94a3b8;
}

.hero-visual {
  position: relative;
}

.xp-toast {
  position: absolute;
  top: 28px;
  right: -18px;
  z-index: 2;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 18px 24px;
  border-radius: 24px;
  color: var(--surface-color);
  background: linear-gradient(90deg, var(--primary-color), var(--secondary-color));
  box-shadow: 0 22px 40px rgba(14, 165, 233, 0.22);
}

.xp-toast-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 42px;
  height: 42px;
  border-radius: 999px;
  background: rgba(255, 255, 255, 0.2);
  font-size: 18px;
}

.xp-toast-value {
  font-size: 20px;
  font-weight: 900;
  line-height: 1;
}

.xp-toast-label {
  margin-top: 8px;
  font-size: 14px;
  font-weight: 500;
  color: rgba(255, 255, 255, 0.9);
}

.dashboard-card {
  padding: 16px;
  border: 1px solid #e2e8f0;
  border-radius: 36px;
  background: var(--surface-color);
  box-shadow: 0 30px 80px rgba(15, 23, 42, 0.1);
}

.browser-bar {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 2px 8px 0;
  margin-bottom: 14px;
}

.browser-dot {
  width: 12px;
  height: 12px;
  border-radius: 999px;
}

.browser-dot-red {
  background: #f87171;
}
.browser-dot-yellow {
  background: #fbbf24;
}
.browser-dot-green {
  background: #4ade80;
}

.browser-address {
  flex: 1;
  margin-left: 10px;
  padding: 10px 18px;
  border-radius: 14px;
  background: var(--background-color);
  color: #94a3b8;
  font-size: 14px;
  font-weight: 600;
}

.dashboard-body {
  padding: 22px;
  border-radius: 28px;
  background: #f8fafc;
}

.dashboard-top,
.progress-head,
.progress-levels,
.tasks-header,
.calendar-head,
.calendar-footer {
  display: flex;
  align-items: center;
  justify-content: space-between;
}

.profile-block {
  display: flex;
  align-items: center;
  gap: 14px;
}

.avatar {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 54px;
  height: 54px;
  border-radius: 999px;
  background: var(--primary-color);
  color: var(--surface-color);
  font-size: 22px;
  font-weight: 900;
}

.profile-name {
  font-size: 28px;
  font-weight: 900;
  line-height: 1;
}

.profile-subtitle,
.mini-subtitle,
.achievement-subtitle,
.calendar-note {
  color: #94a3b8;
}

.profile-subtitle {
  margin-top: 8px;
  font-size: 18px;
  font-weight: 500;
}

.level-pill {
  padding: 10px 18px;
  border-radius: 999px;
  background: #ccfbf1;
  color: var(--primary-color);
  font-size: 15px;
  font-weight: 700;
}

.progress-card {
  margin-top: 24px;
  padding: 18px 22px;
  border-radius: 22px;
}

.dark-card {
  color: var(--surface-color);
  background: var(--text-color);
}

.progress-head {
  font-size: 13px;
  font-weight: 800;
  letter-spacing: 0.18em;
  text-transform: uppercase;
  color: #94a3b8;
}

.progress-total {
  letter-spacing: normal;
  color: var(--surface-color);
}

.progress-track,
.levelup-track {
  overflow: hidden;
  border-radius: 999px;
}

.progress-track {
  height: 10px;
  margin-top: 16px;
  background: #334155;
}

.progress-fill {
  width: 68%;
  height: 100%;
  border-radius: 999px;
  background: linear-gradient(90deg, var(--primary-color), var(--secondary-color));
}

.progress-levels {
  margin-top: 10px;
  font-size: 14px;
  font-weight: 500;
  color: #94a3b8;
}

.dashboard-stats,
.overview-grid,
.features-grid,
.steps-grid,
.calendar-grid {
  display: grid;
}

.dashboard-stats {
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 14px;
  margin-top: 20px;
}

.mini-card,
.overview-stat-card {
  border: 1px solid #e2e8f0;
  background: var(--surface-color);
}

.mini-card {
  padding: 16px 18px;
  border-radius: 18px;
}

.mini-label,
.overview-stat-label {
  font-size: 12px;
  font-weight: 700;
  letter-spacing: 0.16em;
  text-transform: uppercase;
  color: #94a3b8;
}

.mini-value {
  margin-top: 10px;
  font-size: 24px;
  font-weight: 900;
  color: var(--text-color);
}

.mini-value-teal {
  color: var(--primary-color);
}

.mini-subtitle {
  margin-top: 6px;
  font-size: 16px;
  font-weight: 500;
}

.tasks-section {
  margin-top: 22px;
}

.tasks-title,
.calendar-title {
  font-size: 18px;
  font-weight: 900;
  color: var(--text-color);
}

.tasks-add {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 34px;
  height: 34px;
  border-radius: 10px;
  background: var(--primary-color);
  color: var(--surface-color);
  font-size: 20px;
}

.tasks-list {
  margin-top: 12px;
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.task-row,
.achievement-card,
.preview-row,
.preview-input {
  display: flex;
  align-items: center;
}

.task-row {
  justify-content: space-between;
  padding: 14px 16px;
  border: 1px solid #e2e8f0;
  border-radius: 14px;
  background: var(--surface-color);
}

.task-left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.task-checkbox {
  width: 24px;
  height: 24px;
  border: 1px solid #cbd5e1;
  border-radius: 7px;
  background: var(--surface-color);
}

.task-name {
  font-size: 18px;
  font-weight: 500;
  color: #334155;
}

.task-xp {
  font-size: 18px;
  font-weight: 800;
  color: var(--primary-color);
}

.achievement-card {
  gap: 14px;
  margin-top: 16px;
  padding: 14px 16px;
  border-radius: 18px;
  background: var(--surface-color);
  box-shadow: 0 10px 24px rgba(15, 23, 42, 0.06);
}

.achievement-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 48px;
  height: 48px;
  border-radius: 14px;
  background: #fef3c7;
  font-size: 22px;
}

.achievement-title {
  font-size: 20px;
  font-weight: 900;
  color: var(--text-color);
}

.achievement-subtitle {
  margin-top: 4px;
  font-size: 16px;
  font-weight: 500;
}

.achievement-days {
  display: flex;
  gap: 8px;
  margin-left: auto;
}

.day-chip {
  display: flex;
  align-items: center;
  justify-content: center;
  min-width: 34px;
  height: 34px;
  padding: 0 10px;
  border-radius: 10px;
  font-size: 14px;
  font-weight: 700;
}

.weekday-gradient {
  color: var(--surface-color);
  background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));
}

.blue {
  background: var(--secondary-color);
  color: var(--surface-color);
}
.cyan {
  background: #22d3ee;
  color: var(--surface-color);
}
.sky {
  background: #06b6d4;
  color: var(--surface-color);
}
.lime {
  background: var(--accent-color);
  color: var(--surface-color);
}
.gray {
  background: #e2e8f0;
  color: #94a3b8;
}

.white-section {
  padding: 112px 0;
  background: var(--surface-color);
}

.two-column-section {
  display: grid;
  grid-template-columns: 1fr 1fr;
  align-items: center;
  gap: 80px;
  padding: 0 40px;
  max-width: 1240px;
}

.section-copy {
  max-width: 520px;
}

.section-kicker,
.overview-kicker {
  color: var(--primary-color);
}

.section-title {
  margin-top: 0;
  font-size: 58px;
  line-height: 0.98;
}

.section-title span {
  display: block;
}

.section-description {
  max-width: 455px;
  margin-top: 24px;
  font-size: 17px;
  font-weight: 500;
  line-height: 1.82;
}

.mechanics-list,
.benefits-list {
  margin-top: 34px;
  display: flex;
  flex-direction: column;
}

.mechanics-list {
  gap: 28px;
}
.benefits-list {
  gap: 24px;
}

.mechanic-item,
.benefit-item {
  display: flex;
  gap: 16px;
}

.mechanic-number,
.benefit-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  flex-shrink: 0;
  width: 34px;
  height: 34px;
  border-radius: 10px;
}

.mechanic-number {
  background: var(--primary-color);
  color: var(--surface-color);
  font-size: 14px;
  font-weight: 900;
}

.mechanic-title,
.benefit-title,
.feature-title,
.step-title {
  margin: 0;
  color: var(--text-color);
  font-weight: 900;
}

.mechanic-title {
  font-size: 18px;
  line-height: 1.2;
}

.mechanic-text,
.benefit-text,
.feature-text,
.step-text {
  margin-top: 7px;
  color: #64748b;
  font-weight: 500;
}

.mechanic-text {
  max-width: 430px;
  font-size: 16px;
  line-height: 1.78;
}

.overview-card,
.calendar-card {
  border: 1px solid #e2e8f0;
  border-radius: 24px;
  background: #f8fafc;
}

.overview-card {
  padding: 22px;
  box-shadow: 0 20px 54px rgba(15, 23, 42, 0.07);
}

.overview-kicker {
  font-size: 10px;
  letter-spacing: 0.24em;
  color: #94a3b8;
}

.overview-bars {
  display: flex;
  align-items: flex-end;
  gap: 12px;
  margin-top: 24px;
}

.overview-bar-item {
  display: flex;
  flex: 1;
  flex-direction: column;
  align-items: center;
  gap: 11px;
}

.overview-bar-track {
  width: 100%;
  height: 10px;
  overflow: hidden;
  border-radius: 999px;
  background: #e2e8f0;
}

.overview-bar-fill {
  height: 100%;
  border-radius: 999px;
  background: var(--secondary-color);
}

.overview-bar-fill.highlight {
  background: var(--accent-color);
}

.overview-day {
  font-size: 11px;
  font-weight: 600;
  color: #94a3b8;
}

.overview-grid {
  grid-template-columns: repeat(2, minmax(0, 1fr));
  gap: 14px;
  margin-top: 22px;
}

.overview-stat-card {
  padding: 16px 18px;
  border-radius: 16px;
}

.overview-stat-label {
  font-size: 10px;
  font-weight: 800;
  letter-spacing: 0.22em;
}

.overview-stat-value {
  margin-top: 10px;
  font-size: 28px;
  font-weight: 900;
  line-height: 1;
  letter-spacing: -0.05em;
}

.overview-stat-note {
  margin-top: 7px;
  font-size: 14px;
  font-weight: 700;
  color: var(--accent-color);
}

.features-section,
.momentum-section {
  padding: 110px 0;
}

.section-header.centered {
  text-align: center;
}

.centered-title {
  font-size: 56px;
  line-height: 1;
}

.centered-description {
  max-width: 640px;
  margin: 14px auto 0;
  text-align: center;
}

.features-grid {
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 26px;
  margin-top: 52px;
}

.feature-card {
  padding: 30px 28px;
  border: 1px solid #e2e8f0;
  border-radius: 22px;
  background: var(--surface-color);
  box-shadow: 0 10px 28px rgba(15, 23, 42, 0.05);
}

.feature-icon {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 46px;
  height: 46px;
  margin-bottom: 24px;
  border-radius: 14px;
  font-size: 20px;
}

.teal-bg {
  background: #ccfbf1;
  color: var(--primary-color);
}
.blue-bg {
  background: #dbeafe;
  color: var(--secondary-color);
}
.lime-bg {
  background: #ecfccb;
  color: var(--accent-color);
}
.amber-bg {
  background: #fef3c7;
  color: #f59e0b;
}

.feature-title {
  font-size: 17px;
  line-height: 1.2;
}

.feature-text {
  font-size: 15px;
  line-height: 1.86;
}

.feature-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 6px;
  margin-top: 18px;
}

.feature-tag {
  padding: 5px 10px;
  border-radius: 999px;
  background: var(--background-color);
  color: #64748b;
  font-size: 10px;
  font-weight: 700;
}

.steps-section {
  position: relative;
}

.steps-timeline {
  position: absolute;
  left: 50%;
  width: min(880px, calc(100% - 360px));
  height: 3px;
  margin-top: 84px;
  transform: translateX(-50%);
  border-radius: 999px;
  background: #2dd4bf;
}

.steps-grid {
  position: relative;
  grid-template-columns: repeat(3, minmax(0, 1fr));
  gap: 46px;
  margin-top: 58px;
}

.step-card {
  text-align: center;
}

.step-number {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 52px;
  height: 52px;
  margin: 0 auto;
  border: 4px solid #2dd4bf;
  border-radius: 999px;
  background: var(--surface-color);
  box-shadow: 0 2px 8px rgba(15, 23, 42, 0.04);
  color: var(--primary-color);
  font-size: 17px;
  font-weight: 900;
}

.step-title {
  margin-top: 22px;
  font-size: 18px;
}

.step-text {
  max-width: 300px;
  margin: 12px auto 0;
  font-size: 15px;
  line-height: 1.85;
}

.step-preview {
  width: 292px;
  margin: 24px auto 0;
  padding: 14px;
  border: 1px solid #e2e8f0;
  border-radius: 20px;
  background: #f8fafc;
  box-shadow: 0 10px 28px rgba(15, 23, 42, 0.04);
}

.preview-input,
.preview-row {
  justify-content: space-between;
  padding: 10px 14px;
  border-radius: 10px;
  background: var(--surface-color);
  font-size: 12px;
  font-weight: 500;
  color: #64748b;
  box-shadow: 0 2px 6px rgba(15, 23, 42, 0.04);
}

.preview-input span {
  padding: 2px 6px;
  border-radius: 6px;
  background: var(--primary-color);
  color: var(--surface-color);
  font-size: 10px;
  font-weight: 700;
}

.preview-row + .preview-row,
.preview-input + .preview-row,
.preview-earned,
.preview-row + .preview-earned {
  margin-top: 10px;
}

.preview-row.strong span:last-child {
  color: var(--primary-color);
  font-weight: 900;
}

.preview-earned {
  padding: 10px 14px;
  border-radius: 10px;
  background: #ccfbf1;
  color: var(--primary-color);
  font-size: 12px;
  font-weight: 900;
  text-align: center;
}

.levelup-card {
  padding: 18px 16px;
  border-radius: 16px;
  background: var(--text-color);
  color: var(--surface-color);
}

.levelup-star {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 40px;
  height: 40px;
  margin: 0 auto;
  border-radius: 999px;
  background: var(--primary-color);
  font-size: 17px;
}

.levelup-label {
  margin-top: 12px;
  color: #94a3b8;
  font-size: 9px;
  font-weight: 700;
  letter-spacing: 0.22em;
  text-transform: uppercase;
}

.levelup-value {
  margin-top: 6px;
  font-size: 28px;
  font-weight: 900;
  line-height: 1;
  letter-spacing: -0.05em;
}

.levelup-track {
  height: 7px;
  margin-top: 14px;
  background: #334155;
}

.levelup-fill {
  width: 68%;
  height: 100%;
  border-radius: 999px;
  background: var(--secondary-color);
}

.benefit-icon {
  margin-top: 2px;
  font-size: 14px;
}

.benefit-title {
  font-size: 17px;
}

.benefit-text {
  max-width: 430px;
  font-size: 15px;
  line-height: 1.8;
}

.calendar-card {
  padding: 22px;
  background: var(--surface-color);
  box-shadow: 0 20px 54px rgba(15, 23, 42, 0.08);
}

.calendar-title {
  font-size: 16px;
}

.calendar-year {
  padding: 6px 12px;
  border-radius: 999px;
  background: var(--background-color);
  color: #94a3b8;
  font-size: 11px;
  font-weight: 700;
}

.calendar-grid {
  grid-template-columns: repeat(7, minmax(0, 1fr));
  gap: 10px;
  margin-top: 24px;
}

.calendar-day {
  display: flex;
  align-items: center;
  justify-content: center;
  aspect-ratio: 1;
  border-radius: 10px;
  font-size: 12px;
  font-weight: 700;
}

.calendar-day.active {
  background: #22c7ee;
  color: var(--surface-color);
}

.calendar-day.highlight {
  background: var(--accent-color);
}

.calendar-day.inactive {
  background: var(--background-color);
  color: #cbd5e1;
}

.calendar-footer {
  margin-top: 24px;
  gap: 30px;
  align-items: flex-end;
}

.calendar-streak {
  display: flex;
  align-items: center;
  gap: 10px;
}

.calendar-streak span {
  font-size: 28px;
}

.calendar-streak strong {
  font-size: 44px;
  font-weight: 900;
  line-height: 1;
  letter-spacing: -0.05em;
  color: var(--text-color);
}

.calendar-bonus-wrap {
  text-align: right;
}

.calendar-bonus {
  font-size: 17px;
  font-weight: 900;
  color: var(--primary-color);
}

.cta-section {
  padding: 102px 0;
  text-align: center;
  color: var(--surface-color);
  background:
    radial-gradient(circle at top, rgba(20, 184, 166, 0.16), transparent 30%),
    linear-gradient(180deg, #062342, #081c35);
}

.cta-inner {
  width: 100%;
  max-width: 980px;
  margin: 0 auto;
  padding: 0 40px;
}

.cta-badge {
  padding: 7px 14px;
  border: 1px solid rgba(45, 212, 191, 0.2);
  border-radius: 999px;
  background: rgba(45, 212, 191, 0.1);
  color: #67e8f9;
  font-size: 10px;
  letter-spacing: 0.22em;
}

.cta-title {
  margin-top: 26px;
  font-size: 64px;
  line-height: 0.98;
}

.cta-text {
  max-width: 610px;
  margin: 22px auto 0;
  color: #cbd5e1;
  font-size: 17px;
  font-weight: 500;
  line-height: 1.82;
}

.cta-actions {
  justify-content: center;
  margin-top: 34px;
}

.cta-primary {
  padding: 14px 22px;
  border-radius: 14px;
  background: var(--surface-color);
  color: var(--text-color);
  font-size: 14px;
  box-shadow: 0 10px 24px rgba(0, 0, 0, 0.18);
}

.cta-primary:hover {
  background: #f8fafc;
}

.cta-secondary {
  padding: 14px 22px;
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 14px;
  background: rgba(255, 255, 255, 0.05);
  color: #e2e8f0;
  font-size: 14px;
  backdrop-filter: blur(6px);
}

.cta-secondary:hover {
  background: rgba(255, 255, 255, 0.1);
}

.cta-note {
  margin-top: 18px;
  color: #94a3b8;
  font-size: 12px;
  font-weight: 500;
}

@media (max-width: 1200px) {
  .container,
  .two-column-section,
  .cta-inner {
    padding-left: 32px;
    padding-right: 32px;
  }

  .hero-grid,
  .two-column-section,
  .features-grid,
  .steps-grid {
    gap: 32px;
  }

  .hero-title,
  .section-title,
  .centered-title,
  .cta-title {
    font-size: 56px;
  }

  .hero-description,
  .section-description,
  .cta-text {
    font-size: 18px;
  }

  .button {
    font-size: 16px;
  }

  .task-name,
  .task-xp,
  .profile-name {
    font-size: 16px;
  }
}

@media (max-width: 960px) {
  .hero-grid,
  .two-column-section,
  .features-grid,
  .steps-grid {
    grid-template-columns: 1fr;
  }

  .dashboard-stats,
  .overview-grid {
    grid-template-columns: 1fr 1fr;
  }

  .hero-stats {
    flex-wrap: wrap;
    gap: 24px;
  }

  .achievement-card {
    flex-wrap: wrap;
  }

  .achievement-days {
    margin-left: 0;
  }

  .steps-timeline {
    display: none;
  }
}
</style>
