<template>
    <div class="login parent">
        <div class="centered-div">
            <h1>Log In</h1>
            <label>Enter your User Name:</label>
            <input type="text" v-model="userName" />
            <label>Enter your Password:</label>
            <input type="password" v-model="password" />
            <button @click="logIn">Log In</button>
            <h3>Don't have an account yet? <a href="SignUp.html">Sign Up</a></h3>
        </div>
    </div>
</template>

<script>export default {
  data() {
    return {
      userName: '',
      password: '',
      player: []
    };
  },
  methods: {
    async logIn() {
      try {
        const response = await fetch(`http://localhost:61231/api/Player/${this.userName}/${this.password}`);
        if (!response.ok) {
          const errorData = await response.json();
          throw new Error(errorData.message);
        }
        this.player = await response.json();
        console.log(this.player);
        localStorage.setItem('username', this.userName);
        console.log('Username stored in localStorage:', localStorage.getItem('username'));
        this.$router.push('/GameSession');
      } catch (error) {
        console.error('Login failed:', error.message);
        alert('Login failed');
      }
    }
  }
};</script>

<style scoped>
    .login {
        /* Itt hozzáadhatod a LoginStyle.css stílusait */
    }

    .parent {
        /* Például ezek a stílusok kerülhetnek ide */
        display: flex;
        justify-content: center;
        align-items: center;
        height: 100vh;
    }

    .centered-div {
        text-align: center;
    }
</style>