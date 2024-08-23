let playerGameConnections = [];
let ownGames = [];
let players = [];
let userName = '';
getPlayers();



setupSignalR();

function setupSignalR() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:61231/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("GameSessionStarted");
    
    connection.onclose(async () => {
        await start();
    });
    start();
}

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};





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



const frombodyPlayers = [
    {
        "Id": 1,
        "Name": "Player1",
        "Password": "password1",
        "GamesWinCount": 5,
        "GamesLoseCount": 3,
        "CurrentPoints": 10
    },
    {
        "Id": 2,
        "Name": "Player2",
        "Password": "password2",
        "GamesWinCount": 2,
        "GamesLoseCount": 6,
        "CurrentPoints": 8
    }
];

const gameSessionRequest = {
    Players: frombodyPlayers
};

async function StartGame() {
    await fetch('http://localhost:61231/api/Game/start-game-session/1/1/301/Straight%20Out', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(gameSessionRequest)
    })
        .then(response => response.json())
        .then(data => console.log(data))
        .catch(error => console.error('Error:', error));
}
StartGame();





