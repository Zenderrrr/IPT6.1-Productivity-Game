import { flushPromises } from '@vue/test-utils'
import { describe, expect, it } from 'vitest'
import Badges from '@/components/ui/Badges.vue'
import DeleteUser from '@/components/ui/DeleteUser.vue'
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import {
  badge,
  button,
  dashboardRoutes,
  json,
  mockHttp,
  noContent,
  openPage,
  user,
} from './testSupport'

const profileRoutes = {
  ...dashboardRoutes,
  'GET /api/badges': () => json([]),
  'GET /api/badges/unlocked': () => json([]),
}

describe('Profil', () => {
  it('zeigt Accountdaten, Streak und freigeschaltete Badges und kann die Liste erweitern', async () => {
    const badges = Array.from({ length: 12 }, (_, index) => badge(index + 1))
    const iconRoutes = Object.fromEntries(
      badges.map(({ id }) => [
        `GET /uploads/badges/badge_nummer_${id}.svg`,
        () => new Response('<svg xmlns="http://www.w3.org/2000/svg"></svg>'),
      ]),
    )
    mockHttp({
      ...profileRoutes,
      ...iconRoutes,
      'GET /api/badges': () => json(badges),
      'GET /api/badges/unlocked': () => json(badges.slice(0, 1)),
    })
    const page = await openPage('/profile', true)
    expect(page.text()).toContain(user.username)
    expect(page.text()).toContain(user.email)
    expect(page.text()).toContain('Level 2')
    expect(page.text()).toContain('3-Tage-Streak')
    expect(page.findAllComponents(Badges)).toHaveLength(11)
    expect(
      page
        .findAllComponents(Badges)
        .slice(0, 2)
        .map((card) => card.props('checked')),
    ).toEqual([true, false])
    expect(page.getComponent(Badges).find('svg').exists()).toBe(true)

    await button(page, 'Alle anzeigen').trigger('click')
    expect(page.findAllComponents(Badges)).toHaveLength(12)
    await button(page, 'Weniger anzeigen').trigger('click')
    expect(page.findAllComponents(Badges)).toHaveLength(11)
  })

  it('speichert den Dark Mode und ändert das Dokument-Theme', async () => {
    mockHttp(profileRoutes)
    const page = await openPage('/profile', true)
    expect(document.documentElement.classList.contains('dark')).toBe(false)
    await button(page, 'Ändern').trigger('click')
    expect(document.documentElement.classList.contains('dark')).toBe(true)
    expect(localStorage.getItem('isDarkModeActive')).toBe('true')
    await button(page, 'Ändern').trigger('click')
    expect(document.documentElement.classList.contains('dark')).toBe(false)
  })

  it('meldet ab, löscht die Sitzung und hält geschützte Seiten geschlossen', async () => {
    const requests = mockHttp({ ...profileRoutes, 'POST /api/Auth/logout': () => noContent() })
    const page = await openPage('/profile', true)
    await button(page, 'Logout').trigger('click')
    await flushPromises()

    expect(requests('POST', '/api/Auth/logout')).toHaveLength(1)
    expect(localStorage.getItem('token')).toBeNull()
    expect(useAuthStore().user).toBeNull()
    expect(router.currentRoute.value.path).toBe('/login')
    await router.push('/dashboard')
    expect(router.currentRoute.value.path).toBe('/login')
  })

  it('bricht das Löschen ab und behält den Account', async () => {
    mockHttp(profileRoutes)
    const page = await openPage('/profile', true)
    await button(page, 'Löschen').trigger('click')
    const dialog = page.getComponent(DeleteUser)
    expect(dialog.props('isShown')).toBe(true)
    await dialog.get('button:has(.fa-x)').trigger('click')
    expect(dialog.props('isShown')).toBe(false)
    expect(useAuthStore().isAuth).toBe(true)
  })

  it('löscht den Account nach Bestätigung und beendet die lokale Sitzung', async () => {
    const requests = mockHttp({ ...profileRoutes, 'DELETE /api/Auth/me': () => noContent() })
    const page = await openPage('/profile', true)
    await button(page, 'Löschen').trigger('click')
    expect(requests('DELETE', '/api/Auth/me')).toHaveLength(0)
    await button(page.getComponent(DeleteUser), 'Endgültig Löschen').trigger('click')
    await flushPromises()

    expect(requests('DELETE', '/api/Auth/me')).toHaveLength(1)
    expect(localStorage.getItem('token')).toBeNull()
    expect(router.currentRoute.value.path).toBe('/login')
  })

  it('bleibt bei fehlgeschlagener Löschung im Profil und zeigt den Fehler', async () => {
    mockHttp({ ...profileRoutes, 'DELETE /api/Auth/me': () => json({}, 500) })
    const page = await openPage('/profile', true)
    await button(page, 'Löschen').trigger('click')
    const dialog = page.getComponent(DeleteUser)
    await button(dialog, 'Endgültig Löschen').trigger('click')
    await flushPromises()

    expect(dialog.get('[role="alert"]').text()).toBe('Der Account konnte nicht gelöscht werden.')
    expect(router.currentRoute.value.path).toBe('/profile')
    expect(useAuthStore().isAuth).toBe(true)
  })
})
