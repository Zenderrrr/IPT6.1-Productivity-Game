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

    await badgeStore.allBadges()

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

const allVisible = ref<boolean>(false)
const badges = ref<Badge[] | null>(null)
const updatedBadges = computed(() => {
  if(allVisible.value){
    return badges.value
  }
  return badges.value?.slice(0, 11) ?? []
})

async function getBadges(){
  return await Promise.all(
    badgeStore.badgeData?.map(async (t) => ({
      ...t,
      img: await badgeStore.badgeImgById(t.name)
    })) ?? []
  )
}

onMounted(async () => {
  await badgeStore.allBadges()
  badges.value = await getBadges()
})
</script>

<template>
  <DeleteUser :is-shown="isDeleteUserShown" @cancel="isDeleteUserShown = false" @confirm="onDeleteUser()"></DeleteUser>
  <NavAuth></NavAuth>

  <main class="w-full max-w-5xl mx-auto px-4 sm:px-6 lg:px-8 py-4">
    <GreetingsSection
      title="Mein Profil"
      subtitle="Sieh deinen Fortschritt, deine Badges und verwalte deinen Account"
    ></GreetingsSection>

    <!-- Profile Overview-->
<section
  class="relative base-element flex flex-col sm:flex-row sm:items-center sm:justify-between gap-5 overflow-hidden border border-gray-200"
>
  <div
    class="absolute top-0 left-0 w-full h-1 bg-linear-to-r from-[var(--primary-color)] to-[var(--secondary-color)]"
  ></div>

  <div class="flex flex-col sm:flex-row items-center sm:justify-start gap-4 w-full min-w-0">
    <div
      class="w-[110px] h-[110px] sm:w-[130px] sm:h-[130px] lg:w-[150px] lg:h-[150px] shrink-0 border-4 border-[var(--primary-color)] rounded-full flex items-center justify-center bg-linear-to-br from-[var(--primary-color)] to-[var(--secondary-color)]"
    >
      <span class="text-3xl sm:text-4xl text-[var(--text-color-white)] font-semibold">{{
        nameInitials
      }}</span>
    </div>

    <div class="sm:ml-3 flex flex-col items-center sm:items-start justify-start gap-1 min-w-0 text-center sm:text-left">
      <span class="text-xl sm:text-2xl font-semibold break-words">{{ userInfo?.username }}</span>
      <span class="text-[var(--text-color-light)] text-sm break-all">{{ userInfo?.email }}</span>

      <div class="flex flex-wrap items-center justify-center sm:justify-start gap-2 mt-3">
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
      </div>
    </div>
  </div>

<!--      <button-->
<!--        class="hover:border-[var(&#45;&#45;primary-color)] hover:text-[var(&#45;&#45;primary-color)] hover:bg-[var(&#45;&#45;primary-color-light)] transition duration-200 cursor-pointer flex items-center justify-center gap-2 text-sm px-4 py-2 bg-gray-100 rounded-xl border border-gray-200"-->
<!--      >-->
<!--        <i class="fa-solid fa-pen text-xs"></i>-->
<!--        <span>Profilbild bearbeiten</span>-->
<!--      </button>-->
</section>

    <!-- Badges-->
    <section>
      <div class="flex items-center justify-center gap-3">
        <span class="font-semibold text-lg">Badges</span>
        <div class="h-0.5 w-full bg-gray-200"></div>
      </div>
      <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 auto-rows-[140px] gap-4 sm:gap-5 mt-4">
        <Badges v-for="badge in updatedBadges" :key="badge.id" :checked="badgeUnlockedInfo?.some(t => t.id === badge.id)" :name="badge.name" :svg="badge.img ?? '' " :color-hex="badge.secondaryColor" :svg-color-hex="badge.primaryColor"></Badges>

        <button @click="allVisible = !allVisible" class="badge text-center flex flex-col gap-2 justify-center items-center rounded-xl border-dashed border-2 border-[var(--primary-color)]">
          <div class="border border-[var(--primary-color)] flex justify-center items-center w-[45px] h-[45px] bg-[var(--primary-color-light)] text-[var(--primary-color)] rounded-lg">
            <i v-if="!allVisible" class="fa-solid fa-angle-down"></i>
            <i v-if="allVisible" class="fa-solid fa-angle-up"></i>
          </div>
          <span v-if="!allVisible" class="font-semibold text-sm text-[var(--primary-color)]">Alle anzeigen</span>
          <span v-if="allVisible" class="font-semibold text-sm text-[var(--primary-color)]">Weniger anzeigen</span>
        </button>
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

<!--        <div class="flex items-center justify-between gap-2">-->
<!--          <div class="flex items-center justify-start gap-4">-->
<!--            <div-->
<!--              class="flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-[var(&#45;&#45;secondary-color-light)] text-[var(&#45;&#45;secondary-color)]"-->
<!--            >-->
<!--              <i class="fa-solid fa-lock"></i>-->
<!--            </div>-->
<!--            <div class="flex flex-col items-start justify-center gap-1">-->
<!--              <span class="font-semibold text-md">Passwort</span>-->
<!--              <span class="text-[var(&#45;&#45;text-color-light)] text-sm"-->
<!--                >Wechse dein Passwort einfach & sicher</span-->
<!--              >-->
<!--            </div>-->
<!--          </div>-->
<!--          <div class="hover:border-[var(&#45;&#45;primary-color)] hover:text-[var(&#45;&#45;primary-color)] hover:bg-[var(&#45;&#45;primary-color-light)] transition duration-200 flex items-center justify-center border-2 border-gray-200 rounded-xl">-->
<!--            <button class="px-4 py-2 text-sm font-semibold cursor-pointer">Ändern</button>-->
<!--          </div>-->
<!--        </div>-->

<!--        <div class="w-full my-4 h-0.5 bg-gray-200"></div>-->

        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-start sm:items-center justify-start gap-4">
            <div
              class="shrink-0 flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-violet-100 text-violet-500"
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
          <div class="w-full sm:w-fit cursor-not-allowed transition duration-200 flex items-center justify-center border-2 border-gray-200 bg-gray-100 rounded-xl">
            <button class="w-full sm:w-auto cursor-not-allowed px-4 py-2 text-sm font-semibold">Ändern</button>
          </div>
        </div>

        <div class="w-full my-4 h-0.5 bg-gray-200"></div>

        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-start sm:items-center justify-start gap-4">
            <div
              class="shrink-0 flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-red-100 text-red-500"
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
          <div @click="logout" class="w-full sm:w-fit hover:bg-white transition duration-200 flex items-center justify-center border-2 border-red-300 bg-red-100 rounded-xl">
            <button class="w-full sm:w-auto px-4 py-2 text-sm font-semibold cursor-pointer text-red-500">Logout</button>
          </div>
        </div>
      </div>

      <div class="base-element border-2 border-gray-200 mt-4">
        <div class="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4">
          <div class="flex items-start sm:items-center justify-start gap-4">
            <div
              class="shrink-0 flex items-center justify-center w-[40px] h-[40px] rounded-lg bg-red-100 text-red-500"
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
            class="w-full sm:w-fit hover:bg-white transition duration-200 scale-animation-sm flex items-center justify-center border-2 border-red-300 rounded-xl bg-red-100"
          >
            <button class="w-full sm:w-auto cursor-pointer px-4 py-2 text-sm font-semibold text-red-500">
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