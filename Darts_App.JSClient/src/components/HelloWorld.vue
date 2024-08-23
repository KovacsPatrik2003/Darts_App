<template>
    <div class="hello">
        <h1>{{ msg }}</h1>
    </div>
</template>

<script>export default {
  name: 'HelloWorld',
  props: {
    msg: String
  }
}</script>

<style scoped>
    .hello {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        text-align: center;
        color: #2c3e50;
        margin-top: 60px;
    }
</style>