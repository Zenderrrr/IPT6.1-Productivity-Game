import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'

import './styles/base.css'
import './styles/variables.css'
import './styles/components.css'

import { library } from '@fortawesome/fontawesome-svg-core'
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'
import {
  faClipboardList,
  faStar,
  faFire,
  faBolt,
  faLightbulb,
  faBullseye,
  faArrowTrendUp,
  faPlay,
  faPlus,
  faTrophy,
} from '@fortawesome/free-solid-svg-icons'

library.add(
  faClipboardList,
  faStar,
  faFire,
  faBolt,
  faLightbulb,
  faBullseye,
  faArrowTrendUp,
  faPlay,
  faPlus,
  faTrophy,
)

const app = createApp(App)

app.use(createPinia())
app.use(router)

app.component('font-awesome-icon', FontAwesomeIcon)

app.mount('#app')
