/// <reference lib="dom" />
import { expect, test, type Locator, type Page } from '@playwright/test'

type HeatmapResponse = 'activity' | 'empty' | 'error'

const productivity = Array.from({ length: 92 }, (_, index) => ({
  date: new Date(Date.UTC(2026, 0, 3 + index)).toISOString(),
  xpGained: index === 91 ? 100 : 0,
  completedTasks: 0,
  timeSpent: 0,
}))
const stats = {
  totalXp: 0,
  tasksDone: 0,
  tasksOpen: 0,
  streakCount: 0,
  bestStreak: 0,
  totalTimeMin: 0,
}

async function openStatistics(page: Page, baseURL: string, heatmap: HeatmapResponse) {
  await page.clock.setFixedTime(new Date('2026-04-04T10:00:00Z'))
  await page.addInitScript(() => localStorage.setItem('token', 'test-session'))
  await page.route('**/*', async (route) => {
    const url = new URL(route.request().url())
    if (url.origin !== baseURL) {
      await route.abort()
      return
    }

    switch (url.pathname) {
      case '/api/Auth/me':
        await route.fulfill({
          json: { id: 1, username: 'HeatmapTest', email: 'test@example.test' },
        })
        break
      case '/api/dashboard':
        await route.fulfill({ json: { ...stats, lastCompletedTasks: [] } })
        break
      case '/api/stats':
        await route.fulfill({ json: stats })
        break
      case '/api/stats/productivity':
        if (url.searchParams.get('rangeInDays') !== '91' || heatmap === 'empty') {
          await route.fulfill({ json: [] })
        } else if (heatmap === 'error') {
          await route.fulfill({ status: 503, json: { message: 'Dienst nicht verfügbar' } })
        } else {
          await route.fulfill({ json: productivity })
        }
        break
      default:
        if (url.pathname.startsWith('/api/')) {
          throw new Error(`Unexpected API call ${url.pathname}`)
        }
        await route.continue()
    }
  })

  await page.goto('/stats', { waitUntil: 'domcontentloaded' })
  return page.getByRole('region', { name: 'Aktivitäts-Heatmap' })
}

async function findCellPixel(canvas: Locator, rgb: [number, number, number]) {
  return canvas.evaluate((element, [red, green, blue]) => {
    if (!(element instanceof HTMLCanvasElement)) return null
    const { width, height } = element
    const data = element.getContext('2d')?.getImageData(0, 0, width, height).data
    if (!data) return null

    // Use a pixel surrounded by the same fill so hovering hits the cell itself.
    for (let y = 2; y < height - 2; y++) {
      for (let x = 2; x < width - 2; x++) {
        const matches = (pixelX: number, pixelY: number) => {
          const index = (pixelY * width + pixelX) * 4
          return (
            data[index] === red &&
            data[index + 1] === green &&
            data[index + 2] === blue &&
            data[index + 3] === 255
          )
        }
        if (matches(x, y) && matches(x - 2, y - 2) && matches(x + 2, y + 2)) {
          return {
            x: Math.round((x * element.clientWidth) / width),
            y: Math.round((y * element.clientHeight) / height),
          }
        }
      }
    }
    return null
  }, rgb)
}

test('zeichnet die XP-Farben und zeigt Datum und XP beim Überfahren', async ({ page, baseURL }) => {
  const heatmap = await openStatistics(page, baseURL!, 'activity')
  const canvas = heatmap.locator('canvas')
  const maxColor: [number, number, number] = [15, 118, 110]
  await expect(heatmap).toBeVisible()
  await expect.poll(() => findCellPixel(canvas, [243, 244, 246])).not.toBeNull()
  await expect.poll(() => findCellPixel(canvas, maxColor)).not.toBeNull()

  const point = await findCellPixel(canvas, maxColor)
  await canvas.hover({ position: point! })
  await expect(page.getByText('04.04.2026 · 100 XP', { exact: true })).toBeVisible()
})

test.describe('kleiner Touchscreen', () => {
  test.use({ viewport: { width: 375, height: 800 }, hasTouch: true })

  test('erreicht bei 375 px Breite die letzte Spalte durch horizontales Scrollen', async ({
    page,
    baseURL,
  }) => {
    const heatmap = await openStatistics(page, baseURL!, 'activity')
    const canvas = heatmap.locator('canvas')
    await expect(canvas).toBeVisible()
    expect(await heatmap.evaluate((element) => element.scrollWidth)).toBeGreaterThan(
      await heatmap.evaluate((element) => element.clientWidth),
    )

    await heatmap.focus()
    await expect(heatmap).toBeFocused()
    await heatmap.press('ArrowRight')
    await expect.poll(() => heatmap.evaluate((element) => element.scrollLeft)).toBeGreaterThan(0)
    await heatmap.evaluate((element) => {
      element.scrollLeft = element.scrollWidth
    })
    const point = await findCellPixel(canvas, [15, 118, 110])
    expect(point).not.toBeNull()
    const bounds = await canvas.boundingBox()
    const region = await heatmap.boundingBox()
    expect(bounds!.x + point!.x).toBeGreaterThan(region!.x)
    expect(bounds!.x + point!.x).toBeLessThan(region!.x + region!.width)
    await canvas.tap({ position: point! })
    await expect(page.getByText('04.04.2026 · 100 XP', { exact: true })).toBeVisible()
  })
})

for (const { response, message } of [
  { response: 'empty', message: 'Keine Aktivitätsdaten vorhanden.' },
  { response: 'error', message: 'Die Heatmap konnte nicht geladen werden.' },
] as const) {
  test(`zeigt den Zustand ${response} bei der Heatmap`, async ({ page, baseURL }) => {
    const heatmap = await openStatistics(page, baseURL!, response)
    await expect(heatmap).toBeVisible()
    await expect(page.getByText(message, { exact: true })).toBeVisible()
  })
}
