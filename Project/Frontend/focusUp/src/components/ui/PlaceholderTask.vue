<script lang="ts" setup>
</script>

<template>
  <div
    class="unchecked shrink-0 overflow-hidden relative border-1 border-transparent cursor-pointer base-element w-full flex items-center justify-start gap-4"
  >
    <div class="h-full w-0.5 bg-transparent absolute top-0 left-0"></div>

    <label>
      <input type="checkbox" class="w-[25px] h-[25px] appearance-none placeholder" />
      <span class="rounded-full"></span>
    </label>

    <div class="flex items-center justify-between w-full">
      <div class="flex flex-col items-start justify-center gap-2">
        <div class="title placeholder w-11 h-5"></div>
        <div class="placeholder w-100 h-4"></div>
        <div class="mt-1 flex items-center justify-start gap-2">
          <!-- Tags-->

          <!-- Time-->
          <div
            class="placeholder h-5 w-14 flex gap-1 items-center justify-center rounded-lg px-2 py-1 text-xs font-semibold bg-gray-100 text-[var(--text-color-light)]">
          </div>

          <!-- Date-->
          <div
            class="placeholder h-5 w-22 flex gap-1 items-center justify-center rounded-lg px-2 py-1 text-xs font-semibold bg-gray-100 text-[var(--text-color-light)]"
          >
          </div>

          <!-- XP-->
          <div
            class="placeholder h-5 w-14 flex gap-1 items-center justify-center rounded-lg px-2 py-1 text-xs font-semibold bg-gray-100 text-[var(--text-color-light)]"
          >
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
