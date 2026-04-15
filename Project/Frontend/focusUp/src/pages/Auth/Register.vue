<script lang="ts" setup>
import Logo from '@/components/ui/Logo.vue'
import { ref } from 'vue'
import { useAuthStore } from '@/stores/authStore.ts'
import { useRouter } from 'vue-router'

const isPasswordVisible = ref<boolean>(false)
function showPassword() {
  return isPasswordVisible.value
}

const isPasswordVisibleConfirm = ref<boolean>(false)
function showPasswordConfirm() {
  return isPasswordVisibleConfirm.value
}

const username = ref<string>('')
const email = ref<string>('')
const password = ref<string>('')
const passwordConfirm = ref<string>('')
const error = ref<string | null>('')

const authStore = useAuthStore()
const router = useRouter()

async function register() {
  if(password.value !== passwordConfirm.value) {
    error.value = 'Beide Passwörter müssen identisch sein.'
    return
  }

  try{
    await authStore.register(username.value, email.value, password.value)

    error.value = authStore.error

    if(!authStore.loading && !authStore.error)
      await router.push('/dashboard')
  }catch(e){
    error.value = e ? e.message : 'Beim Registieren ist etwas schief gelaufen.'
  }
}
</script>

<template>
  <div class="w-full h-[100vh] bg-dots overflow-hidden">
    <div class="sm:w-[600px] w-full sm:h-[600px] bg-primary-gradient"></div>
    <div class="absolute bottom-0 right-0 sm:w-[600px] w-full sm:h-[600px] bg-secondary-gradient"></div>

    <!-- Login window-->
    <div class="flex items-center h-full justify-center sm:-translate-y-1/2">
      <div class="bg-[var(--surface-color)] base-element sm:w-[400px] w-full">
        <!-- Header -->
        <div class="flex items-center justify-between">
          <RouterLink
            to="/"
            class="color-change-primary-animation flex items-center justify-center gap-1 text-xs font-semibold text-[var(--text-color-light)]"
          >
            <div class="">
              <i class="fa-solid fa-arrow-left"></i>
            </div>
            <span>Zurück</span>
          </RouterLink>
          <Logo link="/"></Logo>
        </div>

        <!-- Visual-->
        <!-- Visual-->
        <div class="my-6 visual relative" aria-hidden="true">
          <svg viewBox="0 0 360 110" xmlns="http://www.w3.org/2000/svg" fill="none">
            <!-- Desk surface -->
            <rect x="20" y="85" width="320" height="6" rx="3" fill="#E2E8F0"/>

            <!-- Monitor -->
            <rect x="120" y="28" width="120" height="72" rx="8" fill="#1E293B"/>
            <rect x="126" y="34" width="108" height="56" rx="5" fill="#0F172A"/>
            <!-- Screen content -->
            <rect x="133" y="41" width="60" height="5" rx="2" fill="#14B8A6" opacity=".8"/>
            <rect x="133" y="50" width="45" height="3" rx="1.5" fill="#64748B" opacity=".6"/>
            <rect x="133" y="57" width="50" height="3" rx="1.5" fill="#64748B" opacity=".4"/>
            <rect x="133" y="64" width="35" height="3" rx="1.5" fill="#64748B" opacity=".4"/>
            <!-- Progress bar on screen -->
            <rect x="133" y="72" width="94" height="5" rx="2.5" fill="#1E293B"/>
            <rect x="133" y="72" width="58" height="5" rx="2.5" fill="#14B8A6" opacity=".9"/>
            <!-- Monitor stand -->
            <rect x="173" y="84" width="14" height="10" rx="2" fill="#CBD5E1"/>
            <rect x="163" y="90" width="34" height="4" rx="2" fill="#CBD5E1"/>

            <!-- Keyboard -->
            <rect x="130" y="94" width="100" height="14" rx="4" fill="#E2E8F0"/>
            <rect x="136" y="98" width="88" height="6" rx="2" fill="#CBD5E1" opacity=".7"/>

            <!-- Cup left -->
            <rect x="62" y="68" width="28" height="32" rx="6" fill="#F8FAFC" stroke="#E2E8F0" stroke-width="1.5"/>
            <path d="M90 76 Q100 80 90 84" stroke="#E2E8F0" stroke-width="1.5" stroke-linecap="round"/>
            <!-- Steam -->
            <path d="M72 64 Q74 59 72 54" stroke="#14B8A6" stroke-width="1.5" stroke-linecap="round" opacity=".5">
              <animate attributeName="opacity" values=".5;.15;.5" dur="2s" repeatCount="indefinite"/>
              <animate attributeName="d" values="M72 64 Q74 59 72 54;M72 64 Q70 59 72 54;M72 64 Q74 59 72 54" dur="2s" repeatCount="indefinite"/>
            </path>
            <path d="M78 62 Q80 57 78 52" stroke="#14B8A6" stroke-width="1.5" stroke-linecap="round" opacity=".4">
              <animate attributeName="opacity" values=".4;.1;.4" dur="2.4s" repeatCount="indefinite"/>
            </path>

            <!-- Plant right -->
            <rect x="270" y="72" width="20" height="18" rx="4" fill="#CBD5E1"/>
            <ellipse cx="280" cy="72" rx="10" ry="14" fill="#15803D" opacity=".55"/>
            <ellipse cx="274" cy="67" rx="7" ry="11" fill="#16A34A" opacity=".65"/>
            <ellipse cx="286" cy="65" rx="7" ry="11" fill="#22C55E" opacity=".5"/>
          </svg>

          <!-- Xp Visual-->
          <div class="levitation-animation absolute top-[10px] left-[30px] text-xs flex items-center justify-center gap-2 border border-[var(--primary-color)] bg-[var(--primary-color-light)] rounded-lg px-2 py-1 text-[var(--primary-color)]">
            <span>+50 XP</span>
          </div>

          <!-- Streak Visual-->
          <div class="levitation-animation absolute top-[10px] right-[30px] text-xs flex items-center justify-center gap-1 border border-red-400 bg-red-100 rounded-lg px-2 py-1 text-red-400">
            <span class="">12</span>
            <i class="fa-solid fa-fire"></i>
          </div>
        </div>

        <div>
          <!-- Title-->
          <h1 class="font-bold text-2xl">
            Konto <em class="text-[var(--primary-color)]">erstellen</em>
          </h1>
          <p class="text-sm text-[var(--text-color-light)]">
            Starte jetzt und verbessere deine Produktivität
          </p>

          <!-- Form (input elements)-->
          <form @submit.prevent @submit="register">

            <div class="flex flex-col items-start justify-center mt-5 gap-1">

              <div class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase">
                <i class="fa-regular fa-user"></i>
                <label
                  class="font-semibold"
                  for="username"
                >Benutzername</label>
              </div>
              <input
                class="input-hover-default bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-gray-200 outline-[var(--primary-color)]"
                id="username"
                type="text"
                placeholder="max@beispiel.ch"
                v-model="username"
                required
              />
            </div>

            <div class="flex flex-col items-start justify-center mt-5 gap-1">

              <div class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase">
                <i class="fa-regular fa-envelope"></i>
                <label
                  class="font-semibold"
                  for="email"
                >E-Mail</label>
              </div>
              <input
                class="input-hover-default bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-gray-200 outline-[var(--primary-color)]"
                id="email"
                type="email"
                placeholder="max@beispiel.ch"
                v-model="email"
                required
              />
            </div>

            <div class="relative flex flex-col items-start justify-center mt-5 gap-1">
              <div class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase">
                <i class="fa-solid fa-key"></i>
                <label
                  class="font-semibold"
                  for="password"
                >Passwort</label>
              </div>
              <input
                class="input-hover-default bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-gray-200 outline-[var(--primary-color)]"
                id="password"
                :type="showPassword() ? 'text' : 'password'"
                placeholder="••••••••"
                v-model="password"
                required
              />

              <button @click="isPasswordVisible = true" v-if="!showPassword()" class="hover:text-[var(--primary-color)] transition duration-200 absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer">
                <i class="fa-solid fa-eye"></i>
              </button>

              <button @click="isPasswordVisible = false" v-if="showPassword()" class="hover:text-[var(--primary-color)] transition duration-200 absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer">
                <i class="fa-solid fa-xmark"></i>
              </button>
            </div>


            <div class="relative flex flex-col items-start justify-center mt-5 gap-1">
              <div class="flex items-center justify-start gap-1 text-[var(--text-color-light)] text-xs uppercase">
                <i class="fa-solid fa-shield-halved"></i>
                <label
                  class="font-semibold"
                  for="passwordConfirm"
                >Passwort bestätigen</label>
              </div>
              <input
                class="input-hover-default bg-[var(--background-color)] w-full rounded-lg px-4 py-2 border border-gray-200 outline-[var(--primary-color)]"
                id="passwordConfirm"
                :type="showPasswordConfirm() ? 'text' : 'password'"
                placeholder="••••••••"
                v-model="passwordConfirm"
                required
              />

              <button @click="isPasswordVisibleConfirm = true" v-if="!showPasswordConfirm()" class="hover:text-[var(--primary-color)] transition duration-200 absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer">
                <i class="fa-solid fa-eye"></i>
              </button>

              <button @click="isPasswordVisibleConfirm = false" v-if="showPasswordConfirm()" class="hover:text-[var(--primary-color)] transition duration-200 absolute right-0 top-4.5 px-3 py-3 text-[var(--text-color-light)] cursor-pointer">
                <i class="fa-solid fa-xmark"></i>
              </button>
            </div>

            <button
              class="scale-animation-sm w-full mb-2 bg-[var(--primary-color)] text-[var(--text-color-white)] cursor-pointer font-semibold text-center py-3 rounded-2xl mt-5 shadow-lg"
              type="submit"
            >
              Registrieren
            </button>

            <span v-if="error" class="text-[var(--error-color)] text-sm mt-10">{{ error }}</span>
          </form>

          <div class="flex justify-center items-center gap-1 mb-4 mt-5">
            <div class="h-0.5 w-[100%] bg-gray-200 rounded-2xl"></div>
            <div class="text-[var(--text-color-light)] text-xs text-nowrap flex gap-1">
              <span>Bereits ein Konto?</span>
              <RouterLink to="/login" class="color-change-secondary-animation text-[var(--primary-color)] font-semibold">Anmelden</RouterLink>
            </div>
            <div class="h-0.5 w-[100%] bg-gray-200 rounded-2xl"></div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.bg-dots {
  background-image: radial-gradient(gray 0.1px, transparent 1px);
  background-size: 20px 20px;
  background-position: center;
}

.bg-primary-gradient {
  background: linear-gradient(
    to bottom right,
    rgba(20, 184, 166, 0.4),
    transparent 50%,
    transparent 100%
  );
}

.bg-secondary-gradient {
  background: linear-gradient(
    to top left,
    rgba(14, 165, 233, 0.4),
    transparent 50%,
    transparent 100%
  );
}
</style>
