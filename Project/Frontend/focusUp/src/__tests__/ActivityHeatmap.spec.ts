import { describe, expect, it } from 'vitest'
import type { Productivity } from '@/types/productivity'
import { activityHeatmapDays, buildActivityHeatmap } from '@/utils/activityHeatmap'

function record(date: Date, xpGained = 20): Productivity {
  return { date, xpGained, completedTasks: 0, timeSpent: 0 }
}

describe('Kalenderdaten der Aktivitäts-Heatmap', () => {
  it('liefert auch ohne Daten ein Raster und eine gültige Farbskala', () => {
    const result = buildActivityHeatmap([], new Date(2026, 3, 5))

    expect(result.weeks).toHaveLength(13)
    expect(result.cells).toEqual([])
    expect(result.maxXp).toBe(10)
  })

  it('sortiert die Daten chronologisch, ohne die API-Liste zu verändern', () => {
    const monday = record(new Date(2026, 0, 5), 25)
    const sunday = record(new Date(2026, 0, 4), 15)
    const records = [monday, sunday]

    const { cells } = buildActivityHeatmap(records, new Date(2026, 3, 4))

    expect(cells.map(({ value }) => value)).toEqual([
      [0, 6, 15],
      [1, 0, 25],
    ])
    expect(cells.map(({ date }) => date.toISOString().slice(0, 10))).toEqual([
      '2026-01-04',
      '2026-01-05',
    ])
    expect(records).toEqual([monday, sunday])
  })

  it.each([
    { start: 'Sonntag', startDay: 4, weekCount: 14, firstCell: [0, 6, 1], lastCell: [13, 5, 91] },
    { start: 'Montag', startDay: 5, weekCount: 13, firstCell: [0, 0, 1], lastCell: [12, 6, 91] },
    { start: 'Dienstag', startDay: 6, weekCount: 14, firstCell: [0, 1, 1], lastCell: [13, 0, 91] },
    { start: 'Mittwoch', startDay: 7, weekCount: 14, firstCell: [0, 2, 1], lastCell: [13, 1, 91] },
    {
      start: 'Donnerstag',
      startDay: 8,
      weekCount: 14,
      firstCell: [0, 3, 1],
      lastCell: [13, 2, 91],
    },
    { start: 'Freitag', startDay: 9, weekCount: 14, firstCell: [0, 4, 1], lastCell: [13, 3, 91] },
    { start: 'Samstag', startDay: 10, weekCount: 14, firstCell: [0, 5, 1], lastCell: [13, 4, 91] },
  ])(
    'zeigt ab $start genau 91 Tage in $weekCount Kalenderwochen',
    ({ startDay, weekCount, firstCell, lastCell }) => {
      const records = Array.from({ length: activityHeatmapDays + 2 }, (_, index) =>
        record(new Date(2026, 0, startDay + index - 1, 18), index),
      )

      const { cells, weeks, maxXp } = buildActivityHeatmap(records, new Date(2026, 3, startDay))

      expect(weeks).toHaveLength(weekCount)
      expect(cells).toHaveLength(91)
      expect(cells[0]?.value).toEqual(firstCell)
      expect(cells[cells.length - 1]?.value).toEqual(lastCell)
      expect(new Set(cells.map(({ value }) => value.slice(0, 2).join(','))).size).toBe(91)
      expect(maxXp).toBe(91)
    },
  )

  it('hält die Spalten stabil, wenn Tage am Anfang, in der Mitte und am Ende fehlen', () => {
    const { cells, weeks } = buildActivityHeatmap(
      [record(new Date(2026, 0, 7), 0), record(new Date(2026, 0, 19), 9)],
      new Date(2026, 3, 4),
    )

    expect(weeks).toHaveLength(14)
    expect(cells.map(({ value }) => value)).toEqual([
      [1, 2, 0],
      [3, 0, 9],
    ])
  })

  it.each([0, 1, 9, 10, 11, 250])('berechnet die Farbskala bei maximal %i XP', (xp) => {
    const today = new Date(2026, 3, 4)
    const { maxXp } = buildActivityHeatmap([record(today, xp)], today)

    expect(maxXp).toBe(Math.max(10, xp))
  })

  it.each([
    { month: 2, saturday: 28, sunday: 29, monday: 30 },
    { month: 9, saturday: 24, sunday: 25, monday: 26 },
  ])(
    'behält die Tageszuordnung bei der Zeitumstellung im Monat $month',
    ({ month, saturday, sunday, monday }) => {
      const today = new Date(2026, month, monday)
      const { cells } = buildActivityHeatmap(
        [
          record(new Date(2026, month, saturday)),
          record(new Date(2026, month, sunday)),
          record(today),
        ],
        today,
      )

      expect(cells.map(({ value }) => value.slice(0, 2))).toEqual([
        [12, 5],
        [12, 6],
        [13, 0],
      ])
    },
  )

  it('ordnet den Jahreswechsel in derselben Kalenderwoche ein', () => {
    const today = new Date(2026, 0, 1)
    const { cells } = buildActivityHeatmap([record(new Date(2025, 11, 31)), record(today)], today)

    expect(cells.map(({ value }) => value.slice(0, 2))).toEqual([
      [13, 2],
      [13, 3],
    ])
  })

  it('ordnet den Schalttag als eigenen Kalendertag ein', () => {
    const today = new Date(2028, 2, 1)
    const { cells } = buildActivityHeatmap([record(new Date(2028, 1, 29)), record(today)], today)

    expect(cells.map(({ value }) => value.slice(0, 2))).toEqual([
      [13, 1],
      [13, 2],
    ])
  })
})
