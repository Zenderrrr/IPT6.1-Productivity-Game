export type CreateTaskType = {
  title: string,
  description: string,
  difficulty: string,
  durationMin: number,
  categoryId?: number,
  dueDate?: string,
}
