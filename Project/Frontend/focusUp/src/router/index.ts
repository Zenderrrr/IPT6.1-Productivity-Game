import { createRouter, createWebHistory } from 'vue-router'
import Dashboard from '../pages/Dashboard.vue'
import Home from '../pages/Home.vue'
import Login from '../pages/Auth/Login.vue'
import Register from '../pages/Auth/Register.vue'
import Tasks from '@/pages/Tasks.vue'
import Stats from '@/pages/Stats.vue'
import Profile from '@/pages/Profile.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      component: Home,
      name: 'Home',
    },
    {
      path: '/login',
      component: Login,
      name: 'Login',
    },
    {
      path: '/register',
      component: Register,
      name: 'Register',
    },
    {
      path: '/dashboard',
      component: Dashboard,
      name: 'Dashboard',
    },
    {
      path: '/tasks',
      component: Tasks,
      name: 'Tasks',
    },
    {
      path: '/stats',
      component: Stats,
      name: 'Stats',
    },
    {
      path: '/profile',
      component: Profile,
      name: 'Profile',
    }
  ],
})

export default router
