import { afterEach, describe, expect, it, vi } from 'vitest'
import { formatDate, formatTime, GetTimeFromNow } from '@/utils/date'
import { applyUIMode, getUIMode, setUIMode, toggleUIMode } from '@/utils/modeUI'
import { slug } from '@/utils/slug'
import { getUserError } from '@/utils/userError'

afterEach(() => {
  vi.useRealTimers()
  localStorage.clear()
  document.documentElement.classList.remove('dark')
})

describe('Formatierung', () => {
  it.each([
    ['  Mein Badge  ', 'mein_badge'],
    ['FIRST', 'first'],
    ['', ''],
  ])('formatiert %j als %j', (input, expected) => expect(slug(input)).toBe(expected))

  it('füllt einstellige Tages-, Monats- und Uhrzeitwerte mit Nullen auf', () => {
    expect(formatDate(new Date(2026, 0, 5))).toBe('2026-01-05')
    expect(formatDate('2026-12-17T12:00:00')).toBe('2026-12-17')
    expect(formatTime(8)).toBe('08')
    expect(formatTime(42)).toBe('42')
  })

  it.each([
    [new Date(2026, 0, 2, 11, 30), 'Vor 30 min'],
    [new Date(2026, 0, 2, 11), 'Vor 1 h'],
    [new Date(2026, 0, 1, 11, 5), 'Gestern, 11:05 Uhr'],
    [new Date(2025, 11, 30, 9, 5), '2025, 30. Dezember - 09:05 Uhr'],
  ])('formatiert vergangene Zeit für %s', (date, expected) => {
    vi.useFakeTimers()
    vi.setSystemTime(new Date(2026, 0, 2, 12))
    expect(GetTimeFromNow(date)).toBe(expected)
  })

  it('erkennt den Kalendertag auch dann korrekt, wenn ein älteres Datum dieselbe Tagesnummer hat', () => {
    vi.useFakeTimers()
    vi.setSystemTime(new Date(2026, 2, 2, 12))
    expect(GetTimeFromNow(new Date(2026, 0, 1, 7, 5))).toBe('Am 1. Januar - 07:05 Uhr')
  })
})

describe('Fehlermeldungen für die Anmeldung', () => {
  it.each([
    ['ERROR: 401', 'E-Mail-Adresse oder Passwort ist falsch.'],
    ['ERROR: 403', 'E-Mail-Adresse oder Passwort ist falsch.'],
    ['ERROR: 409', 'Dieser Benutzername oder diese E-Mail-Adresse existiert bereits.'],
    ['ERROR: 429', 'Zu viele Anmeldeversuche versuche es später nochmals.'],
    ['ERROR: 500', 'Der Server hat ein Problem zurzeit versuche es später nochmals.'],
    ['Failed to fetch', 'Keine Verbindung zum Server möglich.'],
    ['Unerwarteter Fehler', 'Die Anmeldung ist fehlgeschlagen'],
  ])('übersetzt %s in eine lesbare Meldung', (message, expected) => {
    expect(getUserError(new Error(message))).toBe(expected)
  })

  it('liefert eine neutrale Meldung für unbekannte Fehlertypen', () => {
    expect(getUserError(null)).toBe('Ein unbekannter Fehler ist aufgetreten.')
  })
})

describe('Farbmodus', () => {
  it('verwendet ohne gespeicherte Einstellung den hellen Modus', () => {
    document.documentElement.classList.add('dark')
    applyUIMode()
    expect(getUIMode()).toBe(false)
    expect(document.documentElement.classList.contains('dark')).toBe(false)
  })

  it('setzt den gewählten Modus, stellt ihn wieder her und kann ihn umschalten', () => {
    setUIMode(true)
    expect(getUIMode()).toBe(true)
    document.documentElement.classList.remove('dark')
    applyUIMode()
    expect(document.documentElement.classList.contains('dark')).toBe(true)
    toggleUIMode()
    expect(localStorage.getItem('isDarkModeActive')).toBe('false')
    expect(document.documentElement.classList.contains('dark')).toBe(false)
  })
})
