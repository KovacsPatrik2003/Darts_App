let playerGameConnections = [];
let ownGames = [];
let players=[];
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
            PlayersSelecter(players);
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



let frombodyPlayers = [];

const gameSessionRequest = {
    Players: frombodyPlayers
};

async function StartGame() {
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


function PlayersSelecter(playersFromServer){
    let selectHtml = '<label for="players-select">Játékosok:</label>' +
    '<select id="players-select-dropdown" name="jatekosok">';

    playersFromServer.forEach(player => {
        selectHtml += '<option value="'+player.id+'">' + player.name + '</option>';
    });

    selectHtml += '</select>';
    selectHtml+='<button onclick="AddPlayerToGame()">Add player</button>'
    // Állítsd be az innerHTML-t egyszer
    document.getElementById('players-select').innerHTML = selectHtml;
}

function AddPlayerToGame(){
    let p=document.getElementById('players-select-dropdown').value;
    if(frombodyPlayers.includes(p)){
        alert("Ezt a játékost már kiválasztottad.");
        throw new Error("Ezt a játékost már kiválasztottad.");
    }
    frombodyPlayers.push(p);
    console.log('jatekos: ',frombodyPlayers);
    ShowActualPlayers(frombodyPlayers);

}

function RemovePlayerFromTheGame(id){
    const index = frombodyPlayers.findIndex(player => player == id);
    console.log(index);
    if (index !== -1) {
        frombodyPlayers.splice(index, 1);
    }
    ShowActualPlayers(frombodyPlayers);
}

function ShowActualPlayers(selectedPlayers){
    document.getElementById("selected-players").innerHTML='';
    document.getElementById("selected-players").innerHTML+='<label>Kiválasztott játékosok: </br></label>';
    selectedPlayers.forEach(p => {
        document.getElementById("selected-players").innerHTML += "<tr><td>"+players.find(x=>x.id==p).name+' ' +'<button onclick="RemovePlayerFromTheGame('+p+')">Remove player</button></br></td></tr>'
    });
}

