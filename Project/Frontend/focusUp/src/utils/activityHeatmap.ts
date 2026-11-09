import type { Productivity } from '@/types/productivity'

export const activityHeatmapDays = 91

const millisecondsPerDay = 86_400_000

// Project local calendar dates onto UTC so 23- and 25-hour days have equal width.
function calendarDay(date: Date) {
  return Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()) / millisecondsPerDay
}

export function buildActivityHeatmap(records: readonly Productivity[], today: Date) {
  const lastDay = calendarDay(today)
  const firstDay = lastDay - activityHeatmapDays + 1
  const firstWeekday = (new Date(firstDay * millisecondsPerDay).getUTCDay() + 6) % 7
  const firstMonday = firstDay - firstWeekday
  const weekCount = Math.ceil((activityHeatmapDays + firstWeekday) / 7)
  const weeks = Array.from({ length: weekCount }, (_, index) => `W${index + 1}`)

  const cells = records
    .map(({ date, xpGained }) => ({ day: calendarDay(date), xp: xpGained }))
    .filter(({ day }) => day >= firstDay && day <= lastDay)
    .sort((a, b) => a.day - b.day)
    .map(({ day, xp }) => {
      const offset = day - firstMonday
      const value: [week: number, weekday: number, xp: number] = [
        Math.floor(offset / 7),
        offset % 7,
        xp,
      ]
      return { value, date: new Date(day * millisecondsPerDay) }
    })

  const maxXp = Math.max(10, ...cells.map(({ value }) => value[2]))
  return { weeks, cells, maxXp }
}
