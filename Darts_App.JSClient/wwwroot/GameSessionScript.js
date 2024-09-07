let playerGameConnections = [];
let ownGames = [];
let players = [];
let userName = '';

let setCount=0;
let legCount=0;
let startPoint=0;
let checkOutMethod='double';
let scoredPoint=0;
getPlayers();



const connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:61231/hub")
    .build();

// Amikor a backend kéri az adatot
// connection.on("RequestData", async (message) => {
//     console.log(message); // A backend üzenete
//     let userData = prompt("Kérlek add meg az adatot:");

//     // Az adat elküldése a backendnek
//     await connection.invoke("SendData", userData);
// });

// Kapcsolat indítása
connection.start().then(() => {
    console.log("Kapcsolat létrejött");
}).catch((err) => console.error(err));


document.getElementById("sendDataButton").addEventListener("click", function () {
    // Kérünk be egy adatot a felhasználótól
    let userData = prompt("Kérlek, add meg az adatot, amit el szeretnél küldeni:");
    
    if (userData) {
        // Adat küldése a SignalR szervernek
        connection.invoke("SendData", userData)
            .then(() => {
                console.log("Adat elküldve:", userData);
            })
            .catch(err => console.error(err.toString()));
    }
});
connection.on("DataReceived", function (message) {
    console.log("Válasz a szervertől:", message);
    alert("Válasz a szervertől: " + message);
});

//kovi code-ja
// setupSignalR();

// function setupSignalR() {
//     connection = new signalR.HubConnectionBuilder()
//         .withUrl("http://localhost:61231/hub")
//         .configureLogging(signalR.LogLevel.Information)
//         .build();

//     connection.on("GameSessionStarted", function (){
//         console.log("Game session started!");
//     });

//     connection.on("ThrowHappend", function() {
//         console.log("Throw happened!");
//     });
    
//     connection.onclose(async () => {
//         await start();
//     });
//     start();
// }

// async function start() {
//     try {
//         await connection.start();
//         console.log("SignalR Connected.");
//     } catch (err) {
//         console.log(err);
//         setTimeout(start, 5000);
//     }
// };









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
    console.log('Gomb megnyomva');
    await fetch('http://localhost:61231/api/Game/start-game-session/'+setCount+'/'+legCount+'/'+startPoint+'/'+checkOutMethod , {
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


function GetGameSessionInitDatas(){
    checkOutMethod = document.getElementById('checkout-method').value;
    startPoint=document.getElementById('start-point').value;
    legCount=document.getElementById('legs').value;
    setCount=document.getElementById('sets').value;
    StartGame();
}

async function ScoredPoints(){
    scoredPoint=document.getElementById('throw').value;
    await fetch('http://localhost:61231/api/Game/points/'+scoredPoint, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    .then(response => {
        if (!response.ok) {
            throw new Error('Network response was not ok');
        }
        return response.text(); 
    })
    .then(text => {
        const data = text ? JSON.parse(text) : {}; 
        console.log(data);
    })
    .catch(error => console.error('Error:', error));
}




