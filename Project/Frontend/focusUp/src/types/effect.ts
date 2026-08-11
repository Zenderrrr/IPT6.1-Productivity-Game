import type { EffectType } from '@/types/effectType.ts'

export type Effect = {
  id: number,
  type?: EffectType,
  left: number,
  duration: number,
  class?: string,
  amount?: number,
  days?: number,
}
