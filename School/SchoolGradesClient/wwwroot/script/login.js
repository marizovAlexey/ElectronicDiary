async function handleLogin(event) {
    event.preventDefault();

    const url = window.location.href.slice();
    let usernameField = document.getElementById("usernameField")
    let passwordField = document.getElementById("passwordField")
    let data = {
        username: usernameField.getAttribute("value"),
        password: passwordField.getAttribute("value")
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
        window.location.href = "/classes";
    } catch (error) {
        console.error(error.message)
    }
}