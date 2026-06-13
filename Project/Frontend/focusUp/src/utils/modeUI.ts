export function toggleUIMode() {
  const newMode = !getUIMode()

  localStorage.setItem('isDarkModeActive', String(newMode))
  document.documentElement.classList.toggle('dark', newMode)
}

export function applyUIMode() {
  const isActive = getUIMode()
  document.documentElement.classList.toggle('dark', isActive)
}

export function setUIMode(isActive: boolean) {
  localStorage.setItem('isDarkModeActive', String(isActive))
  document.documentElement.classList.toggle('dark', isActive)
}

export function getUIMode() {
  return localStorage.getItem('isDarkModeActive') === 'true'
}
