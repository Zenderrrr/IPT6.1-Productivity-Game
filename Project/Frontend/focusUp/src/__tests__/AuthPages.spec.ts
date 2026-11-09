import { describe, expect, it } from 'vitest'
import type { VueWrapper } from '@vue/test-utils'
import router from '@/router'
import { useAuthStore } from '@/stores/authStore'
import { dashboardRoutes, json, mockHttp, noContent, openPage, submit } from './testSupport'

const password = 'TestPasswort42!'
const email = 'nora@example.test'

async function fillRegistration(page: VueWrapper, confirmation = password) {
  await page.get('input#username').setValue('NoraTest')
  await page.get('input#email').setValue(email)
  await page.get('input#password').setValue(password)
  await page.get('input#passwordConfirm').setValue(confirmation)
}

async function fillReset(page: VueWrapper, confirmation = password) {
  const [newPassword, repeatPassword] = page.findAll('form input')
  await newPassword!.setValue(password)
  await repeatPassword!.setValue(confirmation)
}

describe('Anmeldung', () => {
  it('sendet die Eingaben, speichert die Sitzung und öffnet das Dashboard', async () => {
    const requests = mockHttp({
      ...dashboardRoutes,
      'POST /api/Auth/login': () => json({ token: 'new-token' }),
    })
    const page = await openPage('/login')
    await page.get('#email').setValue(email)
    await page.get('#password').setValue(password)
    await submit(page)

    const [request] = requests('POST', '/api/Auth/login')
    expect(request?.body).toEqual({ UsernameOrEmail: email, Password: password })
    expect(request?.credentials).toBe('include')
    expect(localStorage.getItem('token')).toBe('new-token')
    expect(useAuthStore().isAuth).toBe(true)
    expect(router.currentRoute.value.path).toBe('/dashboard')
  })

  it('zeigt bei falschen Zugangsdaten die Fehlermeldung und bleibt beim Login', async () => {
    mockHttp({ 'POST /api/Auth/login': () => json({}, 401) })
    const page = await openPage('/login')
    await page.get('#email').setValue(email)
    await page.get('#password').setValue('falsches Passwort')
    await submit(page)

    expect(page.text()).toContain('E-Mail-Adresse oder Passwort ist falsch.')
    expect(localStorage.getItem('token')).toBeNull()
    expect(router.currentRoute.value.path).toBe('/login')
  })

  it('schaltet die Sichtbarkeit des Passworts ohne Anfrage um', async () => {
    const page = await openPage('/login')
    expect(page.get('#password').attributes('type')).toBe('password')
    await page.get('form button[type="button"]').trigger('click')
    expect(page.get('#password').attributes('type')).toBe('text')
    await page.get('form button[type="button"]').trigger('click')
    expect(page.get('#password').attributes('type')).toBe('password')
  })
})

describe('Registrierung', () => {
  it('verhindert die Registrierung bei abweichender Bestätigung', async () => {
    const page = await openPage('/register')
    await fillRegistration(page, 'AnderesPasswort')
    await submit(page)

    expect(page.text()).toContain('Beide Passwörter müssen identisch sein.')
    expect(router.currentRoute.value.path).toBe('/register')
  })

  it('registriert den Account und meldet ihn anschliessend an', async () => {
    const requests = mockHttp({
      ...dashboardRoutes,
      'POST /api/Auth/register': () => json(7, 201),
      'POST /api/Auth/login': () => json({ token: 'registered-token' }),
    })
    const page = await openPage('/register')
    await fillRegistration(page)
    await submit(page)

    expect(requests('POST', '/api/Auth/register')[0]?.body).toEqual({
      Username: 'NoraTest',
      Email: email,
      Password: password,
    })
    expect(requests('POST', '/api/Auth/login')[0]?.body).toEqual({
      UsernameOrEmail: 'NoraTest',
      Password: password,
    })
    expect(localStorage.getItem('token')).toBe('registered-token')
    expect(router.currentRoute.value.path).toBe('/dashboard')
  })

  it('zeigt einen Konflikt an und führt keinen automatischen Login aus', async () => {
    mockHttp({ 'POST /api/Auth/register': () => json({}, 409) })
    const page = await openPage('/register')
    await fillRegistration(page)
    await submit(page)

    expect(page.text()).toContain(
      'Dieser Benutzername oder diese E-Mail-Adresse existiert bereits.',
    )
    expect(router.currentRoute.value.path).toBe('/register')
  })
})

