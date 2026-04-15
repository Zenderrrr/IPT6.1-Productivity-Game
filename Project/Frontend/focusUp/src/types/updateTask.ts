export type UpdateTask = {
  title: string,
  description: string,
  difficulty: string,
  durationMin: number,
  categoryId?: number,
  dueDate?: string,
  status: string,
}
