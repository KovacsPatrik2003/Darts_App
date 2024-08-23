<template>
    <div id="app">
        <!-- <img alt="Vue logo" src="./assets/logo.png"> -->
        <!--<HelloWorld msg="Welcome to Your Vue.js App" />-->
        <VueLogIn />
    </div>
</template>

<script>
    /*import HelloWorld from './components/HelloWorld.vue'*/
    import router from './router'
    import VueLogIn from'./components/HelloWorld.vue'
    export default {
        name: 'App',
        components: {
            /*HelloWorld,*/
            VueLogIn
        },
        router
    }
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>
