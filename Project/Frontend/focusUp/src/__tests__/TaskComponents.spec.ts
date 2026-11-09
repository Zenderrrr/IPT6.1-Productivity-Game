import { flushPromises, mount } from '@vue/test-utils'
import { afterEach, describe, expect, it, vi } from 'vitest'
import BadgeUpdate from '@/components/ui/CompleteTaskComponents/BadgeUpdate.vue'
import LevelIncrease from '@/components/ui/CompleteTaskComponents/LevelIncrease.vue'
import StreakUpdate from '@/components/ui/CompleteTaskComponents/StreakUpdate.vue'
import TaskCompleteHandler from '@/components/ui/CompleteTaskComponents/TaskCompleteHandler.vue'
import XpUpdate from '@/components/ui/CompleteTaskComponents/XpUpdate.vue'
import CreateTask from '@/components/ui/CreateTask.vue'
import Filter from '@/components/ui/Filter.vue'
import type { Task } from '@/types/task'
import type { TaskCompleteType } from '@/types/taskComplete'
import { badge, button, category, json, mockHttp, submit, task } from './testSupport'

afterEach(() => vi.useRealTimers())

describe('Task-Formular', () => {
  it('verlangt Schwierigkeit und Dauer, bevor die Task abgesendet werden kann', async () => {
    mockHttp({ 'GET /api/categories': () => json([]) })
    const onSubmit = vi.fn(async () => true)
    const form = mount(CreateTask, { props: { isShown: true, onSubmit } })
    await flushPromises()
    await form.get('input[type="text"]').setValue('Lernen')
    await submit(form)
    expect(form.text()).toContain('Schwierigkeit nicht festgelegt.')

    await button(form, 'Easy').trigger('click')
    await submit(form)
    expect(form.text()).toContain('Dauer ist leer.')
    expect(onSubmit).not.toHaveBeenCalled()
  })

  it('übergibt optionale Kategorie und Deadline und setzt die Eingaben bei Erfolg zurück', async () => {
    mockHttp({ 'GET /api/categories': () => json([category]) })
    const onSubmit = vi.fn(async () => true)
    const form = mount(CreateTask, { props: { isShown: true, onSubmit } })
    await flushPromises()
    await form.get('input[type="text"]').setValue('Lernen')
    await form.get('textarea').setValue('Kapitel 3')
    await form.get('input[type="number"]').setValue('30')
    await button(form, 'Easy').trigger('click')
    await form.get('select').setValue(String(category.id))
    await form.get('input[type="date"]').setValue('2026-02-10')
    await submit(form)

    expect(onSubmit).toHaveBeenCalledExactlyOnceWith({
      title: 'Lernen',
      description: 'Kapitel 3',
      durationMin: 30,
      difficulty: 'Easy',
      categoryId: category.id,
      dueDate: '2026-02-10',
    })
    expect(form.get<HTMLInputElement>('input[type="text"]').element.value).toBe('')
  })
})

