export function slug(text: string) {
  return text.trim().toLowerCase().split(' ').join('_')
}
