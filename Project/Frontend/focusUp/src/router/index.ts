import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../pages/Dashboard.vue'
import Home from '../pages/Home.vue'
import Login from '../pages/Auth/Login.vue'
import Register from '../pages/Auth/Register.vue'
import Tasks from '@/pages/Tasks.vue'
import Stats from '@/pages/Stats.vue'
import Profile from '@/pages/Profile.vue'
import { useAuthStore } from '@/stores/authStore.ts'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: Home,
      name: 'Home',
      meta: { requiresAuth: false },
    },
    {
      path: '/login',
      component: Login,
      name: 'Login',
      meta: { requiresAuth: false },
    },
    {
      path: '/register',
      component: Register,
      name: 'Register',
      meta: { requiresAuth: false },
    },
    {
      path: '/dashboard',
      component: Dashboard,
      name: 'Dashboard',
      meta: { requiresAuth: true },
    },
    {
      path: '/tasks',
      component: Tasks,
      name: 'Tasks',
      meta: { requiresAuth: true },
    },
    {
      path: '/stats',
      component: Stats,
      name: 'Stats',
      meta: { requiresAuth: true },
    },
    {
      path: '/profile',
      component: Profile,
      name: 'Profile',
      meta: { requiresAuth: true },
    },
  ],
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore()

  if (!authStore.user && !authStore.token) {
    await authStore.me()
  }

  if (to.meta.requiresAuth && !authStore.isAuth) {
    return '/login'
  }
})

export default router
