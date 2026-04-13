<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <footer class="site-footer">
    <div class="container footer-inner">
      <RouterLink to="/" class="logo">
        <span class="logo-focus">Focus</span><span class="logo-up">Up</span>
      </RouterLink>

      <div class="footer-links">
        <a href="#">Impressum</a>
        <a href="#">Datenschutz</a>
      </div>

      <div class="footer-copy">© 2026 FocusUp · Sanjivan & Egor</div>
    </div>
  </footer>
</template>

<style scoped>
.site-footer {
  width: 100%;
  background: var(--surface-color);
  border-top: 1px solid #e2e8f0;
}

.container {
  width: 100%;
  max-width: 1360px;
  margin: 0 auto;
  padding: 0 40px;
}

.footer-inner {
  min-height: 84px;
  display: grid;
  grid-template-columns: 1fr auto 1fr;
  align-items: center;
  gap: 20px;
}

.logo {
  justify-self: start;
  text-decoration: none;
  font-weight: 900;
  font-size: 20px;
  letter-spacing: -0.04em;
  color: var(--text-color);
}

.logo-focus {
  color: var(--text-color);
}

.logo-up {
  color: var(--primary-color);
}

.footer-links {
  display: flex;
  align-items: center;
  gap: 28px;
  justify-self: center;
}

.footer-links a {
  color: #94a3b8;
  text-decoration: none;
  font-size: 14px;
  font-weight: 600;
  transition: color 0.2s ease;
}

.footer-links a:hover {
  color: var(--text-color);
}

.footer-copy {
  justify-self: end;
  color: #94a3b8;
  font-size: 14px;
  font-weight: 500;
  text-align: right;
}

@media (max-width: 960px) {
  .container {
    padding: 0 24px;
  }

  .footer-inner {
    grid-template-columns: 1fr;
    justify-items: center;
    text-align: center;
    padding: 24px 0;
    min-height: auto;
  }

  .logo,
  .footer-links,
  .footer-copy {
    justify-self: center;
  }

  .footer-links {
    gap: 18px;
    flex-wrap: wrap;
  }

  .footer-copy {
    text-align: center;
  }
}
</style>
