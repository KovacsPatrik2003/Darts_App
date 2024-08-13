<template>
    <div class="maindiv" style="border: 1px solid black; width: 30%; float: left;">
        <div class="getGamesDiv">
            <h2>Get old games Results:</h2>
            <button @click="getData">Old Games</button>
        </div>
        <div id="oldGameResults">
            <p>Actual player: {{ userName }}</p>
            <table>
                <tr v-for="conn in ownGames" :key="conn.id">
                    <td>ConnectionId: {{ conn.id }}</td>
                    <td>PlayerId: {{ conn.playerId }}</td>
                    <td>GameId: {{ conn.gameId }}</td>
                    <td>Winner: {{ getWinnerName(conn.game.winnerId) }}</td>
                </tr>
            </table>
        </div>
    </div>
    <div id="game-settings" style="border: 1px solid black; width: 20%; float: left;">
        <h2>Game Settings</h2>
        <div>
            <label for="checkout-method">Checkout Method:</label>
            <select v-model="checkoutMethod" id="checkout-method" name="checkout-method">
                <option value="double">Double Out</option>
                <option value="master">Master Out</option>
                <option value="straight">Straight Out</option>
            </select>
        </div>
        <div>
            <label for="start-point">Start Point:</label>
            <select v-model="startPoint" id="start-point" name="start-point">
                <option value="101">101</option>
                <option value="201">201</option>
                <option value="301">301</option>
                <option value="501">501</option>
                <option value="701">701</option>
            </select>
        </div>
        <div>
            <label for="legs">Legs:</label>
            <input v-model.number="legs" type="number" id="legs" name="legs" min="1" max="20">
        </div>
        <div>
            <label for="sets">Sets:</label>
            <input v-model.number="sets" type="number" id="sets" name="sets" min="1" max="10">
        </div>
    </div>
    <div id="game-session" style="border: 1px solid black; width: 40%; float: left;">
        <!-- Game session information -->
        <label>3. resz asdasdadsfdsfdsvdfvdfvdfbgdfgfdgfdcdsgdfgdsgsdgffdfgdf</label>
    </div>
</template>

<script>
export default {
  data() {
    return {
      playerGameConnections: [],
      ownGames: [],
      players: [],
      userName: '',
      checkoutMethod: 'double',
      startPoint: 101,
      legs: 1,
      sets: 1,
    };
  },
  async created() {
    this.getPlayers();
  },
  methods: {
    async getData() {
      try {
        const response = await fetch("http://localhost:61231/api/PlayerGameConnection");
        const data = await response.json();
        this.playerGameConnections = data;
        this.userName = localStorage.getItem('username');
        console.log('Retrieved username from localStorage:', this.userName);

        const player = this.players.find(p => p.name === this.userName);
        if (player) {
          this.ownGames = this.playerGameConnections.filter(game => game.playerId === player.id);
        }
      } catch (error) {
        console.error("Failed to fetch data:", error);
      }
    },
    async getPlayers() {
      try {
        const response = await fetch("http://localhost:61231/api/Player");
        const data = await response.json();
        this.players = data;
        console.log(this.players);
      } catch (error) {
        console.error("Failed to fetch players:", error);
      }
    },
    getWinnerName(winnerId) {
      const winner = this.players.find(player => player.id === winnerId);
      return winner ? winner.name : 'Unknown';
    }
  }
};
</script>

<style scoped>
    /* Add your component-specific styles here */
</style>

