export type Task = {
  id: number,
  title: string,
  description: string,
  difficulty: number,
  durationMin: number,
  categoryId?: number,
  status: number,
  dueDate?: Date,
  createdAt: Date,
  updatedAt: Date,
  completedAt?: Date,
  xp: number,
}