describe('Passwort vergessen', () => {
  it('sendet die E-Mail und bestätigt die Anfrage', async () => {
    const requests = mockHttp({ 'POST /api/Auth/forgot-password': () => noContent() })
    const page = await openPage('/forgot-password')
    await page.get('#email').setValue(email)
    await submit(page)

    expect(requests('POST', '/api/Auth/forgot-password').map((request) => request.body)).toEqual([
      { email },
    ])
    expect(page.text()).toContain('Anfrage erfolgreich gesendet.')
  })

  it('entfernt eine frühere Erfolgsmeldung, wenn die nächste Anfrage fehlschlägt', async () => {
    let attempt = 0
    mockHttp({
      'POST /api/Auth/forgot-password': () =>
        ++attempt === 1 ? noContent() : json({ message: 'Bitte versuche es später erneut.' }, 503),
    })
    const page = await openPage('/forgot-password')
    await page.get('#email').setValue(email)
    await submit(page)
    expect(page.text()).toContain('Anfrage erfolgreich gesendet.')

    await submit(page)
    expect(page.text()).toContain('Bitte versuche es später erneut.')
    expect(page.text()).not.toContain('Anfrage erfolgreich gesendet.')
  })

  it('zeigt die Meldung bei einer vorübergehend nicht verfügbaren API', async () => {
    mockHttp({
      'POST /api/Auth/forgot-password': () =>
        json({ message: 'Bitte versuche es später erneut.' }, 503),
    })
    const page = await openPage('/forgot-password')
    await page.get('#email').setValue(email)
    await submit(page)

    expect(page.text()).toContain('Bitte versuche es später erneut.')
    expect(page.text()).not.toContain('Anfrage erfolgreich gesendet.')
  })
})

describe('Passwort zurücksetzen', () => {
  it('sendet den URL-Token und das neue Passwort als JSON', async () => {
    const requests = mockHttp({ 'POST /api/Auth/reset-password': () => noContent() })
    const page = await openPage('/reset-password?token=token%2B123')
    await fillReset(page)
    await submit(page)

    expect(requests('POST', '/api/Auth/reset-password').map((request) => request.body)).toEqual([
      { token: 'token+123', newPassword: password },
    ])
    expect(page.text()).toContain('Das Passwort konnte erfolgreich geändert werden.')
    expect(page.find('a[href="/login"]').exists()).toBe(true)
  })

  it('sendet bei abweichender Bestätigung keine Anfrage', async () => {
    const page = await openPage('/reset-password?token=token123')
    await fillReset(page, 'AnderesPasswort')
    await submit(page)
    expect(page.text()).toContain('Die beiden Passwörter stimmen nicht überein.')
  })

  it.each(['/reset-password', '/reset-password?token=a&token=b'])(
    'verhindert das Absenden ohne eindeutigen Token bei %s',
    async (path) => {
      const page = await openPage(path)
      await fillReset(page)
      await submit(page)
      expect(page.text()).toContain('Der Link ist ungültig oder abgelaufen.')
    },
  )

  it('zeigt bei abgelaufenem Link nur die lesbare Meldung aus der API', async () => {
    mockHttp({
      'POST /api/Auth/reset-password': () =>
        json({ message: 'Der Link ist ungültig oder abgelaufen.' }, 400),
    })
    const page = await openPage('/reset-password?token=expired')
    await fillReset(page)
    await submit(page)
    expect(page.text()).toContain('Der Link ist ungültig oder abgelaufen.')
    expect(page.text()).not.toContain('ERROR: 400')
    expect(page.text()).not.toContain('Das Passwort konnte erfolgreich geändert werden.')
  })
})
