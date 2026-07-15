import { onMounted, onUnmounted, ref, type Ref } from 'vue'

export function useElementVisible(element: Ref<HTMLElement | null>) {
  const isVisible = ref(false);

  let observer: IntersectionObserver | null = null;

  onMounted(() => {
    observer = new IntersectionObserver(([entry]) => {
        isVisible.value = entry.isIntersecting
      },
      {
        threshold: 0.25,
      },
      )

    if (element.value) {
      observer.observe(element.value)
    }
  })

  onUnmounted(() => {
    observer?.disconnect()
  })

  return { isVisible }
}
