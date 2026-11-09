import { enableAutoUnmount, flushPromises, mount, type VueWrapper } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import { afterEach, beforeEach, expect, vi } from 'vitest'
import { defineComponent, h } from 'vue'
import App from '@/App.vue'
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import type { Badge } from '@/types/badge'
import type { Category } from '@/types/category'
import type { Dashboard } from '@/types/dashboard'
import type { Productivity } from '@/types/productivity'
import type { Stats } from '@/types/stats'
import type { Task } from '@/types/task'
import type { TaskLog } from '@/types/taskLog'
import type { User } from '@/types/user'

export const user: User = { id: 7, username: 'NoraTest', email: 'nora@example.test' }
export const category: Category = {
  id: 3,
  userId: user.id,
  name: 'Schule',
  color: '#14b8a6',
  createdAt: '2026-01-01T12:00:00Z',
}

export function task(overrides: Partial<Task> = {}): Task {
  return {
    id: 1,
    title: 'Mathe üben',
    description: 'Kapitel 3',
    difficulty: 1,
    durationMin: 30,
    status: 1,
    createdAt: new Date('2026-01-01T12:00:00Z'),
    updatedAt: new Date('2026-01-01T12:00:00Z'),
    xp: 35,
    ...overrides,
  }
}

export function taskLog(taskId: number, xpAwarded = 35): TaskLog {
  return {
    id: taskId,
    userId: user.id,
    taskId,
    action: 'Completed',
    xpAwarded,
    createdAt: new Date('2026-01-02T12:00:00Z'),
    title: '',
  }
}

export function stats(overrides: Partial<Stats> = {}): Stats {
  return {
    tasksDone: 5,
    tasksOpen: 2,
    totalTimeMin: 120,
    totalXp: 160,
    streakCount: 3,
    bestStreak: 4,
    ...overrides,
  }
}

export function dashboard(overrides: Partial<Dashboard> = {}): Dashboard {
  return {
    ...stats(),
    level: 2,
    xpCurrent: 160,
    xpNext: 400,
    progressToNextLevel: 0.2,
    lastCompletedTasks: [],
    ...overrides,
  }
}

export function productivity(overrides: Partial<Productivity> = {}): Productivity {
  return {
    date: new Date('2026-01-05T12:00:00'),
    completedTasks: 2,
    xpGained: 35,
    timeSpent: 30,
    ...overrides,
  }
}

export function badge(id: number, name = `Badge Nummer ${id}`): Badge {
  return {
    id,
    name,
    description: 'Eine Belohnung',
    ruleType: 'TaskCount',
    ruleValue: '1',
    rarity: 'Common',
    primaryColor: '#14b8a6',
    secondaryColor: '#000000',
    createdAt: new Date('2026-01-01T12:00:00Z'),
    img: undefined,
  }
}

interface TestRequest {
  method: string
  url: URL
  headers: Headers
  body: unknown
  credentials: RequestCredentials | undefined
}

type Routes = Record<string, (request: TestRequest) => Response | Promise<Response>>

export function json(data: unknown, status = 200) {
  return Response.json(data, { status })
}

export function noContent() {
  return new Response(null, { status: 204 })
}

const unexpectedRequests: string[] = []

export function mockHttp(routes: Routes = {}) {
  const requests: TestRequest[] = []
  vi.stubGlobal(
    'fetch',
    vi.fn(async (input: string, init: RequestInit = {}) => {
      const method = init.method ?? 'GET'
      const url = new URL(input)
      const key = `${method} ${url.pathname}`
      const request = {
        method,
        url,
        headers: new Headers(init.headers),
        body: typeof init.body === 'string' ? JSON.parse(init.body) : undefined,
        credentials: init.credentials,
      }
      requests.push(request)

      const handler = routes[key]
      if (handler) return handler(request)

      unexpectedRequests.push(key)
      return json({ message: `Unerwartete Testanfrage: ${key}` }, 500)
    }),
  )
  return (method: string, path: string) =>
    requests.filter((request) => request.method === method && request.url.pathname === path)
}

export const sessionRoutes = { 'GET /api/Auth/me': () => json(user) }

const chartStub = defineComponent({
  name: 'ChartStub',
  props: { option: { type: Object, required: true } },
  setup: () => () => h('div', { 'aria-label': 'Diagramm' }),
})

export function charts(wrapper: VueWrapper) {
  return wrapper.findAllComponents(chartStub).map((chart) => chart.props('option'))
}

let pinia: ReturnType<typeof createPinia>

enableAutoUnmount(afterEach)

beforeEach(() => {
  localStorage.clear()
  document.documentElement.classList.remove('dark')
  pinia = createPinia()
  setActivePinia(pinia)
  vi.stubGlobal(
    'matchMedia',
    vi.fn(() => ({ matches: false })),
  )
  vi.stubGlobal(
    'IntersectionObserver',
    class {
      observe() {}
      disconnect() {}
    },
  )
  mockHttp()
})

afterEach(() => {
  vi.unstubAllGlobals()
  vi.restoreAllMocks()
  expect(unexpectedRequests.splice(0), 'Unerwartete HTTP-Anfragen').toEqual([])
})

export async function openPage(path: string, authenticated = false) {
  if (authenticated) {
    localStorage.setItem('token', 'test-token')
    useAuthStore(pinia).$patch({ token: 'test-token', user })
  }

  await router.push(path)

  const wrapper = mount(App, {
    global: { plugins: [pinia, router], stubs: { Echarts: chartStub } },
  })
  await flushPromises()
  return wrapper
}

export function button(wrapper: Pick<VueWrapper, 'findAll'>, label: string) {
  const match = wrapper.findAll('button').find((candidate) => candidate.text() === label)
  if (!match) throw new Error(`Schaltfläche fehlt: ${label}`)
  return match
}

export async function submit(wrapper: Pick<VueWrapper, 'get'>) {
  await wrapper.get('form').trigger('submit')
  await flushPromises()
}

export const dashboardRoutes = {
  ...sessionRoutes,
  'GET /api/dashboard': () => json(dashboard()),
  'GET /api/stats': () => json(stats()),
  'GET /api/stats/productivity': () => json([]),
  'GET /api/tasks': () => json([]),
  'GET /api/categories': () => json([]),
}
