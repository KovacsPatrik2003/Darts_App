function SignUp() {
    let name = document.getElementById("userName").value;
    let password = document.getElementById("userPassword").value;
    let passwordAgain = document.getElementById("userPasswordAgain").value;
    if (password==passwordAgain) {
        fetch('http://localhost:61231/api/Player/', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json', },
            body: JSON.stringify(
                { name: name, password: password })
        })
            .then(response => response)
            .then(data => {
                console.log('Success:', data);
                window.location.href = 'LogIn.html';
                
            })
            .catch((error) => { console.error('Error:', error); });
    }
    else {
        alert("The passwords did not match.");
    }
    
}