import type { Task } from '@/types/task.ts'
import type { Badge } from '@/types/badge.ts'

export type TaskCompleteType = {
  streakCountBefore: number,
  streakCountAfter: number,
  isStreakIncreased: boolean,
  task: Task,
  xp: number,
  badges: Badge[],
}
