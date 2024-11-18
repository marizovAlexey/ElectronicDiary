async function createItem(params) {
    let elem = document.createElement("a")
    elem.role = "button"
    elem.href = `${urlApplicate}/viewclass.html?id=${params.id}&number=${params.number}&letter=${params.letter}`
    elem.style.textDecoration = "none"
    elem.text = params.number + params.letter
    return elem
}

(async () => {
    try {
        const response = await fetch(
            `${urlApplicate}/api/Class`,
            {
                method: 'GET'
            }
        )
        const json = await response.json()

        let root = document.getElementById("classesRoot")
        for (let i=0;i<json.length;i++) {
            root.append(await createItem(json[i]))
        }

    } catch (error) {
        console.error(error.message)
    }
})()