<script lang="ts" setup>
  const props = defineProps<{
    nameInitials: string,
  }>();
</script>

<template>
    <nav class="flex bg-[var(--surface-color)] justify-between items-center px-4 border-b border-gray-200">
      <div class="flex items-center justify-between py-5 px-5">
        <RouterLink class="font-bold mr-9" to="/">
          Focus<span class="text-[var(--primary-color)]">Up</span>
        </RouterLink>
        <ul class="flex items-center justify-between gap-5 font-semibold text-sm text-[var(--text-color-light)]">
          <RouterLink to="/dashboard" class="px-2.5 py-1.5 rounded-xl cursor-pointer">Dashboard</RouterLink>
          <RouterLink to="/tasks" class="px-2 py-1.5 rounded-xl cursor-pointer">Tasks</RouterLink>
          <RouterLink to="/stats" class="px-2 py-1.5 rounded-xl cursor-pointer">Stats</RouterLink>
          <RouterLink to="/profile" class="px-2 py-1.5 rounded-xl cursor-pointer">Profil</RouterLink>
        </ul>
      </div>
      <div>
        <p class="text-sm bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--surface-color)] rounded-full w-[35px] h-[35px] flex justify-center items-center">{{ props.nameInitials }}</p>
      </div>
    </nav>
</template>

<style scoped>
  .router-link-active {
    background-color: var(--primary-color-light);
    color: var(--primary-color);
  }
</style>
