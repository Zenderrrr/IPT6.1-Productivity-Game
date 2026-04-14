<script lang="ts" setup>
import Logo from '@/components/ui/Logo.vue'
import { computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/authStore.ts'
import { ref } from 'vue'
import type { User } from '@/types/user.ts'

const userInfo = ref<User | null>(null)
const nameInitials = computed(
  () => (userInfo.value?.username[0] ?? '') + (userInfo.value?.username[1] ?? ''),
)

const authStore = useAuthStore()
onMounted(async () => {
  await authStore.me()
  userInfo.value = authStore.user
})
</script>

<template>
  <nav
    class="flex bg-[var(--surface-color)] justify-between items-center px-4 border-b border-gray-200"
  >
    <div class="flex items-center justify-between py-5 px-5">
      <Logo link="/dashboard" class="mr-9"></Logo>
      <ul
        class="flex items-center justify-between gap-5 font-semibold text-sm text-[var(--text-color-light)]"
      >
        <RouterLink to="/dashboard" class="px-2.5 py-1.5 rounded-xl cursor-pointer"
          >Dashboard</RouterLink
        >
        <RouterLink to="/tasks" class="px-2 py-1.5 rounded-xl cursor-pointer">Tasks</RouterLink>
        <RouterLink to="/stats" class="px-2 py-1.5 rounded-xl cursor-pointer">Stats</RouterLink>
        <RouterLink to="/profile" class="px-2 py-1.5 rounded-xl cursor-pointer">Profil</RouterLink>
      </ul>
    </div>
    <div>
      <p
        class="text-sm bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--surface-color)] rounded-full w-[35px] h-[35px] flex justify-center items-center"
      >
        {{ nameInitials }}
      </p>
    </div>
  </nav>
</template>

<style scoped>
.router-link-active {
  background-color: var(--primary-color-light);
  color: var(--primary-color);
}
</style>
