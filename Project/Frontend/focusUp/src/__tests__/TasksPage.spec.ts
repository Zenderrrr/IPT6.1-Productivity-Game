import { flushPromises } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import CreateCategory from '@/components/ui/CreateCategory.vue'
import CreateTask from '@/components/ui/CreateTask.vue'
import DeleteTask from '@/components/ui/DeleteTask.vue'
import Filter from '@/components/ui/Filter.vue'
import TasksComponent from '@/components/ui/TasksComponent.vue'
import UpdateTask from '@/components/ui/UpdateTaskComponent.vue'
import {
  button,
  category,
  dashboardRoutes,
  json,
  mockHttp,
  noContent,
  openPage,
  submit,
  task,
} from './testSupport'

const openTask = task({ categoryId: category.id, dueDate: new Date('2026-02-10T12:00:00Z') })
const shortTask = task({ id: 2, title: 'E-Mail schreiben', durationMin: 10 })
const doneTask = task({ id: 3, title: 'Bericht abgegeben', status: 3 })
const urgentTask = task({
  id: 4,
  title: 'Projekt planen',
  dueDate: new Date('2026-02-02T12:00:00Z'),
})

const taskRoutes = {
  ...dashboardRoutes,
  'GET /api/tasks': () => json([openTask, shortTask, doneTask, urgentTask]),
  'GET /api/categories': () => json([category]),
}

function taskTitles(page: Awaited<ReturnType<typeof openPage>>) {
  return page.findAllComponents(TasksComponent).map((card) => card.props('taskTitle'))
}

