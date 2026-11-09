import { flushPromises, mount } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import { dashboard, json, mockHttp, openPage, sessionRoutes, stats } from './testSupport'

describe('Navigation und öffentliche Seiten', () => {
  it('verlinkt auf Anmeldung, Registrierung und Rechtsseiten', async () => {
    const page = await openPage('/')
    expect(page.get('h1').text()).toContain('Produktivität')
    for (const path of ['/login', '/register', '/legal-notice', '/privacy-policy']) {
      expect(page.find(`a[href="${path}"]`).exists()).toBe(true)
    }
    await page.get('a[href="/register"]').trigger('click')
    await flushPromises()
    expect(router.currentRoute.value.path).toBe('/register')
  })

  it('zeigt im Impressum den Kontakt und einen Rückweg', async () => {
    const page = await openPage('/legal-notice')
    expect(page.get('h1').text()).toBe('Impressum')
    expect(page.get('a[href="mailto:kontakt@focusup.ch"]').text()).toContain('kontakt@focusup.ch')
    expect(page.find('a[href="/"]').exists()).toBe(true)
  })

  it('verknüpft jeden Eintrag der Datenschutzerklärung mit seinem Abschnitt', async () => {
    const page = await openPage('/privacy-policy')
    expect(page.get('h1').text()).toBe('Datenschutzerklärung')
    const links = page.findAll('.index a')
    expect(links.length).toBeGreaterThan(0)
    for (const link of links) {
      const href = link.attributes('href') ?? ''
      expect(href).toMatch(/^#\w+/)
      expect(page.find(href).exists()).toBe(true)
    }
  })

  it('zeigt für eine unbekannte URL die 404-Seite mit Rückwegen', async () => {
    const page = await openPage('/unbekannte-seite')
    expect(router.currentRoute.value.name).toBe('NotFound')
    expect(page.get('h2').text()).toContain('Diese Seite hat deinen Fokus verloren')
    expect(page.find('a[href="/"]').exists()).toBe(true)
    expect(page.find('a[href="/dashboard"]').exists()).toBe(true)
  })

  it.each(['/dashboard', '/tasks', '/stats', '/profile', '/allCompletedTasks'])(
    'leitet Gäste von %s zur Anmeldung',
    async (path) => {
      const page = await openPage(path)
      expect(router.currentRoute.value.path).toBe('/login')
      expect(page.get('h1').text()).toContain('Willkommen')
    },
  )

  it('leitet eine aktive Sitzung von der Landingpage zum Dashboard', async () => {
    mockHttp({
      ...sessionRoutes,
      'GET /api/dashboard': () => json(dashboard()),
      'GET /api/stats': () => json(stats()),
      'GET /api/stats/productivity': () => json([]),
      'GET /api/tasks': () => json([]),
      'GET /api/categories': () => json([]),
    })
    const page = await openPage('/', true)
    expect(router.currentRoute.value.path).toBe('/dashboard')
    expect(page.text()).toContain('Schnelle Aktionen')
  })

  it('stellt beim Einstieg mit gespeichertem Token den Benutzer wieder her', async () => {
    const requests = mockHttp({
      ...sessionRoutes,
      'GET /api/tasks': () => json([]),
      'GET /api/categories': () => json([]),
      'GET /api/dashboard': () => json(dashboard()),
      'GET /api/stats/productivity': () => json([]),
    })
    localStorage.setItem('token', 'saved-token')
    await openPage('/tasks')
    expect(router.currentRoute.value.path).toBe('/tasks')
    expect(useAuthStore().user?.username).toBe('NoraTest')
    expect(requests('GET', '/api/Auth/me')[0]?.headers.get('Authorization')).toBe(
      'Bearer saved-token',
    )
  })
})
