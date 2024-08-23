import Vue from 'vue'
import App from './App.vue'
import router from './router'

Vue.config.productionTip = false

new Vue({
    router,  // Győződj meg róla, hogy itt van
    render: h => h(App)
}).$mount('#app')