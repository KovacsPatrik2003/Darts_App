let playerGameConnections = [];
let ownGames = [];
let players = [];
let userName = '';
getPlayers();
async function getData() {
    
    await fetch("http://localhost:61231/api/PlayerGameConnection")
        .then(x => x.json())
        .then(y => {
            playerGameConnections = y;
            //console.log(playerGameConnections);
            userName = localStorage.getItem('username');
            console.log('Retrieved username from localStorage:', userName); // Debug statement
            // Assuming you have a way to map username to playerId
            let player = players.find(p => p.name === userName);
            if (player) {
                ownGames = playerGameConnections.filter(game => game.playerId == player.id);
            }
            displayOldGames();
            
        });
    
}

async function getPlayers() {
    await fetch("http://localhost:61231/api/Player")
        .then(x => x.json())
        .then(y => {
            players = y;
            console.log(players);
        });
}

function displayOldGames() {
    document.getElementById("oldGameResults").innerHTML = "";
    document.getElementById("oldGameResults").innerHTML += "<p>Actual player: " + userName +"</p>";
    ownGames.forEach(conn => {
        let p = players.filter(x => x.id == conn.game.winnerId);
        document.getElementById("oldGameResults").innerHTML += "<tr><td> ConnectionId: " + conn.id + "</td><td> PlayerId: "
            + conn.playerId + "</td><td> GameId: " + conn.gameId + "</td><td> Winner: " + p[0].name + "</td></tr>"
    });
}

