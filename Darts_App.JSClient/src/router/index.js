import Vue from 'vue'
import Router from 'vue-router'
import VueLogIn from '../components/VueLogIn.vue'

Vue.use(Router)

export default new Router({
    routes: [
        {
            path: '/',
            name: 'VueLogIn',
            component: VueLogIn
        }
    ]
})