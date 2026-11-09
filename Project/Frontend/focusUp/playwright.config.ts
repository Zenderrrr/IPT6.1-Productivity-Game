import { defineConfig } from '@playwright/test'

const baseURL = 'http://127.0.0.1:4175'

export default defineConfig({
  testDir: './e2e',
  use: {
    baseURL,
    browserName: 'chromium',
    locale: 'de-CH',
    timezoneId: 'Europe/Zurich',
    launchOptions: {
      executablePath: process.env.PLAYWRIGHT_CHROMIUM_EXECUTABLE_PATH,
    },
  },
  webServer: {
    command: 'npm run dev -- --host 127.0.0.1 --port 4175 --strictPort',
    env: { VITE_API_URL: baseURL },
    url: baseURL,
  },
})
