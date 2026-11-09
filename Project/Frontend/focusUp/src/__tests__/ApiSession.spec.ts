import { describe, expect, it } from 'vitest'
import { apiFetch } from '@/api/api'
import { loginApi } from '@/api/auth.api'
import { useAuthStore } from '@/stores/authStore'
import { json, mockHttp, noContent, task, user } from './testSupport'

function seedSession() {
  localStorage.setItem('token', 'old-token')
  useAuthStore().$patch({ user })
}

describe('API-Client und Sitzung', () => {
  it('verwendet den Sitzungstoken und verarbeitet eine Antwort ohne Inhalt', async () => {
    seedSession()
    const requests = mockHttp({ 'DELETE /api/tasks/1': () => noContent() })
    await expect(apiFetch('/tasks/1', { method: 'DELETE' })).resolves.toBeUndefined()
    const [request] = requests('DELETE', '/api/tasks/1')
    expect(request?.headers.get('Authorization')).toBe('Bearer old-token')
    expect(request?.credentials).toBe('include')
  })

  it('erneuert den Token bei HTTP 401 und wiederholt die Anfrage mit dem neuen Token', async () => {
    seedSession()
    const requests = mockHttp({
      'GET /api/tasks': ({ headers }) =>
        headers.get('Authorization') === 'Bearer renewed' ? json([task()]) : json({}, 401),
      'POST /api/Auth/refresh': () => json({ newAccessToken: 'renewed' }),
    })
    await expect(apiFetch('/tasks')).resolves.toMatchObject([{ id: 1 }])
    expect(
      requests('GET', '/api/tasks').map(({ headers }) => headers.get('Authorization')),
    ).toEqual(['Bearer old-token', 'Bearer renewed'])
    expect(requests('POST', '/api/Auth/refresh')).toHaveLength(1)
    expect(localStorage.getItem('token')).toBe('renewed')
  })

  it('entfernt die Sitzung bei abgelehnter Token-Erneuerung', async () => {
    seedSession()
    const requests = mockHttp({
      'GET /api/tasks': () => json({}, 401),
      'POST /api/Auth/refresh': () => json({}, 401),
      'POST /api/Auth/logout': () => noContent(),
    })
    await expect(apiFetch('/tasks')).rejects.toThrow('Session abgelaufen.')
    expect(requests('GET', '/api/tasks')).toHaveLength(1)
    expect(requests('POST', '/api/Auth/logout')).toHaveLength(1)
    expect(localStorage.getItem('token')).toBeNull()
    expect(useAuthStore().isAuth).toBe(false)
    expect(useAuthStore().user).toBeNull()
  })

  it('versucht nach einem fehlgeschlagenen Login keine Token-Erneuerung', async () => {
    const requests = mockHttp({ 'POST /api/Auth/login': () => json({}, 401) })
    await expect(loginApi('NoraTest', 'falsch')).rejects.toThrow('ERROR: 401')
    expect(requests('POST', '/api/Auth/refresh')).toHaveLength(0)
  })

  it('leitet HTTP-Fehler inklusive Antworttext an den Aufrufer weiter', async () => {
    mockHttp({ 'GET /api/tasks': () => json({ message: 'Dienst nicht verfügbar' }, 503) })
    await expect(apiFetch('/tasks')).rejects.toThrow(
      'ERROR: 503: {"message":"Dienst nicht verfügbar"}',
    )
  })
})
