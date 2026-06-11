<script lang="ts" setup>
import Logo from '@/components/ui/Logo.vue'
import { computed, onMounted, ref } from 'vue'
import { useAuthStore } from '@/stores/authStore.ts'
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

const isMenuOpen = ref(false)
</script>

<template>
  <nav
    class="sticky top-0 z-999 bg-[var(--surface-color)] border-b border-gray-200"
  >
    <!-- Mobile -->
    <div class="md:hidden relative flex items-center justify-between px-4 py-4">
      <!-- Burger -->
      <button
        @click="isMenuOpen = !isMenuOpen"
        class="w-10 h-10 flex items-center justify-center rounded-lg hover:bg-gray-100 transition"
      >
        <i
          :class="
            isMenuOpen
              ? 'fa-solid fa-xmark text-lg'
              : 'fa-solid fa-bars text-lg'
          "
        ></i>
      </button>

      <!-- Logo centered -->
      <div class="absolute left-1/2 -translate-x-1/2">
        <Logo link="/dashboard" />
      </div>

      <!-- Avatar -->
      <p
        class="text-sm bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--surface-color)] rounded-full w-[35px] h-[35px] flex justify-center items-center"
      >
        {{ nameInitials }}
      </p>
    </div>

    <!-- Mobile Menu -->
    <Transition name="mobile-menu">
      <div
        v-if="isMenuOpen"
        class="md:hidden border-t border-gray-200 bg-[var(--surface-color)]"
      >
        <div class="flex flex-col p-3 gap-2">
          <RouterLink
            @click="isMenuOpen = false"
            to="/dashboard"
            class="mobile-link"
          >
            Dashboard
          </RouterLink>

          <RouterLink
            @click="isMenuOpen = false"
            to="/tasks"
            class="mobile-link"
          >
            Tasks
          </RouterLink>

          <RouterLink
            @click="isMenuOpen = false"
            to="/stats"
            class="mobile-link"
          >
            Stats
          </RouterLink>

          <RouterLink
            @click="isMenuOpen = false"
            to="/profile"
            class="mobile-link"
          >
            Profil
          </RouterLink>
        </div>
      </div>
    </Transition>

    <!-- Desktop -->
    <div
      class="hidden md:flex justify-between items-center px-4 border-b border-gray-200"
    >
      <div class="flex items-center justify-between py-5 px-5">
        <Logo link="/dashboard" class="mr-9" />

        <ul
          class="flex items-center justify-between gap-5 font-semibold text-sm text-[var(--text-color-light)]"
        >
          <RouterLink
            to="/dashboard"
            class="hover:bg-gray-100 transition duration-200 px-2.5 py-1.5 rounded-xl cursor-pointer"
          >
            Dashboard
          </RouterLink>

          <RouterLink
            to="/tasks"
            class="hover:bg-gray-100 transition duration-200 px-2 py-1.5 rounded-xl cursor-pointer"
          >
            Tasks
          </RouterLink>

          <RouterLink
            to="/stats"
            class="hover:bg-gray-100 transition duration-200 px-2 py-1.5 rounded-xl cursor-pointer"
          >
            Stats
          </RouterLink>

          <RouterLink
            to="/profile"
            class="hover:bg-gray-100 transition duration-200 px-2 py-1.5 rounded-xl cursor-pointer"
          >
            Profil
          </RouterLink>
        </ul>
      </div>

      <div>
        <p
          class="text-sm bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)] text-[var(--surface-color)] rounded-full w-[35px] h-[35px] flex justify-center items-center"
        >
          {{ nameInitials }}
        </p>
      </div>
    </div>
  </nav>
</template>

<style scoped>
.router-link-active {
  background-color: var(--primary-color-light);
  color: var(--primary-color);
}

.mobile-link {
  padding: 0.75rem 1rem;
  border-radius: 0.75rem;
  font-weight: 600;
  color: var(--text-color-light);
  transition: 0.2s;
}

.mobile-link:hover {
  background: rgb(243 244 246);
}

.mobile-menu-enter-active,
.mobile-menu-leave-active {
  transition: all 0.2s ease;
}

.mobile-menu-enter-from,
.mobile-menu-leave-to {
  opacity: 0;
  transform: translateY(-10px);
}
</style>