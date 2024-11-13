(async () => {
    let url = window.location.href.slice();
    try {
        const response = await fetch(
            "http://localhost:5127/api/Class",
            {
                method: 'GET'
            }
        )
        const json = response.json()
        console.log('Success:', JSON.stringify(json));
    } catch (error) {
        console.error(error.message)
    }

})()