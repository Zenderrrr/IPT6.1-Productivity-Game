<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <header class="site-header">
    <div class="container header-inner">
      <RouterLink to="/" class="logo">
        <span class="logo-focus">Focus</span><span class="logo-up">Up</span>
      </RouterLink>

      <nav class="nav">
        <a href="#features">Funktionen</a>
        <a href="#how-it-works">So funktioniert's</a>
        <a href="#benefits">Vorteile</a>
      </nav>

      <div class="header-actions">
        <RouterLink to="/login" class="login-link">Anmelden</RouterLink>
        <RouterLink to="/login" class="cta-button">
          Kostenlos starten
          <span class="cta-arrow">→</span>
        </RouterLink>
      </div>
    </div>
  </header>
</template>

<style scoped>
.site-header {
  position: sticky;
  top: 0;
  z-index: 100;
  width: 100%;
  background: rgba(255, 255, 255, 0.85);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid #e2e8f0;
}

.container {
  width: 100%;
  max-width: 1360px;
  margin: 0 auto;
  padding: 0 40px;
}

.header-inner {
  min-height: 72px;
  display: flex;
  align-items: center;
  justify-content: space-between;
}

/* Logo */
.logo {
  text-decoration: none;
  font-weight: 900;
  font-size: 22px;
  letter-spacing: -0.04em;
}

.logo-focus {
  color: var(--text-color);
}

.logo-up {
  color: var(--primary-color);
}

/* Nav */
.nav {
  display: flex;
  gap: 32px;
}

.nav a {
  color: #64748b;
  text-decoration: none;
  font-weight: 600;
  font-size: 15px;
}

.nav a:hover {
  color: var(--text-color);
}

/* Actions */
.header-actions {
  display: flex;
  align-items: center;
  gap: 18px;
}

.login-link {
  color: #64748b;
  text-decoration: none;
  font-weight: 700;
}

.login-link:hover {
  color: var(--text-color);
}

.cta-button {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 12px 18px;
  border-radius: 14px;
  text-decoration: none;
  font-size: 14px;
  font-weight: 800;
  color: white;

  background: linear-gradient(135deg, var(--primary-color), var(--secondary-color));

  box-shadow: 0 10px 24px rgba(14, 165, 233, 0.25);
  transition: all 0.2s ease;
}

.cta-button:hover {
  transform: translateY(-1px);
  filter: brightness(0.95);
}

.cta-arrow {
  font-size: 14px;
}

/* Responsive */
@media (max-width: 960px) {
  .container {
    padding: 0 24px;
  }

  .nav {
    display: none;
  }

  .login-link {
    display: none;
  }
}
</style>
