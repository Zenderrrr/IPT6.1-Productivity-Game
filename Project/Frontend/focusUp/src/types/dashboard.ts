import { type TaskLog } from '@/types/taskLog.ts'

export type Dashboard = {
  totalXp: number,
  tasksDone: number,
  tasksOpen: number,
  streakCount: number,
  bestStreak: number,
  totalTimeMin: number,
  lastCompletedTasks: TaskLog[],
  level: number,
  xpNext: number,
  progressToNextLevel: number,
  xpCurrent: number,
}
