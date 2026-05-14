<script lang="ts" setup>
</script>

<template>
  <div
    class="flex gap-2 justify-between items-center bg-[var(--background-color)] px-4 py-3 rounded-2xl"
  >
    <div class="flex gap-4 items-center">
      <div
        class="animate-pulse w-[22px] h-[22px] flex justify-center items-center text-center bg-gray-200 text-[var(--text-color-white)] text-xs font-light rounded-2xl"
      >
        <i class="fa-solid fa-check"></i>
      </div>
      <div class="flex flex-col gap-4 font-light">
        <div class="h-3 w-11 placeholder"></div>
        <div class="placeholder h-3 w-15"></div>
      </div>
    </div>
    <div
      class="placeholder w-15 h-6 rounded-2xl! px-2 py-1"
    >
    </div>
  </div>
</template>
