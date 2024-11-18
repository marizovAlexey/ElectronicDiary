async function handleLogin(event) {
    event.preventDefault()
    console.log("fucking slaves!")
    document.getElementById("submitButton").ariaBusy = true
    const url = "http://localhost:5127/api/Account";
    const roleUrl = "http://localhost:5127/api/account/role";
    let usernameField = document.getElementById("usernameField")
    let passwordField = document.getElementById("passwordField")
    let data = {
        login: usernameField.value,
        password: passwordField.value
    }
    console.log(data)

    try {
        const response = await fetch(
            url,
            {
                method: 'POST',
                body: JSON.stringify(data),
                headers: {'Content-Type': 'application/json'}
            }
        )
        
        const json = await response.json()
        console.log('Success:', JSON.stringify(json));

        const roleResponse = await fetch(
            roleUrl,
            {
                method: 'GET'
            }
        )
        const roleId = await roleResponse.json();

        console.log(roleId.role);

        if (roleId.role != 2) {
            window.location.href = "http://localhost:5127/classes.html";
        }
        else {
            window.location.href = "http://localhost:5127/student.html";
        }
        
    } catch (error) {
        console.error("Caught error on login: ", error.message)
    }
}

(async()=>{
    document.getElementById("loginForm").addEventListener("submit", handleLogin)
})()