describe('Task-Übersicht', () => {
  it('zeigt standardmässig offene Aufgaben und kann zur erledigten Ansicht wechseln', async () => {
    mockHttp(taskRoutes)
    const page = await openPage('/tasks', true)
    expect(taskTitles(page)).toEqual([openTask.title, shortTask.title, urgentTask.title])

    await page
      .findAll('main span')
      .find((tab) => tab.text() === 'Erledigt')!
      .trigger('click')
    expect(taskTitles(page)).toEqual([doneTask.title])
    expect(page.getComponent(TasksComponent).get('input[type="checkbox"]').element).toHaveProperty(
      'disabled',
      true,
    )
  })

  it('sucht ohne Beachtung der Gross- und Kleinschreibung und zeigt einen leeren Trefferstand', async () => {
    mockHttp(taskRoutes)
    const page = await openPage('/tasks', true)
    const search = page.get('input[placeholder="Tasks suchen ..."]')
    await search.setValue('mAtHe')
    expect(taskTitles(page)).toEqual([openTask.title])
    await search.setValue('unbekannter Suchbegriff')
    expect(taskTitles(page)).toEqual([])
    await search.setValue('')
    expect(taskTitles(page)).toHaveLength(3)
  })

  it('filtert über die Kategorieauswahl und kann den Filter zurücksetzen', async () => {
    mockHttp(taskRoutes)
    const page = await openPage('/tasks', true)
    const select = page.get('select[name="categories"]')
    await select.setValue(category.name)
    expect(taskTitles(page)).toEqual([openTask.title])
    expect(page.getComponent(TasksComponent).text()).toContain(category.name)
    await select.setValue('')
    expect(taskTitles(page)).toHaveLength(3)
  })

  it('sortiert nach Fälligkeit und setzt Tasks ohne Deadline ans Ende', async () => {
    mockHttp(taskRoutes)
    const page = await openPage('/tasks', true)
    await button(page, 'nach Datum').trigger('click')
    expect(taskTitles(page)).toEqual([urgentTask.title, openTask.title, shortTask.title])
  })

  it('wendet den Schnellfilter für kurze Aufgaben an und entfernt ihn wieder', async () => {
    mockHttp(taskRoutes)
    const page = await openPage('/tasks', true)
    await button(page, 'Filter').trigger('click')
    const filter = page.getComponent(Filter)
    expect(filter.props('isShown')).toBe(true)
    await button(filter, 'Kurze Aufgaben').trigger('click')
    await button(filter, 'Anwenden').trigger('click')
    expect(taskTitles(page)).toEqual([shortTask.title])
    await button(page, 'Filter').trigger('click')
    expect(taskTitles(page)).toHaveLength(3)
  })

  it('legt über das Formular eine Task an und lädt die aktualisierte Liste', async () => {
    const newTask = task({ id: 10, title: 'Neuer Lernplan', durationMin: 45, difficulty: 2 })
    const tasks = [openTask]
    const requests = mockHttp({
      ...taskRoutes,
      'GET /api/tasks': () => json(tasks),
      'POST /api/tasks': () => {
        tasks.push(newTask)
        return json(newTask.id, 201)
      },
    })
    const page = await openPage('/tasks', true)
    await button(page, 'Neue Task').trigger('click')
    const form = page.getComponent(CreateTask)
    expect(form.props('isShown')).toBe(true)
    await form.get<HTMLInputElement>('input[type="text"]').setValue(newTask.title)
    await form.get('input[type="number"]').setValue('45')
    await button(form, 'Medium').trigger('click')
    await submit(form)

    expect(requests('POST', '/api/tasks').map((request) => request.body)).toEqual([
      { title: newTask.title, description: '', durationMin: 45, difficulty: 'Medium' },
    ])
    expect(form.props('isShown')).toBe(false)
    expect(taskTitles(page)).toContain(newTask.title)
  })

  it('behält das Formular und die Eingaben bei einem abgelehnten Speicherversuch', async () => {
    mockHttp({ ...taskRoutes, 'POST /api/tasks': () => json({ message: 'Ungültige Task' }, 400) })
    const page = await openPage('/tasks', true)
    await button(page, 'Neue Task').trigger('click')
    const form = page.getComponent(CreateTask)
    await form.get<HTMLInputElement>('input[type="text"]').setValue('Task mit Fehler')
    await form.get('input[type="number"]').setValue('30')
    await button(form, 'Easy').trigger('click')
    await submit(form)

    expect(form.props('isShown')).toBe(true)
    expect(form.get<HTMLInputElement>('input[type="text"]').element.value).toBe('Task mit Fehler')
    expect(form.text()).toContain('Die Task konnte nicht gespeichert werden.')
    expect(taskTitles(page)).not.toContain('Task mit Fehler')
  })

  it('behandelt eine bereits gespeicherte Task trotz fehlgeschlagener Aktualisierung als gespeichert', async () => {
    let created = false
    const requests = mockHttp({
      ...taskRoutes,
      'GET /api/tasks': () => (created ? json({}, 503) : json([openTask])),
      'POST /api/tasks': () => {
        created = true
        return json(10, 201)
      },
    })
    const page = await openPage('/tasks', true)
    await button(page, 'Neue Task').trigger('click')
    const form = page.getComponent(CreateTask)
    await form.get('input[type="text"]').setValue('Gespeicherte Task')
    await form.get('input[type="number"]').setValue('30')
    await button(form, 'Easy').trigger('click')
    await submit(form)

    expect(requests('POST', '/api/tasks')).toHaveLength(1)
    expect(requests('GET', '/api/tasks')).toHaveLength(2)
    expect(form.props('isShown')).toBe(false)
    expect(taskTitles(page)).toEqual([openTask.title])
  })

  it('öffnet die Bearbeitung mit bestehenden Werten und übernimmt die Änderung', async () => {
    const updated = { ...openTask, title: 'Analysis üben' }
    let tasks = [openTask]
    const requests = mockHttp({
      ...taskRoutes,
      'GET /api/tasks': () => json(tasks),
      'PUT /api/tasks/1': () => {
        tasks = [updated]
        return json(updated)
      },
    })
    const page = await openPage('/tasks', true)
    await page.getComponent(TasksComponent).get('button:has(.fa-pencil)').trigger('click')
    const form = page.getComponent(UpdateTask)
    const input = form.get<HTMLInputElement>('input[type="text"]')
    expect(input.element.value).toBe(openTask.title)
    await input.setValue(updated.title)
    await submit(form)

    expect(requests('PUT', '/api/tasks/1')[0]?.body).toMatchObject({ title: updated.title })
    expect(taskTitles(page)).toEqual([updated.title])
    expect(page.findComponent(UpdateTask).exists()).toBe(false)
  })

  it('löscht erst nach der Bestätigung und behält andere Tasks', async () => {
    const requests = mockHttp({ ...taskRoutes, 'DELETE /api/tasks/1': () => noContent() })
    const page = await openPage('/tasks', true)
    await page.getComponent(TasksComponent).get('button:has(.fa-trash-can)').trigger('click')
    const dialog = page.getComponent(DeleteTask)
    expect(requests('DELETE', '/api/tasks/1')).toHaveLength(0)
    await button(dialog, 'Endgültig Löschen').trigger('click')
    await flushPromises()

    expect(requests('DELETE', '/api/tasks/1')).toHaveLength(1)
    expect(taskTitles(page)).toEqual([shortTask.title, urgentTask.title])
    expect(dialog.props('isShown')).toBe(false)
  })

  it('behält die Task und den Bestätigungsdialog bei einem Löschfehler', async () => {
    mockHttp({ ...taskRoutes, 'DELETE /api/tasks/1': () => json({}, 500) })
    const page = await openPage('/tasks', true)
    await page.getComponent(TasksComponent).get('button:has(.fa-trash-can)').trigger('click')
    const dialog = page.getComponent(DeleteTask)
    await button(dialog, 'Endgültig Löschen').trigger('click')
    await flushPromises()

    expect(taskTitles(page)).toContain(openTask.title)
    expect(dialog.props('isShown')).toBe(true)
  })

  it('erstellt eine Kategorie mit eigener Farbe und lädt die Optionen neu', async () => {
    const newCategory = { ...category, id: 9, name: 'Lernen', color: '#123456' }
    const categories = [category]
    const requests = mockHttp({
      ...taskRoutes,
      'GET /api/categories': () => json(categories),
      'POST /api/categories': () => {
        categories.push(newCategory)
        return json(newCategory.id, 201)
      },
    })
    const page = await openPage('/tasks', true)
    const select = page.get('select[name="categories"]')
    await select.setValue('new')
    const form = page.getComponent(CreateCategory)
    expect(form.props('isShown')).toBe(true)
    await form.get<HTMLInputElement>('input[type="text"]').setValue(newCategory.name)
    await form.get('input[type="color"]').setValue(newCategory.color)
    await submit(form)

    expect(requests('POST', '/api/categories')[0]?.body).toEqual({
      name: newCategory.name,
      color: newCategory.color,
    })
    expect(select.text()).toContain(newCategory.name)
    expect(form.props('isShown')).toBe(false)
  })

  it('behält die Kategorieeingaben bei einer abgelehnten Anlage', async () => {
    mockHttp({ ...taskRoutes, 'POST /api/categories': () => json({}, 409) })
    const page = await openPage('/tasks', true)
    await page.get('select[name="categories"]').setValue('new')
    const form = page.getComponent(CreateCategory)
    await form.get<HTMLInputElement>('input[type="text"]').setValue('Schule')
    await form.get('input[type="color"]').setValue('#123456')
    await submit(form)

    expect(form.props('isShown')).toBe(true)
    expect(form.get<HTMLInputElement>('input[type="text"]').element.value).toBe('Schule')
  })

  it('speichert die Auswahl pro Nutzer und lädt nach dem Abschluss ohne Badge die Liste neu', async () => {
    const completed = { ...openTask, status: 3, completedAt: new Date() }
    let tasks = [openTask, shortTask]
    const requests = mockHttp({
      ...taskRoutes,
      'GET /api/tasks': () => json(tasks),
      'POST /api/tasks/1/complete': () => {
        tasks = [completed, shortTask]
        return json({
          task: completed,
          xp: 35,
          streakCountBefore: 3,
          streakCountAfter: 3,
          isStreakIncreased: false,
          badges: [],
        })
      },
    })
    const page = await openPage('/tasks', true)
    await page.getComponent(TasksComponent).get('input[type="checkbox"]').trigger('click')
    expect(JSON.parse(localStorage.getItem('checkedTasks_7') ?? '[]')).toEqual([1])
    await button(page, 'Tasks abschliessen').trigger('click')
    await flushPromises()
    expect(requests('POST', '/api/tasks/1/complete')).toHaveLength(1)
    expect(localStorage.getItem('checkedTasks_7')).toBeNull()

    await button(page, 'Weiter').trigger('click')
    await button(page, 'Weiter').trigger('click')
    await flushPromises()

    expect(taskTitles(page)).toEqual([shortTask.title])
    expect(requests('GET', '/api/tasks')).toHaveLength(2)
  })
})
