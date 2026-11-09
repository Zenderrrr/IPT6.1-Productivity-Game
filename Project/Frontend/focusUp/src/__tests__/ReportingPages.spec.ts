import { flushPromises } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import CreateTask from '@/components/ui/CreateTask.vue'
import CurrentWeekStats from '@/components/ui/CurrentWeekStats.vue'
import Loading from '@/components/ui/Loading.vue'
import StatsCard from '@/components/ui/StatsCard.vue'
import StatsOverviewCard from '@/components/ui/StatsOverviewCard.vue'
import {
  button,
  category,
  charts,
  dashboard,
  dashboardRoutes,
  json,
  mockHttp,
  openPage,
  productivity,
  stats,
  submit,
  task,
  taskLog,
} from './testSupport'

const earlier = new Date('2026-01-05T12:00:00')
const later = new Date('2026-01-06T12:00:00')
const completed = task({
  id: 8,
  title: 'Bericht abschliessen',
  status: 3,
  categoryId: category.id,
  dueDate: earlier,
  completedAt: earlier,
  durationMin: 45,
})
const completedWithoutCategory = task({
  id: 9,
  title: 'Notizen ordnen',
  status: 3,
  completedAt: later,
  durationMin: 20,
})
const historyRoutes = {
  ...dashboardRoutes,
  'GET /api/dashboard': () =>
    json(dashboard({ lastCompletedTasks: [taskLog(8, 55), taskLog(9, 20)] })),
  'GET /api/tasks/8': () => json(completed),
  'GET /api/tasks/9': () => json(completedWithoutCategory),
  'GET /api/categories/3': () => json(category),
}

describe('Dashboard', () => {
  it('zeigt Laden bis die Kennzahlen verfügbar sind', async () => {
    let resolveDashboard!: (response: Response) => void
    const pending = new Promise<Response>((resolve) => {
      resolveDashboard = resolve
    })
    mockHttp({ ...dashboardRoutes, 'GET /api/dashboard': () => pending })

    const page = await openPage('/dashboard', true)
    expect(page.findComponent(Loading).exists()).toBe(true)
    expect(page.find('main').exists()).toBe(false)

    resolveDashboard(json(dashboard()))
    await flushPromises()
    expect(page.find('main').exists()).toBe(true)
    expect(page.text()).toContain('NoraTest')
    expect(page.findComponent(Loading).exists()).toBe(false)
  })

  it('zeigt Kennzahlen, erledigte Tasks und die Navigation zur vollständigen Liste', async () => {
    mockHttp(historyRoutes)
    const page = await openPage('/dashboard', true)
    const cards = page.findAllComponents(StatsOverviewCard)
    expect(cards.find((card) => card.props('statsName') === 'Gesamt XP')?.props('statsValue')).toBe(
      160,
    )
    expect(cards.find((card) => card.props('statsName') === 'Level')?.props('statsValue')).toBe(2)
    expect(page.text()).toContain(completed.title)
    expect(page.find('a[href="/allCompletedTasks"]').exists()).toBe(true)
    expect(page.find('a[href="/tasks"]').exists()).toBe(true)
  })

  it.each([
    {
      tasks: [
        completed,
        task({ id: 2, completedAt: later, dueDate: earlier }),
        completedWithoutCategory,
      ],
      rate: 50,
    },
    { tasks: [completedWithoutCategory, task({ id: 2, dueDate: earlier })], rate: 100 },
  ])('zeigt die erwartete Erfolgsquote von $rate Prozent', async ({ tasks, rate }) => {
    mockHttp({ ...dashboardRoutes, 'GET /api/tasks': () => json(tasks) })
    const page = await openPage('/dashboard', true)
    const card = page
      .findAllComponents(CurrentWeekStats)
      .find((item) => item.props('name') === 'Erfolgsquote')
    expect(card?.props('statsValue')).toBe(rate)
  })

  it('wechselt den Diagrammzeitraum und reicht die XP-Daten an das Diagramm weiter', async () => {
    const requests = mockHttp({
      ...dashboardRoutes,
      'GET /api/stats/productivity': ({ url }) =>
        json([productivity({ xpGained: Number(url.searchParams.get('rangeInDays')) })]),
    })
    const page = await openPage('/dashboard', true)

    expect(charts(page)[0]?.series[0].data).toEqual([14])
    await button(page, '1M').trigger('click')
    await flushPromises()
    expect(charts(page)[0]?.series[0].data).toEqual([30])
    await button(page, '3M').trigger('click')
    await flushPromises()
    expect(charts(page)[0]?.series[0].data).toEqual([90])
    expect(
      requests('GET', '/api/stats/productivity').map(({ url }) =>
        url.searchParams.get('rangeInDays'),
      ),
    ).toEqual(expect.arrayContaining(['14', '30', '90']))
  })

  it('öffnet die Schnellerfassung und übernimmt die Task-Eingaben', async () => {
    const requests = mockHttp({ ...dashboardRoutes, 'POST /api/tasks': () => json(20, 201) })
    const page = await openPage('/dashboard', true)
    await page
      .findAll('main span')
      .find((span) => span.text() === 'Task erstellen')!
      .trigger('click')
    const form = page.getComponent(CreateTask)
    expect(form.props('isShown')).toBe(true)
    await form.get('input[type="text"]').setValue('Dashboard-Task')
    await form.get('input[type="number"]').setValue('20')
    await button(form, 'Hard').trigger('click')
    await submit(form)

    expect(requests('POST', '/api/tasks')[0]?.body).toMatchObject({
      title: 'Dashboard-Task',
      difficulty: 'Hard',
      durationMin: 20,
    })
    expect(form.props('isShown')).toBe(false)
  })
})

