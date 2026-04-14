<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import Tag from '@/components/ui/Tag.vue'
import Badges from '@/components/ui/Badges.vue'
import { computed, onMounted, ref } from 'vue'
import { useAuthStore } from '@/stores/authStore.ts'
import type { User } from '@/types/user.ts'

const authStore = useAuthStore()
const userInfo = ref<User | null>(null)
const error = ref<string | null>(null)

const nameInitials = computed(
  () => (userInfo.value?.username[0] ?? '') + (userInfo.value?.username[1] ?? ''),
)

onMounted(async () => {
  try {
    await authStore.me()
    userInfo.value = authStore.user
  } catch (e) {
    error.value = e ? e.message : 'Failed to fetch user'
  }
})
</script>

<template>
  <NavAuth :name-initials="nameInitials"></NavAuth>

  <main>
    <GreetingsSection
      title="Mein Profil"
      subtitle="Sieh deinen Fortschritt, deine Badges und verwalte deinen Account"
    ></GreetingsSection>

    <!-- Profile Overview-->
    <section
      class="relative base-element flex items-center justify-between gap-4 overflow-hidden border border-gray-200"
    >
      <div
        class="absolute top-0 left-0 w-full h-1 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"
      ></div>

      <div class="flex items-center justify-center gap-2">
        <div
          class="w-[150px] h-[150px] border-4 border-[var(--primary-color)] rounded-full flex items-center justify-center bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)]"
        >
          <span class="text-4xl text-[var(--text-color-white)] font-semibold">{{ nameInitials }}</span>
        </div>

        <div class="ml-3 flex flex-col items-start justify-start gap-1">
          <span class="text-2xl font-semibold">{{ userInfo?.username }}</span>
          <span class="text-[var(--text-color-light)] text-sm">{{ userInfo?.email }}</span>

          <div class="flex items-center justify-start gap-2 mt-3">
            <Tag name="Level 12" color-hex="#0F172A" text-color-hex="#ffffff"></Tag>
            <Tag name="18-Tage-Streak" color-hex="#0F172A" text-color-hex="#ffffff"></Tag>
            <Tag name="Macher" color-hex="#0F172A" text-color-hex="#ffffff"></Tag>
          </div>
        </div>
      </div>

      <button
        class="cursor-pointer flex items-center justify-center gap-2 text-sm px-4 py-2 bg-gray-100 rounded-xl border border-gray-200"
      >
        <i class="fa-solid fa-pen text-xs"></i>
        <span>Profilbild bearbeiten</span>
      </button>
    </section>

    <!-- Badges-->
    <section>
      <div class="flex items-center justify-center gap-3">
        <span class="font-semibold text-lg">Badges</span>
        <div class="h-0.5 w-full bg-gray-200"></div>
      </div>
      <div class="grid grid-cols-7 gap-5 mt-4">
        <Badges v-for="i in 10" :key="i" :checked="false" name="First Win" svg=""></Badges>
      </div>
    </section>

    <!-- Account Settings-->
    <section>
      <div class="flex items-center justify-center gap-3">
        <span class="font-semibold text-lg text-nowrap">Account Einstellungen</span>
        <div class="h-0.5 w-full bg-gray-200"></div>
      </div>

      <div class="base-element border-2 border-gray-200 mt-3">
        <div class="flex items-center justify-between gap-2">
          <div class="flex items-center justify-start gap-4">
            <div
              class="flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-[var(--primary-color-light)] text-[var(--primary-color)]"
            >
              <i class="fa-regular fa-circle-user"></i>
            </div>
            <div class="flex flex-col items-start justify-center gap-1">
              <span class="font-semibold text-md">Benutzername</span>
              <span class="text-[var(--text-color-light)] text-sm"
                >{{ userInfo?.username }} - Ändere deinen Benutzernamen</span
              >
            </div>
          </div>
          <div class="flex items-center justify-center border-2 border-gray-200 rounded-xl">
            <button class="px-4 py-2 text-sm font-semibold cursor-pointer">Ändern</button>
          </div>
        </div>

        <div class="w-full my-4 h-0.5 bg-gray-200"></div>

        <div class="flex items-center justify-between gap-2">
          <div class="flex items-center justify-start gap-4">
            <div
              class="flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-[var(--secondary-color-light)] text-[var(--secondary-color)]"
            >
              <i class="fa-solid fa-lock"></i>
            </div>
            <div class="flex flex-col items-start justify-center gap-1">
              <span class="font-semibold text-md">Passwort</span>
              <span class="text-[var(--text-color-light)] text-sm"
                >Zuletzt geändert vor 3 Monaten</span
              >
            </div>
          </div>
          <div class="flex items-center justify-center border-2 border-gray-200 rounded-xl">
            <button class="px-4 py-2 text-sm font-semibold cursor-pointer">Ändern</button>
          </div>
        </div>

        <div class="w-full my-4 h-0.5 bg-gray-200"></div>

        <div class="flex items-center justify-between gap-2">
          <div class="flex items-center justify-start gap-4">
            <div
              class="flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-violet-100 text-violet-500"
            >
              <i class="fa-solid fa-moon"></i>
            </div>
            <div class="flex flex-col items-start justify-center gap-1">
              <span class="font-semibold text-md">Darkmode</span>
              <span class="text-[var(--text-color-light)] text-sm"
                >Wechse zwischen hellem und dunklem Modus</span
              >
            </div>
          </div>
          <div class="flex items-center justify-center border-2 border-gray-200 rounded-xl">
            <button class="px-4 py-2 text-sm font-semibold cursor-pointer">Ändern</button>
          </div>
        </div>
      </div>

      <div class="base-element border-2 border-gray-200 mt-4">
        <div class="flex items-center justify-between gap-2">
          <div class="flex items-center justify-start gap-4">
            <div
              class="flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-red-100 text-red-500"
            >
              <i class="fa-solid fa-trash-can"></i>
            </div>
            <div class="flex flex-col items-start justify-center gap-1">
              <span class="font-bold text-md text-red-500">Account löschen</span>
              <span class="text-[var(--text-color-light)] text-sm"
                >Account wird dauerhaft gelöscht - kann nicht rückgängig gemacht werden!</span
              >
            </div>
          </div>
          <div
            class="flex items-center justify-center border-2 border-red-300 rounded-xl bg-red-100"
          >
            <button class="cursor-pointer px-4 py-2 text-sm font-semibold text-red-500">
              Löschen
            </button>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<style scoped>
main {
  max-width: 64rem;
}
</style>
