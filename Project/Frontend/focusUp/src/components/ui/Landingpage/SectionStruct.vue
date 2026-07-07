<script lang="ts" setup>
const props = defineProps<{
  kicker: string
  title: string
  subtitle: string
}>()
</script>

<template>
  <section class="px-8 py-30">
    <div class="max-w-[1400px] mx-auto">
      <div class="text-center">
        <span class="uppercase text-[var(--primary-color)] text-sm font-semibold">{{
            props.kicker
          }}</span>
        <h2 class="text-[40px] font-extrabold">{{ props.title }}</h2>
        <div class="w-full flex justify-center">
          <p class="text-[var(--text-color-light)] text-md max-w-[600px]">
            {{ props.subtitle }}
          </p>
        </div>
      </div>

      <slot></slot>
    </div>
  </section>
</template>
