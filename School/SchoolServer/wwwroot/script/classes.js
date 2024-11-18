async function createItem(params) {
    let elem = document.createElement("a")
    elem.role = "button"
    elem.href = `http://localhost:5127/viewclass.html?id=${params.id}&number=${params.number}&letter=${params.letter}`
    elem.style.textDecoration = "none"
    elem.text = params.number + params.letter
    return elem
}

(async () => {
    let url = window.location.href.slice();
    try {
        const response = await fetch(
            "http://localhost:5127/api/Class",
            {
                method: 'GET'
            }
        )
        // console.log(response.text())
        const json = await response.json()
        console.log('Success:', json);

        let root = document.getElementById("classesRoot")
        for (let i=0;i<json.length;i++) {
            root.append(await createItem(json[i]))
        }

    } catch (error) {
        console.error(error.message)
    }

})()