export type CreateTask = {
  title: string,
  description: string,
  difficulty: number,
  durationMin: number,
  categoryId?: number,
  dueDate?: Date,
}
