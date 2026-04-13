<script setup lang="ts">
import Header from './components/layout/Header.vue'
import Footer from './components/layout/Footer.vue'
import Home from './pages/Home.vue'

import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<template>
  <Header v-if="!route.meta.hideLayout" />
  <Home />
  <Footer v-if="!route.meta.hideLayout" />
</template>

<style scoped></style>