describe('Task-Filter', () => {
  function openFilter(tasks: Task[]) {
    return mount(Filter, { props: { isShown: true, tasks, categories: [category] } })
  }

  it('kombiniert Status, Kategorie und Dauer und kann alle Filter zurücksetzen', async () => {
    const tasks = [
      task({ id: 1, categoryId: category.id, durationMin: 30 }),
      task({ id: 2, categoryId: category.id, durationMin: 60 }),
      task({ id: 3, categoryId: category.id, durationMin: 61 }),
      task({ id: 4, categoryId: category.id, durationMin: 45, status: 3 }),
      task({ id: 5, durationMin: 45 }),
    ]
    const filter = openFilter(tasks)
    await button(filter, 'Erweiterter Filter').trigger('click')
    await filter.get('input[name="status"][value="Open"]').setValue()
    await filter.get(`input[name="category"][value="${category.id}"]`).setValue()
    await filter.get('input[name="time"][value="30-60"]').setValue()
    await button(filter, 'Anwenden').trigger('click')

    expect(filter.emitted('submit')).toEqual([[[tasks[0], tasks[1]]]])

    await button(filter, 'Alles zurücksetzen').trigger('click')
    await button(filter, 'Anwenden').trigger('click')
    expect(filter.emitted('submit')?.[1]).toEqual([tasks])
  })

  it.each([
    { label: 'Wenig XP', ids: [1, 2] },
    { label: 'Hohes XP', ids: [4, 5] },
    { label: 'Kurze Aufgaben', ids: [1, 3] },
  ])('berücksichtigt beim Schnellfilter $label die Grenzwerte', async ({ label, ids }) => {
    const tasks = [
      task({ id: 1, xp: 29, durationMin: 14 }),
      task({ id: 2, xp: 30, durationMin: 16 }),
      task({ id: 3, xp: 31, durationMin: 15 }),
      task({ id: 4, xp: 100 }),
      task({ id: 5, xp: 101 }),
      task({ id: 6, xp: 99 }),
    ]
    const filter = openFilter(tasks)
    await button(filter, label).trigger('click')
    await button(filter, 'Anwenden').trigger('click')
    expect(filter.emitted('submit')).toEqual([[tasks.filter(({ id }) => ids.includes(id))]])
  })

  it.each(['Schnellfilter', 'erweiterter Filter'])(
    'vergleicht bei %s Heute als Kalendertag',
    async (mode) => {
      vi.useFakeTimers({ toFake: ['Date'] })
      vi.setSystemTime(new Date(2026, 0, 7, 12))
      const today = task({ dueDate: new Date(2026, 0, 7, 18) })
      const filter = openFilter([
        today,
        task({ id: 2, dueDate: new Date(2026, 0, 8) }),
        task({ id: 3 }),
      ])

      if (mode === 'Schnellfilter') {
        await button(filter, 'Heute').trigger('click')
      } else {
        await button(filter, 'Erweiterter Filter').trigger('click')
        await filter.get('input[name="dueTo"][value="today"]').setValue()
      }

      await button(filter, 'Anwenden').trigger('click')
      expect(filter.emitted('submit')).toEqual([[[today]]])
    },
  )

  it('schliesst den ganzen letzten Tag eines gewählten Zeitraums ein', async () => {
    const first = task({ dueDate: new Date(2026, 0, 5, 0, 0) })
    const last = task({ id: 2, dueDate: new Date(2026, 0, 6, 23, 59) })
    const filter = openFilter([
      first,
      last,
      task({ id: 3, dueDate: new Date(2026, 0, 7, 0, 0) }),
      task({ id: 4, dueDate: new Date(2026, 0, 4, 23, 59) }),
    ])
    await button(filter, 'Erweiterter Filter').trigger('click')
    const [from, to] = filter.findAll('input[type="date"]')
    await from!.setValue('2026-01-05')
    await to!.setValue('2026-01-06')
    await button(filter, 'Anwenden').trigger('click')
    expect(filter.emitted('submit')).toEqual([[[first, last]]])
  })

  it('begrenzt diese Woche auf Montag bis Sonntag', async () => {
    vi.useFakeTimers({ toFake: ['Date'] })
    vi.setSystemTime(new Date(2026, 0, 7, 12))
    const monday = task({ dueDate: new Date(2026, 0, 5) })
    const sunday = task({ id: 2, dueDate: new Date(2026, 0, 11, 23, 59) })
    const filter = openFilter([
      monday,
      sunday,
      task({ id: 3, dueDate: new Date(2026, 0, 12) }),
      task({ id: 4, dueDate: new Date(2026, 0, 4, 23, 59) }),
      task({ id: 5, dueDate: new Date(2026, 0, 9), completedAt: new Date(2026, 0, 7) }),
    ])
    await button(filter, 'Erweiterter Filter').trigger('click')
    await filter.get('input[name="dueTo"][value="week"]').setValue()
    await button(filter, 'Anwenden').trigger('click')
    expect(filter.emitted('submit')).toEqual([[[monday, sunday]]])
  })
})

describe('Abschlussdialog', () => {
  it('zeigt Streak, XP und mehrere Badges in Reihenfolge und schliesst nach dem letzten Badge', async () => {
    const first = badge(1)
    const second = badge(2)
    mockHttp({
      'GET /uploads/badges/badge_nummer_1.svg': () => new Response('<svg/>'),
      'GET /uploads/badges/badge_nummer_2.svg': () => new Response('<svg/>'),
    })
    const result: TaskCompleteType = {
      task: task({ status: 3 }),
      xp: 35,
      streakCountBefore: 3,
      streakCountAfter: 4,
      isStreakIncreased: true,
      badges: [first],
    }
    const dialog = mount(TaskCompleteHandler, {
      props: {
        tasksCompleteArr: [
          result,
          {
            ...result,
            task: task({ id: 2, status: 3 }),
            xp: 40,
            isStreakIncreased: false,
            badges: [second],
          },
        ],
      },
      global: {
        stubs: { StreakUpdate: true, XpUpdate: true, LevelIncrease: true, BadgeUpdate: true },
      },
    })
    await flushPromises()
    dialog.getComponent(StreakUpdate).vm.$emit('submit')
    await flushPromises()
    expect(dialog.getComponent(XpUpdate).props('task').id).toBe(1)
    dialog.getComponent(XpUpdate).vm.$emit('submit')
    await flushPromises()
    expect(dialog.getComponent(XpUpdate).props('task').id).toBe(2)
    dialog.getComponent(XpUpdate).vm.$emit('submit')
    await flushPromises()
    expect(dialog.getComponent(LevelIncrease).props('totalTaskXP')).toBe(75)
    dialog.getComponent(LevelIncrease).vm.$emit('submit')
    await flushPromises()
    expect(dialog.getComponent(BadgeUpdate).props('badgeName')).toBe(first.name)
    dialog.getComponent(BadgeUpdate).vm.$emit('submit')
    await flushPromises()
    expect(dialog.getComponent(BadgeUpdate).props('badgeName')).toBe(second.name)
    dialog.getComponent(BadgeUpdate).vm.$emit('submit')
    await flushPromises()
    expect(dialog.emitted('close')).toHaveLength(1)
  })
})