describe('Statistik', () => {
  it('lädt Vergleichszeiträume, berechnet Trends und übergibt Diagrammdaten', async () => {
    const requests = mockHttp({
      ...historyRoutes,
      'GET /api/stats': ({ url }) =>
        json(
          url.searchParams.get('rangeInDays') === '14'
            ? stats({ tasksDone: 10, totalXp: 100 })
            : stats({ tasksDone: 7, totalXp: 80 }),
        ),
      'GET /api/stats/productivity': ({ url }) =>
        json(
          url.searchParams.get('rangeInDays') === '91'
            ? [
                productivity({ date: later, xpGained: 25 }),
                productivity({ date: earlier, xpGained: 15 }),
              ]
            : [
                productivity({ date: earlier, completedTasks: 1, xpGained: 25 }),
                productivity({ date: later, completedTasks: 3, xpGained: 50 }),
              ],
        ),
    })
    const page = await openPage('/stats', true)

    const cards = page.findAllComponents(StatsCard)
    expect(cards.find((card) => card.props('title') === 'Erledigte Task')?.text()).toContain('133')
    expect(cards.find((card) => card.props('title') === 'Gesamt XP')?.text()).toContain('300')
    expect(
      requests('GET', '/api/stats').map(({ url }) => url.searchParams.get('rangeInDays')),
    ).toEqual(['14', '7'])
    expect(
      requests('GET', '/api/stats/productivity').map(({ url }) =>
        url.searchParams.get('rangeInDays'),
      ),
    ).toEqual(['7', '91'])

    const [bars, categories, xp, heatmap] = charts(page)
    expect(bars?.series[0].data).toEqual([1, 3])
    expect(categories?.series[0].data).toEqual([
      { name: 'Schule', value: 1 },
      { name: 'Keine', value: 1 },
    ])
    expect(xp?.series.map((serie: { data: number[] }) => serie.data)).toEqual([
      [25, 75],
      [25, 50],
    ])
    expect(heatmap?.series[0].data).toEqual([
      [0, 0, 15],
      [0, 1, 25],
    ])
    expect(heatmap?.visualMap.max).toBe(25)
    expect(page.get('table').text()).toContain(completed.title)
  })

  it('zeigt bei leeren Statistikdaten weder ungültige Zahlen noch Datenpunkte', async () => {
    mockHttp({
      ...dashboardRoutes,
      'GET /api/stats': () =>
        json(stats({ tasksDone: 0, totalXp: 0, totalTimeMin: 0, streakCount: 0, bestStreak: 0 })),
    })
    const page = await openPage('/stats', true)
    expect(page.get('main').text()).not.toMatch(/NaN|Infinity/)
    const [bars, categories, xp, heatmap] = charts(page)
    expect(bars?.series[0].data).toEqual([])
    expect(categories?.series[0].data).toEqual([])
    expect(xp?.series[0].data).toEqual([])
    expect(heatmap?.series[0].data).toEqual([])
    expect(heatmap?.visualMap.max).toBe(10)
  })
})

describe('Verlauf erledigter Tasks', () => {
  it('zeigt Titel, Kategorie, Dauer und XP pro Zeile', async () => {
    const requests = mockHttp(historyRoutes)
    const page = await openPage('/allCompletedTasks', true)
    expect(requests('GET', '/api/dashboard')[0]?.url.searchParams.get('taskLogLimit')).toBe('200')
    const rows = page.findAll('tbody tr')
    expect(
      rows.map((row) =>
        row
          .findAll('td')
          .slice(1)
          .map((cell) => cell.text()),
      ),
    ).toEqual([
      [completed.title, 'Schule', '45 min', '+ 55 XP'],
      [completedWithoutCategory.title, 'Keine', '20 min', '+ 20 XP'],
    ])
  })

  it('zeigt bei einem leeren Verlauf nur die Tabellenüberschriften', async () => {
    mockHttp(dashboardRoutes)
    const page = await openPage('/allCompletedTasks', true)
    expect(page.findAll('thead th').map((heading) => heading.text())).toEqual([
      'Datum',
      'Task',
      'Kategorie',
      'Dauer',
      'XP',
    ])
    expect(page.findAll('tbody tr')).toHaveLength(0)
  })

  it.each(['/allCompletedTasks', '/stats'])(
    'behält in %s den Verlauf bei einer nicht verfügbaren Task',
    async (path) => {
      mockHttp({ ...historyRoutes, 'GET /api/tasks/8': () => json({}, 404) })
      const page = await openPage(path, true)
      const rows = page.findAll('tbody tr')
      expect(rows[0]?.text()).toContain('Task nicht verfügbar')
      expect(rows[0]?.text()).toContain('+ 55 XP')
      expect(rows[1]?.text()).toContain(completedWithoutCategory.title)
    },
  )
})
