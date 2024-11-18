var urlApplicate = "https://localhost:44385"

async function handleLogin(event) {
    event.preventDefault()
    document.getElementById("submitButton").ariaBusy = true
    const url = `${urlApplicate}/api/Account`;
    const roleUrl = `${urlApplicate}/api/account/role`;
    let usernameField = document.getElementById("usernameField")
    let passwordField = document.getElementById("passwordField")
    let data = {
        login: usernameField.value,
        password: passwordField.value
    }

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

        if (!roleId.role) {
            alert(JSON.stringify(json));
            document.getElementById("submitButton").ariaBusy = false;
            return;
        }

        if (roleId.role != 2) {
            window.location.href = `${urlApplicate}/classes.html`;
        }
        else {
            window.location.href = `${urlApplicate}/student.html`;
        }
        
    } catch (error) {
        console.error("Caught error on login: ", error.message)
    }
}

(async()=>{
    document.getElementById("loginForm").addEventListener("submit", handleLogin)
})()