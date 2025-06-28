import './assets/main.css'
import ToastService from 'primevue/toastservice'
import PrimeVue from 'primevue/config'
import { createApp } from 'vue'
import App from './App.vue'
import '/node_modules/primeflex/primeflex.css';
import "primeicons/primeicons.css";
import Aura from '@primeuix/themes/aura'

const app = createApp(App)
app.use(PrimeVue, {
    theme: {
        preset: Aura
    }
})
app.use(ToastService)
app.mount('#app')