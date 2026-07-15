import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../pages/Dashboard.vue'
import Home from '../pages/Home.vue'
import Login from '../pages/Auth/Login.vue'
import Register from '../pages/Auth/Register.vue'
import Tasks from '@/pages/Tasks.vue'
import Stats from '@/pages/Stats.vue'
import Profile from '@/pages/Profile.vue'
import { useAuthStore } from '@/stores/authStore.ts'
import AllCompletedTasks from '@/pages/AllCompletedTasks.vue'
import Landingpage from '@/pages/Landingpage.vue'
import NotFound from '@/pages/NotFound.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: Landingpage,
      name: 'Landingpage',
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
    {
      path: '/allCompletedTasks',
      component: AllCompletedTasks,
      name: 'AllCompletedTasks',
      meta: { requiresAuth: true },
    },
    {
      path: '/:pathMatch(.*)*',
      component: NotFound,
      name: 'NotFound',
      meta: { requiresAuth: false },
    }
  ],
})

router.beforeEach(async (to) => {
  const authStore = useAuthStore()

  if (!authStore.user && !authStore.token) {
    try{
        await authStore.me()
      }catch(err){
        return '/login'
      }
  }

  if (to.meta.requiresAuth && !authStore.isAuth) {
    return '/login'
  }
})

export default router
