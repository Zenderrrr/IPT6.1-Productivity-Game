export type CreateTask = {
  title: string,
  description: string,
  difficulty: string,
  durationMin: number,
  categoryId?: number,
  dueDate?: string,
}
