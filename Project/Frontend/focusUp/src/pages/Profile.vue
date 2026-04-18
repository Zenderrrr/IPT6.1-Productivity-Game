<script lang="ts" setup>
import NavAuth from '@/components/layout/NavAuth.vue'
import GreetingsSection from '@/components/ui/GreetingsSection.vue'
import Tag from '@/components/ui/Tag.vue'
import Badges from '@/components/ui/Badges.vue'
import { computed, onMounted, ref } from 'vue'
import { useAuthStore } from '@/stores/authStore.ts'
import type { User } from '@/types/user.ts'
import { useStatsStore } from '@/stores/statsStore.ts'
import type { Dashboard } from '@/types/dashboard.ts'
import { useBadgeStore } from '@/stores/badgeStore.ts'
import type { Badge } from '@/types/badge.ts'
import { useRouter } from 'vue-router'
import DeleteTask from '@/components/ui/DeleteTask.vue'
import DeleteUser from '@/components/ui/DeleteUser.vue'

const authStore = useAuthStore()
const statsStore = useStatsStore()
const badgeStore = useBadgeStore()

const userInfo = ref<User | null>(null)
const dashboardInfo = ref<Dashboard | null>(null)
const badgeInfo = ref<Badge[] | null>(null)
const badgeUnlockedInfo = ref<Badge[] | null>(null)
const error = ref<string | null>(null)

const nameInitials = computed(
  () => (userInfo.value?.username[0] ?? '') + (userInfo.value?.username[1] ?? ''),
)

onMounted(async () => {
  try {
    await authStore.me()
    userInfo.value = authStore.user

    await statsStore.dashboard('0')
    dashboardInfo.value = statsStore.dashboardData

    await badgeStore.allBadge()
    badgeInfo.value = badgeStore.badgeData

    await badgeStore.badgeUnlocked()
    badgeUnlockedInfo.value = badgeStore.badgeUnlockedData

  } catch (e) {
    error.value = e ? e.message : 'Failed to fetch user'
  }
})

const router = useRouter()
async function logout() {
  await authStore.logout()
  await router.push('/login')
}

async function onDeleteUser(){
  try{
    await authStore.deleteUser()
    await router.push('/login')
  }catch{}
}

const isDeleteUserShown = ref<boolean>(false)
</script>

<template>
  <DeleteUser :is-shown="isDeleteUserShown" @cancel="isDeleteUserShown = false" @confirm="onDeleteUser()"></DeleteUser>
  <NavAuth></NavAuth>

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
          <span class="text-4xl text-[var(--text-color-white)] font-semibold">{{
            nameInitials
          }}</span>
        </div>

        <div class="ml-3 flex flex-col items-start justify-start gap-1">
          <span class="text-2xl font-semibold">{{ userInfo?.username }}</span>
          <span class="text-[var(--text-color-light)] text-sm">{{ userInfo?.email }}</span>

          <div class="flex items-center justify-start gap-2 mt-3">
            <Tag
              :name="`Level ${dashboardInfo?.level}`"
              color-hex="#ebf8f7"
              text-color-hex="#0F172A"
            ></Tag>
            <Tag
              v-if="(dashboardInfo?.streakCount ?? 0) > 0"
              :name="`${dashboardInfo?.streakCount}-Tage-Streak`"
              color-hex="#ebf8f7"
              text-color-hex="#0F172A"
            ></Tag>
            <Tag name="Macher" color-hex="#ebf8f7" text-color-hex="#0F172A"></Tag>
          </div>
        </div>
      </div>

      <button
        class="hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] hover:bg-[var(--primary-color-light)] transition duration-200 cursor-pointer flex items-center justify-center gap-2 text-sm px-4 py-2 bg-gray-100 rounded-xl border border-gray-200"
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
        <Badges v-for="badge in badgeInfo" :key="badge.id" :checked="badgeUnlockedInfo?.some(t => t.id === badge.id)" :name="badge.name" svg="" color-hex="" svg-color-hex=""></Badges>
      </div>
    </section>

    <!-- Account Settings-->
    <section>
      <div class="flex items-center justify-center gap-3">
        <span class="font-semibold text-lg text-nowrap">Account Einstellungen</span>
        <div class="h-0.5 w-full bg-gray-200"></div>
      </div>

      <div class="base-element border-2 border-gray-200 mt-3">
<!--        <div class="flex items-center justify-between gap-2">-->
<!--          <div class="flex items-center justify-start gap-4">-->
<!--            <div-->
<!--              class="flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-[var(&#45;&#45;primary-color-light)] text-[var(&#45;&#45;primary-color)]"-->
<!--            >-->
<!--              <i class="fa-regular fa-circle-user"></i>-->
<!--            </div>-->
<!--            <div class="flex flex-col items-start justify-center gap-1">-->
<!--              <span class="font-semibold text-md">Benutzername</span>-->
<!--              <span class="text-[var(&#45;&#45;text-color-light)] text-sm"-->
<!--                >{{ userInfo?.username }} - Ändere deinen Benutzernamen</span-->
<!--              >-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="hover:border-[var(&#45;&#45;primary-color)] hover:text-[var(&#45;&#45;primary-color)] hover:bg-[var(&#45;&#45;primary-color-light)] transition duration-200 flex items-center justify-center border-2 border-gray-200 rounded-xl">-->
<!--            <button class="px-4 py-2 text-sm font-semibold cursor-pointer">Ändern</button>-->
<!--          </div>-->
<!--        </div>-->

<!--        <div class="w-full my-4 h-0.5 bg-gray-200"></div>-->

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
          <div class="hover:border-[var(--primary-color)] hover:text-[var(--primary-color)] hover:bg-[var(--primary-color-light)] transition duration-200 flex items-center justify-center border-2 border-gray-200 rounded-xl">
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
              <span class="font-semibold text-md">Darkmode - Kommt bald</span>
              <span class="text-[var(--text-color-light)] text-sm"
                >Wechse zwischen hellem und dunklem Modus</span
              >
            </div>
          </div>
          <div class="cursor-not-allowed transition duration-200 flex items-center justify-center border-2 border-gray-200 bg-gray-100 rounded-xl">
            <button class="cursor-not-allowed px-4 py-2 text-sm font-semibold">Ändern</button>
          </div>
        </div>

        <div class="w-full my-4 h-0.5 bg-gray-200"></div>

        <div class="flex items-center justify-between gap-2">
          <div class="flex items-center justify-start gap-4">
            <div
              class="flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-red-100 text-red-500"
            >
              <i class="fa-solid fa-arrow-right-from-bracket"></i>
            </div>
            <div class="flex flex-col items-start justify-center gap-1">
              <span class="font-semibold text-md text-md text-red-500">Konto verlassen</span>
              <span class="text-[var(--text-color-light)] text-sm"
              >Melde dich von diesem Konto ab.</span
              >
            </div>
          </div>
          <div @click="logout" class="hover:bg-white transition duration-200 flex items-center justify-center border-2 border-red-300 bg-red-100 rounded-xl">
            <button class="px-4 py-2 text-sm font-semibold cursor-pointer text-red-500">Logout</button>
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
            @click="isDeleteUserShown = true"
            class="hover:bg-white transition duration-200 scale-animation-sm flex items-center justify-center border-2 border-red-300 rounded-xl bg-red-100"
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
