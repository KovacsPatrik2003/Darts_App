let player = [];

async function LogIn() {
    let userName = document.getElementById("userName").value;
    let password = document.getElementById("userPassword").value;
    await fetch("http://localhost:61231/api/Player/" + userName + "/" + password)
        .then(x => {
            if (!x.ok) {
                return x.json().then(error => {
                    throw new Error(error.message);
                });
            }
            return x.json();
        })
        .then(y => {
            player = y;
            console.log(player);
            localStorage.setItem('username', userName);
            console.log('Username stored in localStorage:', localStorage.getItem('username')); 
            window.location.href = 'GameStation.html';

        })
        .catch(error => {
            console.error('Login failed:', error.message);
            alert('Login failed');
        });

}


