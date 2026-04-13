import { TaskLog } from '@/types/taskLog.ts'

export type Dashboard = {
  totalXp: number,
  tasksDone: number,
  tasksOpen: number,
  streakCount: number,
  bestStreak: number,
  totalTimeMin: number,
  tasks: TaskLog[],
  level: number,
  xpNext: number,
  progressToNextLevel: number,
  xpCurrent: number,
}
